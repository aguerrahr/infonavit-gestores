using OAuthLibrary.Converters;
using OAuthLibrary.DTO;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OAuthLibrary.Services
{

    public class OAuthService
    {
        private static readonly string API_TOKEN = "/token";
        private static readonly string API_INITIATE = "/initiate";
        private static readonly string API_AUTHORIZE = "/authorize";
        private readonly Uri _url;
        private readonly string _basePath;

        public OAuthService(Uri url, string basePath)
        {
            _url = url;
            _basePath = basePath;
        }

        public async Task<bool> IsLogout(string autorizationHeader, string ip)
        {
            var decodedLogoutHeader = Base64Converter.BasicDecode(autorizationHeader);
            if(null == decodedLogoutHeader.Key || null == decodedLogoutHeader.Value)
                return false;
            var expireResponse = await Call("delete", $"{_basePath}{API_TOKEN}",
                OAuthDTO.Builder()
                    .ConsumerKey(decodedLogoutHeader.Key)
                    .Token(decodedLogoutHeader.Value)
                    .IPClient(ip)
                .build());
            return null != expireResponse;
        }
        public async Task<OAuthDTO> Login(string autorizationHeader, string ip)
        {
            var decodedLoginHeader = Base64Converter.BasicDecode(autorizationHeader);
            if (null == decodedLoginHeader.Key || null == decodedLoginHeader.Value)
                return null;
            //get temporal token from: api/Authentication/initiate
            var responseInit = await Call("get", $"{_basePath}{API_INITIATE}",
                OAuthDTO.Builder()
                    .ConsumerKey(decodedLoginHeader.Key)
                    .IPClient(ip)
                .build());
            if (null == responseInit)
                return null;
            //authenticate
            var responseAuthorize = await Call("post", $"{_basePath}{API_AUTHORIZE}",
                OAuthDTO.Builder(responseInit)
                    .Signature(decodedLoginHeader.Value)
                .build());
            return responseAuthorize;
        }
        public async Task<bool> IsResourceAuthorized(string autorizationHeader, string ip, string resource)
        {
            var decodedHeader = Base64Converter.BasicDecode(autorizationHeader);
            if (null == decodedHeader.Key || null == decodedHeader.Value)
                return false;
            //validate token and resource on: api/Authentication/token
            var responseToken = await Call("post", $"{_basePath}{API_TOKEN}",
                OAuthDTO.Builder()
                    .ConsumerKey(decodedHeader.Key)
                    .Token(decodedHeader.Value)
                    .IPClient(ip)
                    .Resource(resource)
                .build());
            return null != responseToken;
        }
        public async Task<OAuthDTO> Call(string method, string path, OAuthDTO oauthHeader)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _url;
                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue(OAuthConverter.OAUTH_TOKEN, OAuthConverter.serializeToHeader(oauthHeader));
                var result = await
                    ("get".Equals(method, StringComparison.OrdinalIgnoreCase)
                        ? client.GetAsync(path)
                    : "delete".Equals(method, StringComparison.OrdinalIgnoreCase)
                        ? client.DeleteAsync(path)
                        : client.PostAsync(path, new StringContent(string.Empty)));
                if (HttpStatusCode.Forbidden == result.StatusCode)
                {
                    return OAuthDTO.Builder()
                            .ConsumerKey(oauthHeader.ConsumerKey)
                        .build();
                }
                if (HttpStatusCode.NotFound == result.StatusCode)
                {
                    return OAuthDTO.Builder().build();
                }
                if (result.IsSuccessStatusCode)
                {
                    string responseBody = await result.Content.ReadAsStringAsync();
                    return OAuthConverter.deserializeBody(responseBody);
                }
                return null;
            }
        }
    }
}
