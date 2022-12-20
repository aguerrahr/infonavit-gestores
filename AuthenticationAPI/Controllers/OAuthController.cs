using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationAPI.Models.Contexts;
using AuthenticationAPI.Models;
using OAuthLibrary.DTO;
using RestSharp;
using System.Net;
using OAuthLibrary.Converters;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using RestSharp.Validation;
//using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
//using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace AuthenticationAPI.Controllers
{
    public class OAuthController : ControllerBase
    {
        private static readonly string AUTHORIZATION_HEADER = "Authorization";
        private static readonly string AUTHORIZATION_TYPE_OAUTH = "OAuth ";
        private static readonly string CONTENT_TYPE_FORM = "application/x-www-form-urlencoded";
        private static readonly string ERROR_AUTHORIZATION_NOT_FOUND = "Header Authorization not found.";
        //private static readonly string INFONVIT_AUTENTICATION_URL = "https://serviciosweb.infonavit.org.mx:8892/AutenticaQa-web/api/autenticaService/autenticarUsuarioGrupo";
        //private static readonly string INFONVIT_AUTENTICATION_URL = "https://10.85.6.28:9443/AutenticaQa-web/api/autenticaService/autenticarUsuarioGrupo";
        //private static readonly string INFONVIT_AUTENTICATION_URL = "https://10.85.3.10:9443/AutenticaDev-web/api/autenticaService/login";        
        //private static readonly string INFONVIT_AUTENTICATION_URL = "https://10.85.6.28:9443/AutenticaQa-web/api/autenticaService/login";

        private readonly AuthenticationAPIContext context;

        private IConfiguration Configuration { get; }

        //public OAuthController(IConfiguration configuration)
        //{

        //    Configuration = configuration;
        //}

        public OAuthController(AuthenticationAPIContext context, IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }
        
        protected IActionResult execute(Func<AuthenticationAPIContext, OAuthDTO, Employee, string> action) {
            string authHeader = Request.Headers[AUTHORIZATION_HEADER];
            if (null == authHeader || !authHeader.StartsWith(AUTHORIZATION_TYPE_OAUTH, StringComparison.OrdinalIgnoreCase))
            {
                return StatusCode(401, ERROR_AUTHORIZATION_NOT_FOUND);
            }
            OAuthDTO oAuthDto = OAuthConverter.deserializeHeader(authHeader);
            var employee = this.context.Employees.FirstOrDefault(f => f.IN.Equals(oAuthDto.ConsumerKey));
            //add session expired
            if (null == employee)
            {
                return StatusCode(403, "Empleado Invalido");
            }
            if (!employee.Enabled ||
                (null != employee.Authentication &&
                !employee.Authentication.Enabled))
            {
                return StatusCode(409, "Empleado Inactivo.");
            }

            string responseBody = action(this.context, oAuthDto, employee);
            if (String.Empty.Equals(responseBody))
            {
                return Unauthorized();
            }
            if (!responseBody.Contains("oauth_consumer_key"))
            {
                return StatusCode(403, responseBody);
            }
            Response.ContentType = CONTENT_TYPE_FORM;
            Response.StatusCode = Ok().StatusCode;
            Response.WriteAsync(responseBody);
            Response.CompleteAsync();
            return new EmptyResult();
        }

        public static string GenerateToken(string consumerKey)
        {
            byte[] key = Encoding.ASCII.GetBytes(DateTime.UtcNow.ToString(new CultureInfo("ru-RU")));
            return Encode(consumerKey, key).Substring(0, 30);
        }

        private static string Encode(string input, byte[] key)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            //sharedSecret HMAC-SHA1
            using (var myhmacsha1 = new HMACSHA1(key))
            {
                var hashArray = myhmacsha1.ComputeHash(byteArray);
                return hashArray.Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            }
        }

        //private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        //{
        //    //Console.WriteLine(certificate);
        //    return true;
        //}

    protected String signatureIsCorrect(string consumerKey, string signature, string group) {
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //var client = new RestClient(this.Configuration.GetConnectionString("URL_AUTH"));
            string INFONVIT_AUTENTICATION_URL = this.Configuration.GetConnectionString("URL_AUTH");
            var client = new RestClient(INFONVIT_AUTENTICATION_URL);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true; //Se agreaga esta linea para saltar el SSL            
            Funciones.FuncionesUtiles.LogToFile("INFONVIT_AUTENTICATION_URL = " + INFONVIT_AUTENTICATION_URL);
            int retry = 3;
            var textResponse = String.Empty;
            do
            {
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept","*/*"); //Se agreaga esta linea para saltar el SSL
                request.AlwaysMultipartFormData = true; 
                Funciones.FuncionesUtiles.LogToFile("usuario: " + consumerKey);
                Funciones.FuncionesUtiles.LogToFile("password: " + signature);
                Funciones.FuncionesUtiles.LogToFile("grupo: " + group);
                request.AddParameter("usuario", consumerKey);
                request.AddParameter("password", signature);
                request.AddParameter("grupo", group);                
                //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                IRestResponse response = client.Execute(request);
                Funciones.FuncionesUtiles.LogToFile((response.ErrorException == null?"---": response.ErrorException.ToString()));
                Funciones.FuncionesUtiles.LogToFile((response.ErrorMessage == null?"---": response.ErrorMessage));
                textResponse = response.Content;
                Funciones.FuncionesUtiles.LogToFile(textResponse);
                if (textResponse.Equals("Usuario inexistente")) 
                {
                    retry = 1;
                }
                if (textResponse.Equals("Password verificado: " + consumerKey))
                {
                    return "Authorized";
                }
                retry--;
            } while (retry > 0);
            return textResponse;
        }
    }
}
