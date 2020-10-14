using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VKClient
{
    public class Response1
    {
        public List<int> response { get; set; }
    }

    public class Response2
    {
        public List<Account> response { get;set; }
    }

    public class Account
    { 
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            /*Console.WriteLine("Enter the key");
            var key = Console.ReadLine();
            var url = "https://api.vk.com/method/users.get?user_id=445475227&v=5.52&access_token=" + key;
            var str = "";
            using (var client = new HttpClient())
            {
                str = await client.GetStringAsync(url);
            }

            var accounts = JsonConvert.DeserializeObject<Response>(str);
            var user = accounts.response[0];
            Console.WriteLine(user.first_name + " " + user.last_name + " " + user.id);
            */
            //29c36da656a7a76686


            Console.WriteLine("Enter access token from opened page");
            var authString = "https://oauth.vk.com/authorize?client_id=7626950&display=page&scope=friends&response_type=token";
            authString = authString.Replace("&", "^&");
            Process.Start(new ProcessStartInfo(
            "cmd",
            $"/c start {authString}")
            { CreateNoWindow = true });
            var accessToken = Console.ReadLine();
            var url = "https://api.vk.com/method/friends.getOnline?user_id=445475227&v=5.52&access_token=" + accessToken;
            var str = "";
            using (var client = new HttpClient())
            {
                str = await client.GetStringAsync(url);
            }
            var accounts = JsonConvert.DeserializeObject<Response2>(str);

            Console.WriteLine("Enter the key");
            var key = Console.ReadLine();
            string str1;
            foreach (var id in accounts)
            {

            }

            var url1 = $"https://api.vk.com/method/users.get?user_ids=" + str1 + "&v=5.52&access_token=" + key;
            var str1 = "";
            using (var client = new HttpClient())
            {
                str = await client.GetStringAsync(url);
            }

            var accounts1 = JsonConvert.DeserializeObject<Response2>(str);
            var user = accounts.response[0];
            Console.WriteLine(user.first_name + " " + user.last_name + " " + user.id);
        }
    }
}
