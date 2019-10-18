using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsharpHttpHelper;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Leisucrawler
{
    public class Crawlgoals
    {
        public HttpHelper BotC;
        public string Url = "https://api.leisu.com/api/v2/match/stats?";
        public string resultstr = string.Empty;


        public Crawlgoals()
        {
            this.BotC = new HttpHelper();
        }

        public string SpiderDo(string dataid)
        {

            
            HttpItem BotAItem = new HttpItem()
            {
                URL = Url + "id=" + dataid + "&_=" + Tools.ConvertDateTimeInt(DateTime.Now).ToString(),
                Method = "get",
            };
            HttpResult result = BotC.GetHtml(BotAItem);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                //Tools.log.Info("请求成功状态码：" + result.StatusCode.ToString()+"        "+dataid);
                this.resultstr = result.Html;
                return result.Html;
            }
            else
            {
                Tools.log.Info("请求失败状态码：" + result.StatusCode.ToString());
                Tools.log.Info("请求失败内容：" + result.Html.ToString());
                return "";
            }

        }

        public List<int> getgoals(string jsonstr)
        {
            List<string> ls = new List<string>();
            List<int> ls2 = new List<int>();
            try
            {
                JObject jo = JObject.Parse(this.resultstr);
                JToken data =jo["data"];
                JArray incidents = (JArray)data["incidents"];

                foreach (JObject item in incidents)
                {
                    if (item["type"].ToString() == "1" || item["type"].ToString() == "8")
                    {
                        string tt = item["belong"].ToString() + "_" + item["time"].ToString();
                        ls.Add(tt);
                        ls2.Add(Convert.ToInt32(item["time"].ToString()));
                    }
                }

                if (ls.Count>0)
                {
                    ls2.Sort();
                }

               
            }
            catch (Exception e)
            {

                return ls2;
            }

            return ls2;

        }

        public string getstats(string jsonstr)
        {
            string stats = string.Empty;
            try
            {
                JObject jo = JObject.Parse(this.resultstr);
                JToken data = jo["data"];
                JArray statsarray = (JArray)data["stats"];

                foreach (JObject item in statsarray)
                {
                    if (item["type"].ToString() == "4")//红牌
                    {
                        stats += " 红牌:" + item["home"].ToString() + "-" + item["away"].ToString();
                    }
                    if (item["type"].ToString() == "21")//射正
                    {
                        stats += " 射正:" + item["home"].ToString() + "-" + item["away"].ToString();
                    }
                }


            }
            catch (Exception e)
            {

                return stats;
            }

            return stats;

        }
    }
}
