using System.Collections.Generic;
using EnterpriseApp.API.Data.ViewModels;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetNewChatResponse
    {
        [JsonProperty("items")]
        public List<ChatModel> Items { get; set; }
    }
}