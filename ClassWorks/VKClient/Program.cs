using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VKClient
{
    public class Response
    {
        public List<Account> response { get; set; }
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
            Console.WriteLine("Enter the key");
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
        }
    }
}
