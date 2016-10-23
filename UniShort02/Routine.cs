using System;
using System.Text.RegularExpressions;
using System.Net;

namespace UniShort02
{
    class Routine
    {
        public string GetJson(string Url)
        {
            const string ShortServAddrs = "http://kisu.me/api/shorten.php?url=";
            string DownStr = null;

            if (!IsUrl(Url)) return null;
            using (WebClient WC = new WebClient())
            {
                DownStr = WC.DownloadString(new Uri(ShortServAddrs + Url));
            }
            return DownStr;

        }
        private bool IsUrl(string SendUrl)
        {
            return Regex.IsMatch(SendUrl, @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$");
        }

        public string ShapeJson(string ResJson)
        {
            string Status = null;
            if(ResJson != null)
            {
                ResJson = ResJson.Substring(0, ResJson.IndexOf(','));
                Status = ResJson.Substring(1, ResJson.IndexOf(':') - 1);

                if (Status.Contains("error")) return null;

                ResJson = ResJson.Remove(0, ResJson.IndexOf(':') + 1);
                ResJson = ResJson.Replace("\"", null);
            }
            return ResJson;

        }
    }
}
