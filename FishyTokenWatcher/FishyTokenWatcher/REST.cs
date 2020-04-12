using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace FishyTokenWatcher
{
    public class ethdata
    {
        public double balance { get; set; }
    }

    public class tokaninfo
    {
        public string symbol { get; set; }
        public int decimals { get; set; }
        public string address { get; set; }
    }

    public class ethtoken
    {
        public tokaninfo tokeninfo { get; set; }
        public double balance { get; set; }
    }

    public class ethplorer_result
    {
        public ethdata ETH { get; set; }
        public IEnumerable<ethtoken> tokens { get; set; }
    }


    public class REST
    {
        private static REST inst = null;
        public static REST Inst
        {
            get
            {
                if (null == inst) inst = new REST();
                return inst;
            }
        }

        HttpClient httpClient;

        private REST()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(1);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Fishy Token Watcher");
        }

        public Data QUERY()
        {
            try
            {
                string URL = $"http://api.ethplorer.io/getAddressInfo/{Properties.Settings.Default.ethAddress?.ToLower()}?apiKey={Properties.Settings.Default.apiKey}";
                HttpResponseMessage httpResponse = httpClient.GetAsync(URL).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    var jsonResult = httpResponse.Content.ReadAsStringAsync().Result;
                    var ethdata =  new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Deserialize<ethplorer_result>(jsonResult);

                    return EthDataToDATA(ethdata);
                }
            }
            catch (Exception ex)
            {
                string xxx = ex.Message;
            }

            return null;
        }

        private Data EthDataToDATA(ethplorer_result ethdata)
        {
            Data data = new Data();

            data.ethvalue = Convert.ToDecimal(ethdata.ETH.balance.ToString("0.00000000"));

            foreach (var tok in ethdata.tokens)
            {
                string name = tok.tokeninfo.symbol;
                if (string.IsNullOrWhiteSpace(name))
                    name = tok.tokeninfo.address;

                if (string.IsNullOrWhiteSpace(name))
                    continue;

                data.tokens[name] = Convert.ToDecimal((tok.balance / Math.Pow(10, tok.tokeninfo.decimals)).ToString("0.00000000"));
            }

            return data;
        }
    }
}
