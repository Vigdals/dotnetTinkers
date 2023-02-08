using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using dotnetTinkers.Models;

namespace dotnetTinkers.Resources
{
    public class ApiCall
    {
        public static string DoApiCall(string apiURL)
        {
            using var client = new HttpClient();
            // Get data response
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(apiURL).Result;
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            return stringResponse;
        }

        public static void CheckIfSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body
                //Console.WriteLine(apiResultAsModel.StringResponse);
                Debug.WriteLine(response.StatusCode);
            }
            else
            {
                Debug.WriteLine("Feilkode: {0} og grunnen til dette er: ({1})", (int)response.StatusCode,
                    response.ReasonPhrase);
            }
        }
    
        public static string GetApiInfo()
            {
                var ListOfHackerNewsModels = new List<HackerNewsModel>();

                string apiUrl = "https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty";
                var jsonResult = DoApiCall(apiUrl);
                JsonElement json = JsonSerializer.Deserialize<JsonElement>(jsonResult);
                int antall = Math.Min(json.GetArrayLength(), 20);

                //Getting 20 stories
                for (int i = 0; i < antall; i++){
                    var storyUrl = "https://hacker-news.firebaseio.com/v0/item/" + json[i].GetInt32() + ".json?print=pretty";
                    Console.WriteLine("Fetching this story: " + storyUrl);
                    var jsonStoryResult = DoApiCall(storyUrl);
                    JsonElement jsonStory = JsonSerializer.Deserialize<JsonElement>(jsonStoryResult);
                    Debug.WriteLine("The json story: " + jsonStory);
                    JsonElement title = jsonStory.GetProperty("title");

                    var model = new HackerNewsModel(){
                        by = jsonStory.GetProperty("by").GetString(),
                        descendants = jsonStory.GetProperty("descendants").GetInt32(),
                        score = jsonStory.GetProperty("score").GetInt32(),
                        title = jsonStory.GetProperty("title").GetString(),
                        url = jsonStory.GetProperty("url").GetString()

                    };
                    ListOfHackerNewsModels.Add(model);

                    //Iterate through each story
                    // foreach (JsonElement story in jsonStory.EnumerateArray()){
                    //     var model = new HackerNewsModel()
                    //     {
                    //         by = story.GetProperty("by").GetString(),
                    //         descendants = story.GetProperty("descendants").GetInt32(),
                    //         score = story.GetProperty("score").GetInt32(),
                    //         title = story.GetProperty("title").GetString(),
                    //         url = story.GetProperty("url").GetString()
                    //     };
                    //     ListOfHackerNewsModels.Add(model);
                    // }
                }

                return "";
            }
    }
}
