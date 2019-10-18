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
    public class Crawlodds
    {
        public HttpHelper BotC;
        public string Url = "https://api.leisu.com/api/v2/match/odds_detail?";
        //http://api.leisu.com/api/odds/detail?



        public Crawlodds()
        {
            this.BotC = new HttpHelper();
        }


        public string[] SpiderDo(string dataid)
        {

            string[] odds = { "0.0", "0.0" };
            HttpItem BotAItem = new HttpItem()
            {
                URL = Url + "sid="+dataid+"&_="+Tools.ConvertDateTimeInt(DateTime.Now).ToString(),
                Method = "get",
            };
            HttpResult result = BotC.GetHtml(BotAItem);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                //Tools.log.Info("请求成功状态码：" + result.StatusCode.ToString()+"        "+dataid);
                return getodds(result.Html);
                //return result.Html;
            }
            else
            {
                Tools.log.Info("请求失败状态码：" + result.StatusCode.ToString());
                Tools.log.Info("请求失败内容：" + result.Html.ToString());
                return odds;
            }

        }

        public string[] getodds(string jsonstr)
        {
            string[] odds = { "0.0", "0.0", "0.0", "0.0" };//滚球欧赔，滚球小球赔率，滚球大小球数，初盘大小球数
            try
            {
                JObject jo = JObject.Parse(jsonstr);
                JArray gunqiu = (JArray)jo.Value<JArray>("data")[2];//第三行 滚球数据
                JArray gunqiu_oupei = (JArray)gunqiu[1][0];//第三行 第2个数组  欧赔数据
                JArray gunqiu_bigsmall = (JArray)gunqiu[2][0];//第三行 第3个数组  大小球数据
                if (!string.IsNullOrEmpty(gunqiu_oupei[1].ToString()))
                {
                    odds[0] = gunqiu_oupei[1].ToString().Trim();
                }
                if (!string.IsNullOrEmpty(gunqiu_bigsmall[2].ToString()))
                {
                    odds[1] = gunqiu_bigsmall[2].ToString().Trim();
                }
                if (!string.IsNullOrEmpty(gunqiu_bigsmall[1].ToString()))
                {
                    odds[2] = gunqiu_bigsmall[1].ToString().Trim();
                }

                JArray chupan = (JArray)jo.Value<JArray>("data")[0];//第一行 初盘数据
                JArray chupan_bigsmall = (JArray)chupan[2][0];//第一行 第3个数组  大小球数据
                if (!string.IsNullOrEmpty(chupan_bigsmall[1].ToString()))
                {
                    odds[3] = chupan_bigsmall[1].ToString().Trim();
                }
            }
            catch (Exception e)
            {

                return odds;
            }
            
            return odds;

        }
    }
}
