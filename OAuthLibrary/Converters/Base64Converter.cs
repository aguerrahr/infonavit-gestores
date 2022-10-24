using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace OAuthLibrary.Converters
{
    public abstract class Base64Converter
    {
        public static readonly string BASIC_TOKEN = "Basic";
        public static readonly char SEPARATOR = ':';
        public static readonly ILogger<Base64Converter> _logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<Base64Converter>();
        public static String BasicEncode(KeyValuePair<string, string> pair) =>
            $"{BASIC_TOKEN} {Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", pair.Key, pair.Value)))}";
        public static KeyValuePair<string, string> BasicDecode(string str)
        {
            try
            {
                var decodedBytes = Convert.FromBase64String(str.Substring(BASIC_TOKEN.Length + 1));
                string decodedStr = Encoding.ASCII.GetString(decodedBytes, 0, decodedBytes.Length);
                return new KeyValuePair<string, string>(
                    decodedStr.Substring(0, decodedStr.IndexOf(SEPARATOR)),
                    decodedStr.Substring(decodedStr.IndexOf(SEPARATOR) + 1));
            }
            catch (Exception e) {
                _logger.LogError("Error decoding base64 string received.", e);
                return new KeyValuePair<string, string>();
            }
        }
    }
}
