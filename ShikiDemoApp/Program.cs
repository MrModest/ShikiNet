using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ShikiNet.Core;
using ShikiNet.Entities;
using ShikiNet.Filter.FilterEntity;

namespace ShikiDemoApp
{
    class Program
    {
        private static string filePath = "AuthData.json";

        static void Main(string[] args)
        {
            var result = Start().Result;
            

            Console.ReadKey();
        }

        private static async Task<int> Start()
        {
            var authData = await GetAuthDataAsync();

            PrintJson(authData.OAuth2Token);

            IEnumerable<Anime> animes = await GetSomeAnimesByFilterAsync();
            var animeList = new List<Anime>(animes);

            PrintJson(animes);

            return 0;
        }

        private static async Task<IEnumerable<Anime>> GetSomeAnimesByFilterAsync()
        {
            return await Animes.GetByFilterAsync(f =>
            {
                //f.Page = 1;
                //f.Limit = 10;
                //f.Seasons.Add(new SeasonYear(Season.SPRING, 2018), true); //include
                //f.Seasons.Add(new SeasonYear(2017), false); //exclude
                //f.Seasons.Add(new SeasonYear(1990, 2010), true); //include
                //f.Score = 7;
                //f.Censored = false;
                ////f.GenreIds.Include(12, 24, 56); //include several genres
                //f.GenreIds.Exclude(1, 2, 3); //exclude several genres
                ////f.SearchString = "some anime name";
                //f.Order = AnimeOrder.POPULARITY; //sorting by popularity
                f.Order = AnimeOrder.RANDOM;
            });
        }

        private static async Task<AuthData> GetAuthDataAsync()
        {
            var authData = AuthData.FromJsonPath(filePath);

            if (authData.ClientId == null)
            {
                Console.WriteLine("Enter Client ID: ");
                authData.ClientId = Console.ReadLine();
            }

            if (authData.ClientSecret == null)
            {
                Console.WriteLine("Enter Client Secret: ");
                authData.ClientSecret = Console.ReadLine();
            }

            if (authData.RedirectUrl == null)
            {
                Console.WriteLine("Enter Redirect Url: ");
                authData.RedirectUrl = Console.ReadLine();
            }

            if (authData.OAuth2Token == null || authData.IsTokenExpired)
            {
                Api.ClientId = authData.ClientId;
                Api.ClientSecret = authData.ClientSecret;
                Api.RedirectUrl = authData.RedirectUrl;

                //Api.AppName = "test";
                //Api.DevName = String.Empty;

                var codeRequestUrl = Api.GetAuthorizationCodeRequestUrl();

                Util.OpenUrl(codeRequestUrl);

                Console.Write("Enter authorization code here: ");
                var authCode = Console.ReadLine();

                authData.OAuth2Token = await Api.RequestTokenAsync(authCode);

                //Api.OAuth2Token is null (!) if use 'Api.RequestTokenAsync(authCode)' without 'await' or '.Result'

                authData.SaveToJson(filePath);
            }

            return authData;
        }

        private static void PrintJson(object obj)
        {
            var settings = new JsonSerializerSettings //for (de-)serialization get-autoproperty
            {
                ContractResolver = new PrivateSetterContractResolver()
            };

            //EnumConverter don't work if set 'JsonSerializerSettings.ContractResolver'
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented/*, settings*/));
        }
    }
}
