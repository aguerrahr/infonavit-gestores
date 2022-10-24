using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthLibrary.DTO
{
    public class OAuthDTO
    {
        public OAuthDTO() {
        }
        public string ConsumerKey { get; set; }
        public string Signature { get; set; }
        public string IPClient { get; set; }
        public string Token { get; set; }
        public string Resource { get; set; }

        public static OAuthDTOBuilder Builder()
        {
            return new OAuthDTOBuilder();
        }

        public static OAuthDTOBuilder Builder(OAuthDTO dto)
        {
            return new OAuthDTOBuilder(dto);
        }
    }

}
