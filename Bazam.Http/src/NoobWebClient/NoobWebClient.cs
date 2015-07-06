using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bazam.Http
{
    public class NoobWebClient
    {
        public Task DownloadFile(string url, string fileName)
        {
            HttpClient client = new HttpClient();
            client.GetAsync(url).ContinueWith(
                async (requestTask) => {
                    HttpResponseMessage response = requestTask.Result;
                    response.EnsureSuccessStatusCode();
                    await response.Content.ReadAsFileAsync(fileName);
                }
            );

            return null;
        }

        public Task<string> DownloadString(string url)
        {
            return DownloadString(url, RequestMethod.Get);
        }

        public Task<string> DownloadString(string address, RequestMethod requestType, params string[] values)
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            if (values != null && values.Length > 1) {
                string key = string.Empty;
                for (int i = 0; i < values.Length; i++) {
                    if (i % 2 == 0) {
                        key = values[i];
                    }
                    else {
                        dictValues.Add(key, values[i]);
                    }
                }
            }

            return DownloadString(address, requestType, dictValues);
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