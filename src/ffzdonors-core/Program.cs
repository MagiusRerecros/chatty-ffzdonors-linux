using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ffzdonors_core
{
    public class Program
    {
        static string json = "Loading";
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter your username (used to find your chatty settings directory):");
            string username = Console.ReadLine();
            string chattyFileName = $"/home/{username}/.chatty/addressbookImport.txt";
            string chattyFileContents = "";

            Task.Factory.StartNew(async () => await GetJsonFromUrl("http://api.frankerfacez.com/v1/badge/supporter"));

            while (json == "Loading")
            {
                Console.WriteLine("Loading . . .");
                Thread.Sleep(1000);
            }

            JObject o = JObject.Parse(json);
            JArray a = (JArray)o["users"]["3"];

            List<string> users = a.Select(c => (string)c).ToList();

            foreach (string user in users)
            {
                chattyFileContents += $"add {user} ffzdonor" + Environment.NewLine;
            }

            File.WriteAllText(chattyFileName, chattyFileContents);

            Console.WriteLine($"{chattyFileName} created.");
            Console.WriteLine("Run the command \"/abImport\" from within Chatty.");
            Console.WriteLine("Additionally, make sure the FFZ Donor badge is set to the restriction \"$cat:ffzdonor\" in Chatty's Usericons settings.");
        }

        private static async Task GetJsonFromUrl(string url)
        {
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                json = await client.GetStringAsync(url);
            }
        }
    }
}
