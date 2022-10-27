using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthenticationAPI.Models.Contexts;
using AuthenticationAPI.Models;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : OAuthController
    {
        private readonly int MAX_MINUTES_TO_EXPIRE = 15;
        private readonly ILogger<AuthenticationController> _logger;
        
        public AuthenticationController(AuthenticationAPIContext context, ILogger<AuthenticationController> logger, IConfiguration configuration) 
            : base(context, configuration)
        {
            _logger = logger;
        }

        /** The client obtains a set of temporary credentials from the server. */
        [HttpGet("initiate")]
        public IActionResult Initiate() =>
            this.execute((context, oAuthDto, employee) =>
            {
                string token = GenerateToken(oAuthDto.ConsumerKey);
                if (null == employee.Authentication)
                {
                    Authentication authentication = new Authentication
                    {
                        ID = employee.ID,
                        Enabled = employee.Enabled,
                        IN = employee.IN,
                        Attempts = 0,
                        TemporalToken = token,
                        Token = String.Empty,
                        IP = oAuthDto.IPClient,
                        Employee = employee,
                        AuthenticatedAt = DateTime.Now,
                    };
                    context.Authentications.Add(authentication);
                    employee.Authentication = authentication;
                }
                else
                {
                    Authentication authentication = employee.Authentication;
                    authentication.Attempts = 0;
                    authentication.AuthenticatedAt = DateTime.Now;
                    authentication.TemporalToken = token;
                    authentication.Token = String.Empty;
                    authentication.IP = oAuthDto.IPClient;
                }
                context.SaveChanges();
                return string.Format("oauth_consumer_key={0}&oauth_token={1}&oauth_callback_confirmed=true",
                    oAuthDto.ConsumerKey, token);
            });

        /**Resource Owner Authorization URI. */
        [HttpPost("authorize")]
        public IActionResult Authorize() =>
            this.execute((context, oAuthDto, employee) =>
            {
                var messageSignature = String.Empty;
                if (
                    null != employee.Authentication &&
                    !String.IsNullOrEmpty(employee.Authentication.TemporalToken) &&
                    employee.Authentication.TemporalToken.Equals(oAuthDto.Token) &&
                    3 >= (employee.Authentication.Attempts + 1)
                    )
                {
                    messageSignature = signatureIsCorrect(oAuthDto.ConsumerKey, oAuthDto.Signature, employee.Job.Group);
                    if (messageSignature.Equals("Authorized"))
                    {
                        employee.Authentication.Attempts = 0;
                        employee.Authentication.AuthenticatedAt = DateTime.Now;
                        employee.Authentication.TemporalToken = String.Empty;
                        employee.Authentication.Token = GenerateToken(employee.IN);
                        context.SaveChanges();
                        return string.Format("oauth_consumer_key={0}&oauth_token={1}",
                            oAuthDto.ConsumerKey, employee.Authentication.Token);
                    }
                }
                employee.Authentication.Attempts += 1;
                context.SaveChanges();
                return messageSignature;
            });
      
        /**Ask for rights in the resource such as:
         * Authorization: OAuth realm="api/Gerente",
             oauth_consumer_key="dpf43f3p2l4k3l03",
             oauth_token="74KNZJeDHnMBp0EMJ9ZHt%2FXKycU%3D"*/
        [HttpPost("token")]
        public IActionResult Validate() =>
            this.execute((context, oAuthDto, employee) =>
            {
                if (null != employee.Authentication &&
                    !String.IsNullOrEmpty(employee.Authentication.Token) &&
                    employee.Authentication.Token.Equals(oAuthDto.Token) &&
                    employee.Authentication.IP.Equals(oAuthDto.IPClient) &&
                    null != employee.Roles &&
                    employee.Roles.Any())
                {
                    if (DateTime.Now.Subtract(employee.Authentication.AuthenticatedAt).TotalMinutes > MAX_MINUTES_TO_EXPIRE) {
                        employee.Authentication.Attempts = 0;
                        employee.Authentication.AuthenticatedAt = DateTime.Now;
                        employee.Authentication.TemporalToken = String.Empty;
                        employee.Authentication.Token = String.Empty;
                        employee.Authentication.IP = oAuthDto.IPClient;
                        context.SaveChanges();
                        return String.Empty;
                    }
                    //validate resource received
                    var resources = context.Resources
                        .Where(res => 
                            oAuthDto.Resource.ToLower().Contains(res.Path.ToLower()))
                        .ToList();
                    var rolesResource = resources
                        .SelectMany(r => r.Roles)
                        .ToList();
                    if (null != rolesResource &&
                        rolesResource.Any() &&
                        rolesResource
                            .Select(r => r.Name)
                            .Intersect<String>(employee.Roles.Select(r => r.Name))
                        .Any())
                    {
                        employee.Authentication.AuthenticatedAt = DateTime.Now;
                        context.SaveChanges();
                        return string.Format("oauth_consumer_key={0}&oauth_token={1}",
                            oAuthDto.ConsumerKey, oAuthDto.Token);
                    }
                }
                return String.Empty;
            });


        [HttpDelete("token")]
        public IActionResult Expire() =>
            this.execute((context, oAuthDto, employee) =>
            {
                if (null != employee.Authentication &&
                    employee.Authentication.Token.Equals(oAuthDto.Token) &&
                    employee.Authentication.IP.Equals(oAuthDto.IPClient))
                {
                    employee.Authentication.Attempts = 0;
                    employee.Authentication.AuthenticatedAt = DateTime.Now;
                    employee.Authentication.TemporalToken = String.Empty;
                    employee.Authentication.Token = String.Empty;
                    employee.Authentication.IP = oAuthDto.IPClient;
                    context.SaveChanges();
                    return string.Format("oauth_consumer_key={0}", oAuthDto.ConsumerKey);
                }
                return String.Empty;
            });        
    }
}
