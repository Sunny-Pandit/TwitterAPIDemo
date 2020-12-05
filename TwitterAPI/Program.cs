using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.IO;



namespace TwitterAPI
{
    class Program
    {

        static async Task Main(string[] args)
        {

            var _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (Tweets.tCollection.Count > 0)
                        UpdateConsole();

                    await Task.Delay(Config.TimeInterval);
                }
            });

            await CreateTweetCollection();

        }


        private static async Task CreateTweetCollection()
        {
            string url = Config.StreamURL;
            string bearer_token = Config.BearerToken;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearer_token);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var retcode = response.StatusCode;

            if (retcode == HttpStatusCode.OK)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var reader = new StreamReader(responseStream);

                while (!reader.EndOfStream)
                {
                    bool b = await Tweets.ProcessTweetStream(reader.ReadLine());


                }
            }


        }

        private static void UpdateConsole()
        {
            List<tdatastream> t = Tweets.tCollection;
            //total tweets
            int TotalTweet = t.Count;
            Console.WriteLine($"Total Tweets: {TotalTweet}");

            //Average Tweets            

            //Per Second
            //DateTime.Now.ToString("G") returns date with seconds
            var Counts = Tweets.tCollection.GroupBy(t =>
            {
                return t.tDate.ToString("G");
            }
                )
                .Select(g => new { TimeStamp = g.Key, Count = g.Count() })
                .ToList();
            
            var average = Counts.Average(c => c.Count);
            Console.WriteLine($"Average tweets per second: {average}");

            //Per Minute
            //DateTime.Now.ToString("g") returns date without seconds
            Counts = Tweets.tCollection.GroupBy(t =>
            {
                return t.tDate.ToString("g");
            }
                )
                .Select(g => new { TimeStamp = g.Key, Count = g.Count() })
                .ToList();

            average = Counts.Average(c => c.Count);
            Console.WriteLine($"Average tweets per minute: {average}");

            //Per Hour
            //date.ToString("dd/MM/yyyy HH:mm:ss") datetime in 24 hr format (hh is 12 hr format.)
            Counts = Tweets.tCollection.GroupBy(t =>
            {
                return t.tDate.ToString("dd/MM/yyyy HH");
            }
                )
                .Select(g => new { TimeStamp = g.Key, Count = g.Count() })
                .ToList();

            average = Counts.Average(c => c.Count);
            Console.WriteLine($"Average tweets per hour: {average}");


            //Tweets with emoji
            int e = Tweets.tCollection.Count(t=>t.hasEmoji == true);
            Console.WriteLine($"Total tweets with emoji: {e}");

            string topEmoji = Tweets.EmojiCounts.FirstOrDefault(t => t.Value == Tweets.EmojiCounts.Values.Max()).Key;
            Console.WriteLine($"Top emoji in tweets: {topEmoji}");

            double EmojiPercent = TotalTweet > 0 ? (e *100 / TotalTweet)  : 0;
            Console.WriteLine($"Percent of tweets with Emoji: {EmojiPercent}%");

            //tweets with hashtag
            int h = Tweets.tCollection.Count(t => t.hasHashTag == true);
            Console.WriteLine($"Total tweets with hashtag: {h}");

            string topHashTag = Tweets.HashTagCounts.FirstOrDefault(t => t.Value == Tweets.HashTagCounts.Values.Max()).Key;
            Console.WriteLine($"Top hashtag in tweets: {topHashTag}");


            //tweets with URL
            int u = Tweets.tCollection.Count(t => t.hasURL == true);
            Console.WriteLine($"Total tweets with URL: {u}");

            double URLPercent = TotalTweet > 0 ? (u * 100 / TotalTweet) : 0;
            Console.WriteLine($"Percent of tweets with url: {URLPercent}%");

            //tweets with pictures
            int p = Tweets.tCollection.Count(t => t.hasPhotoURL == true);
            Console.WriteLine($"Total tweets with pictures: {p}");

            double PhotoURLPercent = TotalTweet > 0 ? (p * 100 / TotalTweet) : 0;
            Console.WriteLine($"Percent of tweets with photo url: {PhotoURLPercent}%");

            //top domain
            string topDomain = Tweets.DomainCounts.FirstOrDefault(t => t.Value == Tweets.DomainCounts.Values.Max()).Key;
            Console.WriteLine($"Top hashtag in tweets: {topDomain}");


            Console.WriteLine("\n\n");
        }

        



    }
}
