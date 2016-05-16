using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;

namespace SN.Vk
{
    public class VkSocialApi
    {
        private readonly VkApi _vkApi;

        public VkSocialApi(VkApi vkApi)
        {
            _vkApi = vkApi;
        }

        public async void Auth()
        {
            await _vkApi.AuthorizeAsync(new ApiAuthParams
            {
                
            });
        }
    }
}
