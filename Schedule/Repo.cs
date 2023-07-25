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
        public async Task<Root> ScheduleGet(string station)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.rasp.yandex.net/v3.0/schedule/?apikey=37a6c983-d404-4790-9c5d-4b795e3f62f9&station={station}&transport_types=plane&system=iata"); 
            request.Headers.Add("Authorization", "Bearer ZDk4ODM3NzktMjAz");
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
