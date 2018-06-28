using OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShoploAuth
{
    class Program
    {
        private static string ConsumerKey = string.Empty;
        private static string ConsumerSecret = string.Empty;

        private static string url = "http://api.shoplo.com/services/oauth/request_token";

        static void Main(string[] args)
        {
            while (string.IsNullOrEmpty(ConsumerKey))
            {
                Console.WriteLine("Please provide the Consumer Key:");
                ConsumerKey = Console.ReadLine();
            }

            while (string.IsNullOrEmpty(ConsumerSecret))
            {
                Console.WriteLine("Please provide the Consumer Secret:");
                ConsumerSecret = Console.ReadLine();
            }

            CallService();

            Console.ReadLine();
            
        }

        private static async void CallService()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync(GetRequestToken(url), new FormUrlEncodedContent(new Dictionary<string, string>())).GetAwaiter().GetResult();
                string content = await response.Content.ReadAsStringAsync();

                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static string GetRequestToken(string url)
        {
            OAuthRequest client = OAuthRequest.ForRequestToken(ConsumerKey, ConsumerSecret);
            client.RequestUrl = url;
            client.Method = "POST";
            client.SignatureMethod = OAuthSignatureMethod.HmacSha1;
            string urlRet = client.GetAuthorizationQuery();
            return url + '?' + urlRet;
        }
    }
}
