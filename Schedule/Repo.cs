using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;


namespace TelegramBot.Schedule
{

    internal class Repo
    {
        public async Task<Root> ScheduleGet()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://rickandmortyapi.com/api/character"); 
            var content = new MultipartFormDataContent();
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(json);
            return myDeserializedClass;
           
        
          
        }
    }
}
