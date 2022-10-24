using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;

using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

using GestoresAPI.Models;
using GestoresAPI.Models.Contexts;


namespace GestoresAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignPayloadController : ControllerBase
    {
        private readonly ILogger<SignPayloadController> _logger;
        private readonly GestoresAPIContext context;
        private static readonly string publickey = "B10Esp@.";
        private static readonly string secretkey = "daff4998%305a#492d-9557$d85f5e6cb0daa9746ff7?14a1!4094|8c01<7add5203e988a6c9504c>f53e[4f4c/ba5d'e1f4f42dcbe0955e79ed.4190(4d3d+a" +
               "e45-1982447b8c1427ad65af>ce15&4c07=9099,73236c5e79b0e6ae878:e88ai46ed}b5a6&d7b5c31a2e4bcf6e1352-2d24-4927-990a-4323f40ee6a1ca425" +
               "11d-6cbc-4b90-a49e-4633c6f347a9f603b78b-2432-4e2a-bd46-ce7069a4836c00d0e458-ecf7-454d-9e9e-d99dc613ce2d7207d89b-55a0-4864-a31e-0" +
               "797cd72e149ebeaa875-3e47-4f39-bb35-48617a7b83e817b994f7-cc24-4e33-8a71-547ce8e2d2b0e3e9be4d-3a42$4bd4-8f41$f2571c39bcf3d3fd0baxx";
        private IConfiguration configurationString { get; }
        //private IConfigurationRoot Configuration { get; set; }
        public SignPayloadController(GestoresAPIContext context, ILogger<SignPayloadController> logger ,IConfiguration configurationString)
        {
            this.context = context;
            _logger = logger;
            this.configurationString = configurationString;
        }
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult FirmarPayload(CallBioSphere ObjetoEnviado)
        {
            _logger.LogInformation("Ini getSign: ");
            SendObject ObjSend = new SendObject();
            
            ObjSend.Sign = CalculateSignature(ObjetoEnviado.PayLoad); 
            return new JsonResult(ObjSend);
        }

        private String CalculateSignature(String payloadJson)
        {
            
            string clientSecret = configurationString.GetValue<string>("ConfigStringSign"); 

            if (clientSecret == null)
                throw new Exception("La aplicación cliente no ha sido configurada.");
            StringBuilder sb = new StringBuilder(Decrypt(clientSecret));
            sb.Append(payloadJson);
            var hash = new SHA256Managed().ComputeHash(
                Encoding.UTF8.GetBytes(sb.ToString()));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        private string Encrypt(string textToEncrypt)
        {
            try
            {
                string ToReturn = "";             
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private string Decrypt(string textToDecrypt)
        {
            try
            {
                string ToReturn = "";                
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }

    }
    public class SendObject 
    { 
        public string Sign { get; set; }
    }
   
}
