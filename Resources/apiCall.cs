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

        public static List<CLMatchModel> GetChampionsLeagueInfo(string apiEndpoint)
        {
            var ListOfCLMatchModels = new List<CLMatchModel>();

            // string apiUrl = "https://api.nifs.no/stages/690256/matches/";
            var jsonResult = ApiCall.DoApiCall(apiEndpoint);

            //Kinda insane that this works. Deserializes this straight into my model! Seems legit tho
            CLMatchModel[] clMatchModels = JsonSerializer.Deserialize<CLMatchModel[]>(jsonResult);

            Console.WriteLine("Matches:");
            foreach (CLMatchModel clMatchModel in clMatchModels)
            {
                
                Console.WriteLine("-------------");
                Console.WriteLine($"Away Team Logo: {clMatchModel.AwayTeam.logo}");
                // Add code to access other properties of your model here
            }




            //Old code that still works?


            // JsonElement jsonElementMatches = JsonSerializer.Deserialize<JsonElement>(jsonResult);

            // foreach (JsonElement match in jsonElementMatches.EnumerateArray())
            //     {
            //         Console.WriteLine("-------------");
            //         var matchId = match.GetProperty("id").GetInt32();
            //         Console.WriteLine($"Match ID: {matchId}");

            //         JsonElement homeTeam = match.GetProperty("homeTeam");
            //         var homeTeamName = homeTeam.GetProperty("name").GetString();
            //         Console.WriteLine($"Home Team: {homeTeamName}");

            //         JsonElement awayTeam = match.GetProperty("awayTeam");
            //         var awayTeamName = awayTeam.GetProperty("name").GetString();
            //         Console.WriteLine($"Away Team: {awayTeamName}");

            //         var timestamp = match.GetProperty("timestamp").GetString();
            //         Console.WriteLine($"Match date: {timestamp}");

            //         JsonElement result = match.GetProperty("result");
            //         var resultString = result.GetProperty("homeScore90").GetString() ?? "Not played yet" + " " + result.GetProperty("homeScore90").GetString() ?? "Not played yet";
            //         Console.WriteLine($"Score: {resultString}");

            //         var model = new CLMatchModel(){

            //         };ListOfCLMatchModels.Add(model);
            //     }

            return ListOfCLMatchModels;
        }
        public static List<CLMatchModel> GetApiInfo()
        {
            var ListOfCLMatchModels = new List<CLMatchModel>();

            string apiUrl = "https://api.nifs.no/stages/690256/matches/";
            var jsonResult = ApiCall.DoApiCall(apiUrl);
            var jsonData = JsonSerializer.Deserialize<dynamic>(jsonResult);


            foreach (var item in jsonData)
            {
                var homeTeam = item.homeTeam;
                var awayTeam = item.awayTeam;
                Debug.WriteLine(homeTeam + " " + awayTeam);
            }

            ////Getting 10 stories
            //for (int i = 0; i < antall; i++)
            //{
            //    var storyUrl = "https://hacker-news.firebaseio.com/v0/item/" + json[i].GetInt32() +
            //        ".json?print=pretty";
            //    var jsonStoryResult = ApiCall.DoApiCall(storyUrl);
            //    JsonElement jsonHackerNewsStory = JsonSerializer.Deserialize<JsonElement>(jsonStoryResult);
            //    Debug.WriteLine("The json story we are getting: " + jsonHackerNewsStory);

            //    //Wonky that i have to set variables, then trycatch them THEN put them into the model. But it works
            //    string url, by, title;
            //    int descendants, score, id;
            //    try
            //    {
            //        url = jsonHackerNewsStory.GetProperty("url").GetString();
            //        descendants = jsonHackerNewsStory.GetProperty("descendants").GetInt32();
            //        by = jsonHackerNewsStory.GetProperty("by").GetString();
            //        score = jsonHackerNewsStory.GetProperty("score").GetInt32();
            //        title = jsonHackerNewsStory.GetProperty("title").GetString();
            //        id = jsonHackerNewsStory.GetProperty("id").GetInt32();
            //    }
            //    catch (KeyNotFoundException)
            //    {
            //        url = "N/A"; descendants = 0; by = "N/A"; score = 0; title = "N/A";
            //        id = 0;
            //    }

            //    var model = new CLMatchModel()
            //    {
            //        by = by,
            //        descendants = descendants,
            //        score = score,
            //        title = title,
            //        url = url,
            //        id = id
            //    };
            //    ListOfCLMatchModels.Add(model);
            //}

            return ListOfCLMatchModels;
        }
    }
}
