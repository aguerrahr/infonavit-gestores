using System;
using System.Linq;
using OAuthLibrary.DTO;

namespace OAuthLibrary.Converters
{
    public abstract class OAuthConverter
    {
        public static readonly string OAUTH_TOKEN = "OAuth";
        private static char SEPARATOR_BODY = '&';
        private static char SEPARATOR_HEADER = ',';
        public static string serializeToHeader(OAuthDTO dto) =>
            string.Format("oauth_consumer_key={0},oauth_signature={1},oauth_token={2},consumer_ip={3},realm={4}",
                dto.ConsumerKey, dto.Signature, dto.Token, dto.IPClient, dto.Resource);

        public static OAuthDTO deserializeBody(string body)
        {
            OAuthDTO oAuthDto = new OAuthDTO();
                oAuthDto.ConsumerKey = deserializeField("oauth_consumer_key", body, SEPARATOR_BODY);
                oAuthDto.Signature = deserializeField("oauth_signature", body, SEPARATOR_BODY);
                oAuthDto.Token = deserializeField("oauth_token", body, SEPARATOR_BODY);
                oAuthDto.Resource = deserializeField("realm", body, SEPARATOR_BODY);
            return oAuthDto;
        }

        public static OAuthDTO deserializeHeader(string rawHeader)
        {
            string header = rawHeader.Substring(OAUTH_TOKEN.Length + 1);
            //string.Format("OAuth oath_consumer_key=\"{0}\",oath_signature_method=\"{1}\",oath_timestamp=\"{3}\",oath_nonce=\"{4}\",oath_version=\"{5}\",oath_signature=\"{2}\"", _consumerKey, OAuthSignatureMethod.HmacSha1, GenerateSignature(), GenerateTimeStamp(), GenerateNonce(), Version);
            OAuthDTO oAuthDto = new OAuthDTO();
            oAuthDto.ConsumerKey = deserializeField("oauth_consumer_key", header, SEPARATOR_HEADER);
            oAuthDto.Signature = deserializeField("oauth_signature", header, SEPARATOR_HEADER);
            oAuthDto.Token = deserializeField("oauth_token", header, SEPARATOR_HEADER);
            oAuthDto.Resource = deserializeField("realm", header, SEPARATOR_HEADER);
            oAuthDto.IPClient = deserializeField("consumer_ip", header, SEPARATOR_HEADER);
            return oAuthDto;
        }

        private static string deserializeField(string fieldName, string oAuthData, char separator) =>
            oAuthData.Split(separator)
                .AsEnumerable()
                .Where(data => {
                        var keyValue = data.Split('=');
                        return (2 <= keyValue.Length && fieldName.Equals(keyValue[0], StringComparison.OrdinalIgnoreCase));
                    })
                .Select(data => data.Split('=')[1])
                .FirstOrDefault();
    }
}
