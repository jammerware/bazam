using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Bazam.APIs
{
    public class TwitterGitter
    {
        #region Fields
        private string _BearerToken = string.Empty;
        #endregion

        #region Properties
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        #endregion

        public TwitterGitter(string consumerKey, string consumerSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
        }

        /// <summary>
        /// Returns a string of Json containing tweets made by the specified user.
        /// </summary>
        /// <param name="twitterHandle">The Twitter handle of the user whose timeline you want to retrieve (without the leading "@" for best performance).</param>
        /// <param name="includeRetweets">Whether or not you want the result to include retweets by the user. Defaults to false.</param>
        /// <returns></returns>
        public string GetUserTimeline(string twitterHandle, bool includeRetweets = false)
        {
            CheckBearerToken();

            if (twitterHandle.StartsWith("@")) twitterHandle = twitterHandle.Substring(1);
            return ExecuteRequest("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=" + twitterHandle);
        }

        /// <summary>
        /// Returns a string of Json corresponding to the results of your search on Twitter
        /// </summary>
        /// <param name="query">Your Twitter Search API query. Check out https://dev.twitter.com/rest/public/search for details.</param>
        /// <returns></returns>
        public string Search(string query)
        {
            CheckBearerToken();

            return ExecuteRequest("https://api.twitter.com/1.1/search/tweets.json?q=" + Uri.EscapeDataString(query));
        }

        private void CheckBearerToken()
        {
            if (string.IsNullOrEmpty(_BearerToken)) {
                // encode and concatenate the string to submit for a bearer token
                // process detailed here: https://dev.twitter.com/oauth/application-only
                string urlEncodedKey = Uri.EscapeDataString(ConsumerKey);
                string urlEncodedSecret = Uri.EscapeDataString(ConsumerSecret);
                string concatenation = urlEncodedKey + ":" + urlEncodedSecret;

                byte[] bytesToEncode = Encoding.UTF8.GetBytes(concatenation);
                string encoded = Convert.ToBase64String(bytesToEncode);

                WebRequest bearerTokenRequest = WebRequest.Create("https://api.twitter.com/oauth2/token");
                bearerTokenRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                bearerTokenRequest.Headers.Add("Authorization", "Basic " + encoded);
                bearerTokenRequest.Method = "POST";

                byte[] encodedBody = Encoding.UTF8.GetBytes("grant_type=client_credentials");
                bearerTokenRequest.ContentLength = encodedBody.Length;
                using (Stream stream = bearerTokenRequest.GetRequestStream()) {
                    stream.Write(encodedBody, 0, encodedBody.Length);
                }

                string bearerTokenResponseText = string.Empty;
                WebResponse bearerTokenResponse = bearerTokenRequest.GetResponse();
                using (Stream responseStream = bearerTokenResponse.GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(responseStream)) {
                        bearerTokenResponseText = reader.ReadToEnd();
                    }
                }

                JToken json = JValue.Parse(bearerTokenResponseText);
                _BearerToken = json["access_token"].ToString();
            }
        }

        private string ExecuteRequest(string url)
        {
            WebRequest searchRequest = WebRequest.Create(url);
            searchRequest.Headers.Add("Authorization", "Bearer " + _BearerToken);

            string searchResponseText = string.Empty;
            WebResponse searchResponse = searchRequest.GetResponse();
            using (Stream responseStream = searchResponse.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(responseStream)) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}