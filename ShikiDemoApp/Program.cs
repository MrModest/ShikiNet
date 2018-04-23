using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShikiNet.Core;
using ShikiNet.Entity;
using ShikiNet.Filter.FilterEntity;

namespace ShikiDemoApp
{
    class Program
    {
        private static string filePath = "AuthData.json";

        static async void Main(string[] args)
        {
            var authData = AuthData.FromJsonPath(filePath);

            if(authData.ClientId == null)
            {
                Console.WriteLine("Enter Client ID: ");
                authData.ClientId = Console.ReadLine();
            }

            if(authData.ClientSecret == null)
            {
                Console.WriteLine("Enter Client Secret: ");
                authData.ClientSecret = Console.ReadLine();
            }

            if(authData.RedirectUrl == null)
            {
                Console.WriteLine("Enter Redirect Url: ");
                authData.RedirectUrl = Console.ReadLine();
            }

            authData.OAuth2Token = null;

            if (authData.OAuth2Token == null)
            {
                //Console.WriteLine("Go to next url and copy authorization code.");

                Api.ClientId = authData.ClientId;
                Api.ClientSecret = authData.ClientSecret;
                Api.RedirectUrl = authData.RedirectUrl;

                //Api.AppName = "test";
                //Api.DevName = String.Empty;

                var codeRequestUrl = Api.GetAuthorizationCodeRequestUrl();

                Util.OpenUrl(codeRequestUrl);

                Console.Write("Enter authorization code here: ");
                var authCode = Console.ReadLine();

                var result =  Api.RequestTokenAsync(authCode).Result;

                authData.OAuth2Token = Api.OAuth2Token; //it null (!) if not use .Result

                authData.SaveToJson(filePath);

                Console.WriteLine(JsonConvert.SerializeObject(authData.OAuth2Token));

                IEnumerable<Anime> animes = await Animes.GetByFilterAsync(f =>
                {
                    f.Page = 1;
                    f.Limit = 10;
                    f.Seasons.Add(new SeasonYear(Season.SPRING, 2018), true); //include
                    f.Seasons.Add(new SeasonYear(2017), false); //exclude
                    f.Seasons.Add(new SeasonYear(1990, 2010), true); //include
                    f.Score = 7;
                    f.Censored = false;
                    f.GenreIds.Include(12, 24, 56); //include several genres
                    f.GenreIds.Exclude(1, 2, 3); //exclude several genres
                    f.SearchString = "some anime name";
                    f.Order = AnimeOrder.POPULARITY; //sorting by popularity
                });
                var animeList = new List<Anime>(animes);
                Console.ReadKey();
            }
        }
    }
}
