using System;
using System.Collections.Generic;
using System.Linq;

namespace FishyTokenWatcher
{
    public class Data
    {
        public Data()
        {
            Init();
        }

        public decimal ethvalue = 0;
        public SortedDictionary<string, decimal> tokens = new SortedDictionary<string, decimal>();

        private void Init()
        {
            string alldata = Properties.Settings.Default.data;

            if (string.IsNullOrEmpty(alldata))
                return;

            var parts1 = alldata.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var p1 in parts1)
            {
                var p2 = p1.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (p2.Count() != 2)
                    throw new Exception("Bad split: " + p2);

                if (p2[0].Trim().ToUpper() == "ETH")
                    ethvalue = Convert.ToDecimal(p2[1]);
                else
                {
                    tokens[p2[0].Trim().ToUpper()] = Convert.ToDecimal(p2[1]);
                }
            }
        }

        public string CheckAndChange(Data other)
        {
            string change = "";

            if (this.ethvalue != other.ethvalue)
            {
                change += $"ETH:{(other.ethvalue > this.ethvalue ? "+" : "")}{(other.ethvalue - this.ethvalue).ToString("0.00000000")};";
            }

            foreach (var kvp in other.tokens.Where(t => !this.tokens.Any(tt => tt.Key == t.Key)))
            {
                change += $"{kvp.Key}:+{kvp.Value.ToString("0.00000000")};";
            }

            foreach (var kvp in this.tokens.Where(t => !other.tokens.Any(tt => tt.Key == t.Key)))
            {
                change += $"{kvp.Key}:-{kvp.Value.ToString("0.00000000")};";
            }

            foreach (var kvp in other.tokens.Where(t => this.tokens.Any(tt => tt.Key == t.Key)))
            {
                if (other.tokens[kvp.Key] == this.tokens[kvp.Key])
                    continue;
                change += $"{kvp.Key}:{(other.tokens[kvp.Key] > this.tokens[kvp.Key] ? "+" : "")}{(other.tokens[kvp.Key] - this.tokens[kvp.Key]).ToString("0.00000000")};";
            }

            if (!string.IsNullOrEmpty(change))
            {
                this.ethvalue = other.ethvalue;
                this.tokens = other.tokens;
                Save();
            }

            return change;
        }

        private void Save()
        {
            string newData = $"ETH:{ethvalue.ToString("0.00000000")};";
            foreach (var kvp in tokens)
            {
                newData += $"{kvp.Key}:{kvp.Value.ToString("0.00000000")};";
            }

            Properties.Settings.Default.data = newData;
            Properties.Settings.Default.Save();
        }

    }
}
