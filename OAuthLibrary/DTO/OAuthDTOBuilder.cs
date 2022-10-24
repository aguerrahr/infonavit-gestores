using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthLibrary.DTO
{
    public class OAuthDTOBuilder
    {
        private readonly OAuthDTO _dto;

        public OAuthDTOBuilder()
        {
            _dto = new OAuthDTO();
        }
        public OAuthDTOBuilder(OAuthDTO dto)
        {
            _dto = dto;
        }

        public OAuthDTO build()
        {
            return _dto;
        }

        public OAuthDTOBuilder ConsumerKey(string consumerKey)
        {
            _dto.ConsumerKey = consumerKey;
            return this;
        }
        public OAuthDTOBuilder IPClient(string iPClient)
        {
            _dto.IPClient = iPClient;
            return this;
        }
        public OAuthDTOBuilder Resource(string resource)
        {
            _dto.Resource = resource;
            return this;
        }
        public OAuthDTOBuilder Signature(string signature)
        {
            _dto.Signature = signature;
            return this;
        }
        public OAuthDTOBuilder Token(string token)
        {
            _dto.Token = token;
            return this;
        }
    }
}
