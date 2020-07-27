using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Leaf.xNet;
namespace DiscordReactions
{
    class Program
    {
        public static List<string> Tokens = new List<string>();
        public static int JoinCount = 0;

        public static bool Join(string invite, string token)
        {
            CookieContainer cookieContainer = new CookieContainer();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://discord.gg/" + invite);
                request.Headers["authorization"] = token;
                request.CookieContainer = cookieContainer;
                var response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                var request3 = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v6/invites/" + invite);
                request3.Headers["authorization"] = token;
                request3.Method = "POST";
                request3.ContentType = "application/json";
                request3.CookieContainer = cookieContainer;
                request3.ContentLength = 0;

                var r3 = (HttpWebResponse)request3.GetResponse();
                var rs3 = new StreamReader(r3.GetResponseStream()).ReadToEnd();
                Console.WriteLine(rs3);
                return true;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        public static bool React(string CHID, string MSGID, string emoji, string token)
        {
            try
            {
                emoji = Uri.EscapeUriString(emoji);
                var request3 = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v6/channels/" + CHID + "/messages/" + MSGID + "/reactions/" + emoji + "/%40me");

                request3.Headers["authorization"] = token;
                request3.Method = "PUT";
                request3.ContentType = "application/json";
                request3.ContentLength = 0;
                var r3 = (HttpWebResponse)request3.GetResponse();
                var rs3 = new StreamReader(r3.GetResponseStream()).ReadToEnd();
                Console.WriteLine(rs3);
                return true;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        static void Main(string[] args)
        {
            Console.Title = $"Discord Joiner & Reacter";
            Console.WriteLine("Tokens file path(only name.txt if he is in the folder): ");
            var tokensfile = Console.ReadLine();
            foreach (string token in File.ReadLines(tokensfile, Encoding.UTF8)){Tokens.Add(token);}
            Console.Title = $"Discord Joiner & Reacter | Tokens: {Tokens.Count()}";
            /////////////////Lol/////////////////
            Console.WriteLine("Invite: ");
            var invite = Console.ReadLine();
            Console.WriteLine("Channel ID: ");
            var chanid = Console.ReadLine();
            Console.WriteLine("Message ID: ");
            var messageid = Console.ReadLine();
            Console.WriteLine("React: ");
            var Reaction = Console.ReadLine();
            /////////////////////////////////////
            foreach (string token in Tokens)
            {
                new Thread(delegate ()
                {
                    Join(invite, token);
                    Thread.Sleep(120);
                    React(chanid, messageid, Reaction, token);
                }).Start();
                Thread.Sleep(50);
            }
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
