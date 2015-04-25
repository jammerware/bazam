using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bazam.NoobWebClient
{
    public class NoobWebClient
    {
        public Task<string> GetResponse(string address, params string[] values)
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

            return GetResponse(address, dictValues);
        }

        public async Task<string> GetResponse(string address, Dictionary<string, string> bodyValues = null)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(bodyValues);
            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage response = await client.PostAsync(address, content);
                return await response.Content.ReadAsStringAsync();

            }
        }
    }
}