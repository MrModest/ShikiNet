using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShikiNet.Core;

namespace ShikiDemoApp
{
    class Program
    {
        private static string filePath = "AuthData.json";

        static void Main(string[] args)
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

                authData.OAuth2Token = Api.RequestTokenAsync(authCode).Result;
                Api.RefreshTokenAsync(Api.OAuth2Token.RefreshToken);

                authData.SaveToJson(filePath);

                Console.WriteLine(JsonConvert.SerializeObject(authData.OAuth2Token));

                Console.ReadKey();
            }
        }
    }
}
