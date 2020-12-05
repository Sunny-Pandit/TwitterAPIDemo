
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;




namespace TwitterAPI
{
    public static class Tweets
    {
        public static List<tdatastream> tCollection = new List<tdatastream>();

        public static Dictionary<string, int> EmojiCounts = new Dictionary<string, int>();
        public static Dictionary<string, int> HashTagCounts = new Dictionary<string, int>();
        public static Dictionary<string, int> DomainCounts = new Dictionary<string, int>();


        public static async Task<bool> ProcessTweetStream(string TweetStream)
        {
            
            await Task.Run(() =>
            {
                if (TweetStream.Length > 0)
                {
                    tdatastream t = JsonConvert.DeserializeObject<tdatastream>(TweetStream);
                    t.tDate = DateTime.Now;
                    t.hasEmoji = eMojiStat(t.data.text);
                    t.hasHashTag = HashTagStat(t.data.text);
                    t.hasURL = URLStat(t.data.text);
                    t.hasPhotoURL = PICStat(t.data.text);

                    tCollection.Add(t);
                }
            });
            return true;
        }




        public static bool eMojiStat(string tText)
        {
            string EmojiPattern = Config.EmojiPattern;

            bool hasEmoji = false;
            Regex r = new Regex(EmojiPattern);

            MatchCollection matches = r.Matches(tText);

            if (matches.Count > 0)
                hasEmoji = true;

            foreach (Match match in matches)
            {

                GroupCollection groups = match.Groups;
                for (int i = 0; i < groups.Count; i++)
                {
                    string emoji = groups[i].Value;
                    if (EmojiCounts.ContainsKey(emoji))
                        EmojiCounts[emoji]++;
                    else
                        EmojiCounts.Add(emoji, 1);
                }
            }

            return hasEmoji;
        }

        public static bool HashTagStat(string tText)
        {
            string HashTagPattern = Config.HashTagPattern;

            bool hasHashTag = false;
            Regex r = new Regex(HashTagPattern);
            
            MatchCollection matches = r.Matches(tText);
            
            if (matches.Count>0)
                hasHashTag = true;

            foreach (Match match in matches)
            {
                
                GroupCollection groups = match.Groups;
                for (int i = 0; i < groups.Count; i++)
                {
                    string hashtag = groups[i].Value;
                    if (HashTagCounts.ContainsKey(hashtag))
                        HashTagCounts[hashtag]++;
                    else
                        HashTagCounts.Add(hashtag, 1);
                }
            }

            return hasHashTag;
        }

        public static bool URLStat(string tText)
        {

            string URLPattern = Config.URLPattern;



            bool hasURL = false;
            Regex r = new Regex(URLPattern);


            MatchCollection matches = r.Matches(tText);

            if (matches.Count > 0)
                hasURL = true;

            foreach (Match match in matches)
            {

                GroupCollection groups = match.Groups;
                for (int i = 0; i < groups.Count; i++)
                {
                    string domain = getDomainName(groups[i].Value);
                    if (domain.Length > 0)
                    {
                        if (DomainCounts.ContainsKey(domain))
                            DomainCounts[domain]++;
                        else
                            DomainCounts.Add(domain, 1);
                    }

                }
            }



            return hasURL;

        }

        public static String getDomainName(String url)
        {
            try
            {
                Uri uri = new Uri(url);
                String domain = uri.Host;
                return domain.StartsWith("www.") ? domain.Substring(4) : domain;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public static bool PICStat(string tText)
        {
            bool hasPhotoURL = false;

            
                if (tText.Contains("pic.twitter.com") || tText.Contains("Instagram.com"))
                {
                    hasPhotoURL = true;
                }
            
            return hasPhotoURL;
        }




     



    }
}
