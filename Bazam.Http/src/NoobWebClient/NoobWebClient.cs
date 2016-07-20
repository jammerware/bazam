using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bazam.Http
{
    public class NoobWebClient
    {
        public async Task DownloadFile(string url, string fileName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMsg = await client.GetAsync(url);

            if (responseMsg.IsSuccessStatusCode) {
                await responseMsg.Content.ReadAsFileAsync(fileName);
            }
            else {
                throw new Exception("Something went wrong during an attempted file download: " + responseMsg.ReasonPhrase);
            }
        }

        public async Task<string> DownloadString(string url)
        {
            return await DownloadString(url, RequestMethod.Get);
        }

        public async Task<string> DownloadString(string address, RequestMethod requestType, params string[] bodyValues)
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            if (bodyValues != null && bodyValues.Length > 1) {
                string key = string.Empty;
                for (int i = 0; i < bodyValues.Length; i++) {
                    if (i % 2 == 0) {
                        key = bodyValues[i];
                    }
                    else {
                        dictValues.Add(key, bodyValues[i]);
                    }
                }
            }

            return await DownloadString(address, requestType, dictValues);
        }

        public async Task<string> DownloadString(string address, RequestMethod requestType, Dictionary<string, string> bodyValues = null)
        {
            FormUrlEncodedContent content = null;
            if (bodyValues != null) {
                content = new FormUrlEncodedContent(bodyValues);
            }

            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage response = null;
                if (requestType == RequestMethod.Post) {
                    response = await client.PostAsync(address, content);
                }
                else {
                    response = await client.GetAsync(address);
                }

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}