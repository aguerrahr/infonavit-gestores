using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using OAuthLibrary.Converters;
using OAuthLibrary.Services;
using System.Collections.Generic;

namespace OAuthLibrary.Middleware
{
    public class OAuthMiddleware
    {
        private static readonly string AUTHORIZATION_HEADER = "Authorization";
        private static readonly string REFERER_HEADER = "Referer";

        private readonly ILogger<OAuthMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly OAuthService _oauthService;
        private readonly String[] _pathsWithoutAuthoeizaion;



        public OAuthMiddleware(RequestDelegate next, string path, string baseApi, String [] pathsWithoutAuthoeizaion, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<OAuthMiddleware>();
            _oauthService = new OAuthService(new Uri(path), baseApi);
            _pathsWithoutAuthoeizaion = pathsWithoutAuthoeizaion;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("In OAuthMidleware");
            try
            {
                var resource = context.Request.Path.Value.Substring(1);
                _logger.LogInformation("Resource to be validated: " + resource);
                if(_pathsWithoutAuthoeizaion != null && Array.Exists(_pathsWithoutAuthoeizaion, r => r == resource))
                {
                    _logger.LogInformation("Resource authorized.");
                    await _next.Invoke(context);
                    return;
                }
                var method = context.Request.Method;
                string ip = context.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                var hasAuthorization = context.Request.Headers.ContainsKey(AUTHORIZATION_HEADER);
                var hasReferer = context.Request.Headers.ContainsKey(REFERER_HEADER);
                var isLogin = Array.Exists(new[] { "login", "sigin" }, item => resource.ToLower().Contains(item));
                var isLogout = Array.Exists(new[] { "logout", "sigout" }, item => resource.ToLower().Contains(item));

                if (!hasReferer && !isLogin)
                {
                    context.Response.StatusCode = 403;
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(REFERER_HEADER + " header is required."));
                    await ms.CopyToAsync(context.Response.Body);
                    return;
                }
                else if (!hasAuthorization)
                {
                    if (isLogin)
                    {
                        context.Response.Headers.Add(REFERER_HEADER, resource);
                        var ms = new MemoryStream(Encoding.UTF8.GetBytes(AUTHORIZATION_HEADER + " header is required."));
                        context.Response.StatusCode = 403;
                        await ms.CopyToAsync(context.Response.Body);
                        return;
                    }
                    _logger.LogInformation("Redirected from [" + resource + "] to [" + context.Request.Headers[REFERER_HEADER] + "] ...");
                    context.Response.Headers.Add(REFERER_HEADER, resource);
                    context.Response.Redirect(context.Request.Headers[REFERER_HEADER]);
                    return;
                }
                else if (isLogout)
                {
                    if (!method.Equals("DELETE"))
                    {
                        context.Response.StatusCode = 405;
                        var ms = new MemoryStream(Encoding.UTF8.GetBytes("Method received is not allowed for logout."));
                        await ms.CopyToAsync(context.Response.Body);
                        return;
                    }
                    _logger.LogInformation("Getting rid of token...");
                    if (await _oauthService.IsLogout(context.Request.Headers[AUTHORIZATION_HEADER], ip))
                    {
                        context.Response.Headers.Remove(AUTHORIZATION_HEADER);
                        context.Response.StatusCode = 204;
                        context.Response.Headers.Add(REFERER_HEADER, resource);
                        return;
                    }
                    context.Response.StatusCode = 401;
                    return;
                }
                else if (isLogin)
                {
                    if (!method.Equals("POST"))
                    {
                        context.Response.StatusCode = 405;
                        var ms = new MemoryStream(Encoding.UTF8.GetBytes("Method received is not allowed for login."));
                        await ms.CopyToAsync(context.Response.Body);
                        return;
                    }
                    _logger.LogInformation("authenticating...");
                    var responseAuthorize = await _oauthService.Login(context.Request.Headers[AUTHORIZATION_HEADER], ip);
                    if (null != responseAuthorize && null != responseAuthorize.ConsumerKey && null == responseAuthorize.Token)
                    {
                        context.Response.StatusCode = 403;
                        var ms = new MemoryStream(Encoding.UTF8.GetBytes("Usuario no Registrado."));
                        await ms.CopyToAsync(context.Response.Body);
                        return;
                    }
                    if (null != responseAuthorize && null == responseAuthorize.ConsumerKey && null == responseAuthorize.Token)
                    {
                        context.Response.StatusCode = 404;
                        var ms = new MemoryStream(Encoding.UTF8.GetBytes("El empleado soliciado no esta activo."));
                        await ms.CopyToAsync(context.Response.Body);
                        return;
                    }
                    if (null != responseAuthorize)
                    {

                        context.Items.Add("IN", responseAuthorize.ConsumerKey);
                        context.Response.Headers.Remove(AUTHORIZATION_HEADER);
                        _logger.LogInformation("return Base64 in:token...");
                        context.Response.Headers.Add(
                            AUTHORIZATION_HEADER, Base64Converter.BasicEncode(
                                new KeyValuePair<string, string>(responseAuthorize.ConsumerKey, responseAuthorize.Token)));
                        await _next.Invoke(context);
                        return;
                    }
                    context.Response.StatusCode = 401;
                    return;
                }
                _logger.LogInformation("validate token and resource[" + resource + "] ...");
                if (await _oauthService.IsResourceAuthorized(context.Request.Headers[AUTHORIZATION_HEADER], ip, resource))
                {
                    _logger.LogInformation("Resource authorized.");
                    await _next.Invoke(context);
                    return;
                }
                context.Response.StatusCode = 401;
                return;
            }
            catch (Exception ex) {
                _logger.LogInformation("Exception: " + ex.GetType() + ", Message: " + ex.Message);
                context.Response.StatusCode = 503;
                var msg = new MemoryStream(
                    Encoding.UTF8.GetBytes("The server is not ready to handle the request: " + ex.Message));
                await msg.CopyToAsync(context.Response.Body);
                return;
            }
        }

    }
}
