using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Newtonsoft.Json.Linq;

namespace Bazam.Twitter
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
        public async Task<string> GetUserTimeline(string twitterHandle, bool includeRetweets = false)
        {
            await CheckBearerToken();

            if (twitterHandle.StartsWith("@")) twitterHandle = twitterHandle.Substring(1);
            return await ExecuteRequest("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=" + twitterHandle);
        }

        /// <summary>
        /// Returns a string of Json corresponding to the results of your search on Twitter.
        /// </summary>
        /// <param name="query">Your Twitter Search API query. Check out https://dev.twitter.com/rest/public/search for details.</param>
        /// <returns></returns>
        public async Task<string> Search(string query)
        {
            await CheckBearerToken();

            return await ExecuteRequest("https://api.twitter.com/1.1/search/tweets.json?q=" + Uri.EscapeDataString(query));
        }

        private async Task CheckBearerToken()
        {
            if (string.IsNullOrEmpty(_BearerToken)) {
                // encode and concatenate the string to submit for a bearer token
                // process detailed here: https://dev.twitter.com/oauth/application-only
                string urlEncodedKey = Uri.EscapeDataString(ConsumerKey);
                string urlEncodedSecret = Uri.EscapeDataString(ConsumerSecret);
                string concatenation = urlEncodedKey + ":" + urlEncodedSecret;

                byte[] bytesToEncode = Encoding.UTF8.GetBytes(concatenation);
                string encoded = Convert.ToBase64String(bytesToEncode);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://api.twitter.com/oauth2/token");
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("charset=UTF-8"));

                // compose body data
                Dictionary<string, string> bodyData = new Dictionary<string, string>();
                bodyData.Add("grant_type", "client_credentials");
                HttpContent bodyContent = new FormUrlEncodedContent(bodyData);

                // post
                HttpResponseMessage responseMessage = await client.PostAsync("https://api.twitter.com/oauth2/token", bodyContent);
                
                if(responseMessage.IsSuccessStatusCode) {
                    string response = await responseMessage.Content.ReadAsStringAsync();

                    JToken json = JValue.Parse(response);
                    _BearerToken = json["access_token"].ToString();
                }

                throw new HttpRequestException("Something went wrong when attempting to obtain a Twitter bearer token: " + responseMessage.ReasonPhrase);
            }
        }

        private async Task<string> ExecuteRequest(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _BearerToken);
            HttpResponseMessage responseMsg = await client.GetAsync(url);

            if(responseMsg.IsSuccessStatusCode) {
                return await responseMsg.Content.ReadAsStringAsync();
            }

            throw new HttpRequestException("Couldn't reach the url " + url);
        }
    }
}