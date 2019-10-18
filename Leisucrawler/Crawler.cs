using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Net.Mail;
using System.Net;

namespace Leisucrawler
{
    public class Crawler
    {
        public string inifilepath;
        public Crawlodds myodds;
        public Crawlgoals mygoals;
        public Hashtable mailsubhs;//邮件主题hash condition:string value
        public Hashtable mailconhs;//命中发邮件的条件hash dataid:string value
        public Hashtable gunbigsmallhs;//特定时间滚球大小球hash dataid:double value
        public Hashtable smalloddshs;//前一分钟小球赔率hash  dataid:double value
        public Hashtable gunbigsmallprehs;//前一分钟滚球大小球hash   dataid:double vlaue







        public Crawler()
        {

            this.inifilepath = AppDomain.CurrentDomain.BaseDirectory + "AppConfig.ini";
            this.myodds = new Crawlodds();
            this.mygoals = new Crawlgoals();
            this.mailsubhs = new Hashtable();
            this.mailconhs = new Hashtable();
            this.gunbigsmallhs = new Hashtable();
            this.smalloddshs = new Hashtable();
            this.gunbigsmallprehs = new Hashtable();



            mailsubhs.Add("c4_1","334");
            mailsubhs.Add("c4_2", "757");
            mailsubhs.Add("c5_1", "331");
            mailsubhs.Add("c6_1", "下半场有球");
        }


        public string Crawl(string html)
        {
            string input = html.Replace(Convert.ToChar(10).ToString(), "").Replace(Convert.ToChar(13).ToString(), ""); ; ;
            //return input;
            int crawlcount = 0;
            string result = string.Empty;
            string test = string.Empty;
            List<Match> ls = new List<Match>();
            try
            {
                ls.Clear();
                string pattern = "正在进行中的比赛</td></tr>\\s*?(.*)\\s*<tr class=\"dd-item title dd-notStart-title\"";
                Regex rgx = new Regex(pattern);
                MatchCollection matches = rgx.Matches(input);

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                if (matches.Count>0)
                {
                    
                    List<int> timels=new List<int>();

                    int c1_1_1 = -1;
                    double c1_1_2=-1.0;
                    if (Tools.ReadIniData("application", "c1_1_1", "null", inifilepath).ToString() != "null")
                    {
                        c1_1_1 = Convert.ToInt32(Tools.ReadIniData("application", "c1_1_1", "70", inifilepath));
                        c1_1_2 = Convert.ToDouble(Tools.ReadIniData("application", "c1_1_2", "2", inifilepath));
                    }

                    int c1_2_1 = -1;
                    double c1_2_2 = -1.0;
                    if (Tools.ReadIniData("application", "c1_2_1", "null", inifilepath).ToString() != "null")
                    {
                        c1_2_1 = Convert.ToInt32(Tools.ReadIniData("application", "c1_2_1", "74", inifilepath));
                        c1_2_2 = Convert.ToDouble(Tools.ReadIniData("application", "c1_2_2", "1.72", inifilepath));
                    }

                    int c1_3_1 = -1;
                    double c1_3_2 = -1.0;
                    if (Tools.ReadIniData("application", "c1_3_1", "null", inifilepath).ToString() != "null")
                    {
                        c1_3_1 = Convert.ToInt32(Tools.ReadIniData("application", "c1_3_1", "80", inifilepath));
                        c1_3_2 = Convert.ToDouble(Tools.ReadIniData("application", "c1_3_2", "1.53", inifilepath));
                    }

                    int c1_4_1 = -1;
                    double c1_4_2 = -1.0;
                    if (Tools.ReadIniData("application", "c1_4_1", "null", inifilepath).ToString() != "null")
                    {
                        c1_4_1 = Convert.ToInt32(Tools.ReadIniData("application", "c1_4_1", "80", inifilepath));
                        c1_4_2 = Convert.ToDouble(Tools.ReadIniData("application", "c1_4_2", "1.53", inifilepath));
                    }
                    ///////////
                    int c2_1_1 = -1;
                    double c2_1_2 = -1.0;
                    if (Tools.ReadIniData("application", "c2_1_1", "null", inifilepath).ToString() != "null")
                    {
                        c2_1_1 = Convert.ToInt32(Tools.ReadIniData("application", "c2_1_1", "70", inifilepath));
                        c2_1_2 = Convert.ToDouble(Tools.ReadIniData("application", "c2_1_2", "2", inifilepath));
                    }

                    int c2_2_1 = -1;
                    double c2_2_2 = -1.0;
                    if (Tools.ReadIniData("application", "c2_2_1", "null", inifilepath).ToString() != "null")
                    {
                        c2_2_1 = Convert.ToInt32(Tools.ReadIniData("application", "c2_2_1", "74", inifilepath));
                        c2_2_2 = Convert.ToDouble(Tools.ReadIniData("application", "c2_2_2", "1.72", inifilepath));
                    }

                    int c2_3_1 = -1;
                    double c2_3_2 = -1.0;
                    if (Tools.ReadIniData("application", "c2_3_1", "null", inifilepath).ToString() != "null")
                    {
                        c2_3_1 = Convert.ToInt32(Tools.ReadIniData("application", "c2_3_1", "80", inifilepath));
                        c2_3_2 = Convert.ToDouble(Tools.ReadIniData("application", "c2_3_2", "1.53", inifilepath));
                    }

                    int c2_4_1 = -1;
                    double c2_4_2 = -1.0;
                    if (Tools.ReadIniData("application", "c2_4_1", "null", inifilepath).ToString() != "null")
                    {
                        c2_4_1 = Convert.ToInt32(Tools.ReadIniData("application", "c2_4_1", "80", inifilepath));
                        c2_4_2 = Convert.ToDouble(Tools.ReadIniData("application", "c2_4_2", "1.53", inifilepath));
                    }
                    /////
                    int c3_1_1 = -1;
                    int c3_1_2 = -1;
                    double c3_1_3 = -1.0;
                    if (Tools.ReadIniData("application", "c3_1_1", "null", inifilepath).ToString() != "null")
                    {
                        c3_1_1 = Convert.ToInt32(Tools.ReadIniData("application", "c3_1_1", "85", inifilepath));
                        c3_1_2 = Convert.ToInt32(Tools.ReadIniData("application", "c3_1_2", "1", inifilepath));
                        c3_1_3 = Convert.ToDouble(Tools.ReadIniData("application", "c3_1_3", "0.5", inifilepath));
                    }
                    /////
                    int c4_1_1 = -1;
                    int c4_1_2 = -1;
                    if (Tools.ReadIniData("application", "c4_1_1", "null", inifilepath).ToString() != "null")
                    {
                        c4_1_1 = Convert.ToInt32(Tools.ReadIniData("application", "c4_1_1", "31", inifilepath));
                        c4_1_2 = Convert.ToInt32(Tools.ReadIniData("application", "c4_1_2", "3", inifilepath));
                    }

                    int c4_2_1 = -1;
                    int c4_2_2 = -1;
                    if (Tools.ReadIniData("application", "c4_2_1", "null", inifilepath).ToString() != "null")
                    {
                        c4_2_1 = Convert.ToInt32(Tools.ReadIniData("application", "c4_2_1", "31", inifilepath));
                        c4_2_2 = Convert.ToInt32(Tools.ReadIniData("application", "c4_2_2", "3", inifilepath));
                    }

                    int c4_3_1 = -1;
                    int c4_3_2 = -1;
                    if (Tools.ReadIniData("application", "c4_3_1", "null", inifilepath).ToString() != "null")
                    {
                        c4_3_1 = Convert.ToInt32(Tools.ReadIniData("application", "c4_3_1", "31", inifilepath));
                        c4_3_2 = Convert.ToInt32(Tools.ReadIniData("application", "c4_3_2", "3", inifilepath));
                    }
                    ////
                    int c5_1_1 = -1;
                    int c5_1_2 = -1;
                    int c5_1_3 = -1;
                    int c5_1_4 = -1;
                    if (Tools.ReadIniData("application", "c5_1_1", "null", inifilepath).ToString() != "null")
                    {
                        c5_1_1 = Convert.ToInt32(Tools.ReadIniData("application", "c5_1_1", "55", inifilepath));
                        c5_1_2 = Convert.ToInt32(Tools.ReadIniData("application", "c5_1_2", "3", inifilepath));
                        c5_1_3 = Convert.ToInt32(Tools.ReadIniData("application", "c5_1_3", "3", inifilepath));
                        c5_1_4 = Convert.ToInt32(Tools.ReadIniData("application", "c5_1_4", "7", inifilepath));
                    }
                    ////
                    double c6_1_1 = -1.0;
                    double c6_1_2 = -1.0;
                    if (Tools.ReadIniData("application", "c6_1_1", "null", inifilepath).ToString() != "null")
                    {
                        c6_1_1 = Convert.ToDouble(Tools.ReadIniData("application", "c6_1_1", "0.75", inifilepath));
                        c6_1_2 = Convert.ToDouble(Tools.ReadIniData("application", "c6_1_2", "0.25", inifilepath));
                    }



                    doc.LoadHtml(matches[0].Groups[1].Value.ToString().Trim().Replace("&nbsp;", "").Replace("<br>", ""));
                    HtmlNodeCollection trs = doc.DocumentNode.SelectNodes("/tr");
                    crawlcount = trs.Count;
                    foreach (HtmlNode item in trs)
                    {
                        try
                        {
                            string matchinfo = string.Empty;//比赛
                            string matchscore = string.Empty;//比分
                            string teaminfo = string.Empty;//主客队
                            string matchtime = string.Empty;//比赛时间
                            string matchodds = string.Empty;//滚球欧赔
                            string smallodds = string.Empty;//小球赔率
                            string dataid = string.Empty;//数据ID
                            string scoretotal = string.Empty;//总比分
                            string scorehalf = string.Empty;//半场分数
                            string ballcount = string.Empty;//大小球数
                            string chupanballcount = string.Empty;//初盘大小球
                            string goalslist = string.Empty;//进球时间点
                            string stats = string.Empty;//射正 红牌数据


                            dataid = item.GetAttributeValue("data-id", "123456");
                            //HtmlNode a = item.SelectSingleNode(".//a[@class='event-name lang ']");
                            HtmlNode a = item.SelectSingleNode(".//a[starts-with(@class,'event-name lang ')]");
                            matchinfo = a.InnerHtml.Trim();
                            HtmlNode b = item.SelectSingleNode(".//td[starts-with(@class,'lab-status')]/span/i").NextSibling;

                            if (Regex.IsMatch(b.InnerHtml.Trim(), @"^[+-]?\d*$"))
                            {
                                matchtime = b.InnerHtml.Trim();
                            }
                            else
                            {
                                matchtime = "-90";
                            }


                            //HtmlNode c = item.SelectSingleNode(".//td[@class='lab-score color-red']/div/span/b");
                            HtmlNode c = item.SelectSingleNode(".//td[starts-with(@class,'lab-score color-red')]/div/span/b");
                            matchscore = c.InnerHtml.Trim();//当前比分
                            //teaminfo = item.SelectSingleNode(".//td[@class='lab-team-home bd-left']/a").InnerHtml.Trim() + "-" + item.SelectSingleNode(".//td[@class='lab-team-away']/a").InnerHtml.Trim();//主客场队伍
                            teaminfo = item.SelectSingleNode(".//td[starts-with(@class,'lab-team-home bd-left')]/a").InnerHtml.Trim() + "-" + item.SelectSingleNode(".//td[starts-with(@class,'lab-team-away')]/a").InnerHtml.Trim();//主客场队伍
                            int timenow = Convert.ToInt32(matchtime);//比赛时间
                            string[] odds = myodds.SpiderDo(dataid);
                            matchodds = odds[0];//滚球欧赔
                            smallodds = odds[1];//滚球小球赔率
                            ballcount = odds[2];//滚球大小球数
                            chupanballcount = odds[3];//初盘大小球数


                            scoretotal = (Convert.ToInt32(matchscore.Split('-')[0]) + Convert.ToInt32(matchscore.Split('-')[1])).ToString();
                            if (timenow>45)
                            {
                                //HtmlNode d = item.SelectSingleNode(".//td[@class='lab-half']");
                                HtmlNode d = item.SelectSingleNode(".//td[starts-with(@class,'lab-half')]");
                                string shalf = d.InnerHtml.Trim();
                                if (shalf!="-")
                                {
                                    scorehalf=(Convert.ToInt32(shalf.Split('-')[0]) + Convert.ToInt32(shalf.Split('-')[1])).ToString();
                                }
                                else
                                {
                                    scorehalf = "-1";
                                }
                            }
                            else
                            {
                                scorehalf = scoretotal;
                            }

                            //HtmlNode e = item.SelectSingleNode(".//td[@class='lab-sb bd-left']/div/table/tbody/tr[@class='color-666 size']/td[@class='lab-bet-odds p-b-5']/span/span");
                            //HtmlNode e = item.SelectSingleNode(".//td[starts-with(@class,'lab-sb bd-left')]/div/table/tbody/tr[starts-with(@class,'color-666 size')]/td[starts-with(@class,'lab-bet-odds p-b-5')]/span/span");
                            //if (e!=null)
                            //{
                                //ballcount = e.InnerHtml.Trim();
                            //}
                            string jstr=mygoals.SpiderDo(dataid);
                            List<int> ls2 = mygoals.getgoals(jstr);
                            if (ls2.Count>0)
                            {
                                foreach (int goaltime in ls2)
                                {
                                    goalslist += goaltime.ToString() + ",";
                                }
                                goalslist=goalslist.Remove(goalslist.Length - 1, 1);
                            }


                            stats = mygoals.getstats(jstr);


                            if (((timenow == 46) || (timenow == 47) || (timenow == 48)) && (!gunbigsmallhs.ContainsKey(dataid)))
                            {
                                gunbigsmallhs.Add(dataid,ballcount);
                            }


                            


                            


                            //test += "\n matchinfo:" + matchinfo + "|matchscore:" + matchscore + "|teaminfo:" + teaminfo + "|matchtime:" + matchtime + "|matchodds:" + matchodds + "|smallodds:" + smallodds + "|dataid:" + dataid + "|scoretotal:" + scoretotal + "|scorehalf:" + scorehalf + "|ballcount:" + ballcount+"|chupanballcount:"+chupanballcount+"|goalslist:"+goalslist+"|stats:"+stats+"\n";

                            
                            //
                            Match mm = new Match();
                            mm.Matchinfo = matchinfo;
                            mm.Matchodds = matchodds;
                            mm.Matchscore = matchscore;
                            mm.Matchtime = matchtime;
                            mm.Teaminfo = teaminfo;
                            mm.Smallodds = smallodds;
                            mm.Dataid = dataid;
                            mm.Scoretotal = scoretotal;
                            mm.Scorehalf = scorehalf;
                            mm.Ballcount = ballcount;
                            mm.Chupanballcount = chupanballcount;
                            mm.Goalslist = goalslist;
                            mm.Stats = stats;
                            //


                            //条件对比逻辑开始

                            if (Math.Abs(Convert.ToInt32(matchscore.Split('-')[0]) - Convert.ToInt32(matchscore.Split('-')[1])) == 0)//C1
                            {
                                if ((timenow == c1_1_1) && (Convert.ToDouble(matchodds) < c1_1_2) && (Convert.ToDouble(matchodds) > 0))
                                {

                                    
                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString()!="c1_1")
                                        {
                                            mailconhs[dataid] = "c1_1";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c1_1");
                                        ls.Add(mm);
                                    }
                                    
                                }
                                if ((timenow == c1_2_1) && (Convert.ToDouble(matchodds) > c1_2_2) && (Convert.ToDouble(matchodds) > 0))
                                {

                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString() != "c1_2")
                                        {
                                            mailconhs[dataid] = "c1_2";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c1_2");
                                        ls.Add(mm);
                                    }
                                }
                                if ((timenow == c1_3_1) && (Convert.ToDouble(matchodds) > c1_3_2) && (Convert.ToDouble(matchodds) > 0))
                                {

                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString() != "c1_3")
                                        {
                                            mailconhs[dataid] = "c1_3";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c1_3");
                                        ls.Add(mm);
                                    }
                                }
                                if ((timenow == c1_4_1) && (Convert.ToDouble(matchodds) > c1_4_2) && (Convert.ToDouble(matchodds) > 0))
                                {

                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString() != "c1_4")
                                        {
                                            mailconhs[dataid] = "c1_4";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c1_4");
                                        ls.Add(mm);
                                    }
                                }

                                //Tools.log.Info("执行平局:" + matchinfo + "|" + matchtime+"|"+matchscore+"|"+teaminfo+"|"+matchodds);

                            }
                            if (Math.Abs(Convert.ToInt32(matchscore.Split('-')[0]) - Convert.ToInt32(matchscore.Split('-')[1])) == 1)//C2
                            {
                                if ((timenow == c2_1_1) && (Convert.ToDouble(matchodds) > c2_1_2) && (Convert.ToDouble(matchodds) > 0))
                                {

                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString() != "c2_1")
                                        {
                                            mailconhs[dataid] = "c2_1";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c2_1");
                                        ls.Add(mm);
                                    }
                                }
                                if ((timenow == c2_2_1) && (Convert.ToDouble(matchodds) < c2_2_2) && (Convert.ToDouble(matchodds) > 0))
                                {

                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString() != "c2_2")
                                        {
                                            mailconhs[dataid] = "c2_2";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c2_2");
                                        ls.Add(mm);
                                    }
                                }
                                if ((timenow == c2_3_1) && (Convert.ToDouble(matchodds) < c2_3_2) && (Convert.ToDouble(matchodds) > 0))
                                {

                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString() != "c2_3")
                                        {
                                            mailconhs[dataid] = "c2_3";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c2_3");
                                        ls.Add(mm);
                                    }
                                }
                                if ((timenow == c2_4_1) && (Convert.ToDouble(matchodds) < c2_4_2) && (Convert.ToDouble(matchodds) > 0))
                                {

                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString() != "c2_4")
                                        {
                                            mailconhs[dataid] = "c2_4";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c2_4");
                                        ls.Add(mm);
                                    }
                                }


                                //Tools.log.Info("执行差一:" + matchinfo + "|" + matchtime + "|" + matchscore + "|" + teaminfo + "|" + matchodds);
                            }
                            if ((timenow >= c3_1_1) && (Math.Abs(Convert.ToInt32(matchscore.Split('-')[0]) - Convert.ToInt32(matchscore.Split('-')[1])) <= c3_1_2) && (Convert.ToDouble(ballcount) == Convert.ToDouble(gunbigsmallprehs[dataid].ToString())) && ((Convert.ToDouble(smallodds) - Convert.ToDouble(smalloddshs[dataid].ToString())) > (c3_1_3 * Convert.ToDouble(smalloddshs[dataid].ToString()))))//C3
                            {
                                Tools.log.Debug("C3=" + "matchinfo:" + matchinfo + "|matchscore:" + matchscore + "|teaminfo:" + teaminfo + "|matchtime:" + matchtime + "|matchodds:" + matchodds + "|smallodds:" + smallodds + "|dataid:" + dataid + "|scoretotal:" + scoretotal + "|scorehalf:" + scorehalf + "|ballcount:" + ballcount + "|chupanballcount:" + chupanballcount + "|goalslist:" + goalslist + "|stats:" + stats + "\n");
                                Tools.log.Debug("C3:前一分钟滚球大小球数=" + gunbigsmallprehs[dataid].ToString() + "|当前滚球大小球数=" + ballcount + "|前一分钟小球赔率=" + smalloddshs[dataid].ToString() + "|当前小球赔率=" + smallodds + "|增幅=" + (Convert.ToDouble(smallodds) - Convert.ToDouble(smalloddshs[dataid].ToString())).ToString() + "-" + (c3_1_3 * Convert.ToDouble(smalloddshs[dataid].ToString())).ToString());

                                if (mailconhs.ContainsKey(dataid))
                                {
                                    //if (mailconhs[dataid].ToString() != "c3_1")
                                    //{
                                        mailconhs[dataid] = "c3_1";
                                        ls.Add(mm);
                                    //}
                                }
                                else
                                {
                                    mailconhs.Add(dataid, "c3_1");
                                    ls.Add(mm);
                                }
                            }
                            if ((timenow == c4_1_1) && (Convert.ToInt32(scoretotal) == c4_1_2))//c4_1
                            {
                                if (mailconhs.ContainsKey(dataid))
                                {
                                    if (mailconhs[dataid].ToString() != "c4_1")
                                    {
                                        mailconhs[dataid] = "c4_1";
                                        ls.Add(mm);
                                    }
                                }
                                else
                                {
                                    mailconhs.Add(dataid, "c4_1");
                                    ls.Add(mm);
                                }

                            }
                            if ((timenow == c4_2_1) && (Convert.ToInt32(scoretotal) == c4_2_2))//c4_2
                            {
                                if (mailconhs.ContainsKey(dataid))
                                {
                                    if (mailconhs[dataid].ToString() != "c4_2")
                                    {
                                        mailconhs[dataid] = "c4_2";
                                        ls.Add(mm);
                                    }
                                }
                                else
                                {
                                    mailconhs.Add(dataid, "c4_2");
                                    ls.Add(mm);
                                }

                            }
                            if ((timenow == c4_3_1) && (Convert.ToInt32(scoretotal) == c4_3_2))//c4_3
                            {
                                if (mailconhs.ContainsKey(dataid))
                                {
                                    if (mailconhs[dataid].ToString() != "c4_3")
                                    {
                                        mailconhs[dataid] = "c4_3";
                                        ls.Add(mm);
                                    }
                                }
                                else
                                {
                                    mailconhs.Add(dataid, "c4_3");
                                    ls.Add(mm);
                                }

                            }
                            if ((timenow == c5_1_1) && (Convert.ToInt32(scoretotal) == c5_1_2) && (Convert.ToInt32(scorehalf) == c5_1_3))//c5_1
                            {
                                bool ok = true;

                                if ((!string.IsNullOrEmpty(goalslist)) && (ls2.Count>1))
                                {
                                    int temp = ls2[0];
                                    for (int i = 1; i < ls2.Count; i++)
                                    {
                                        ok=ok && ((ls2[i] - temp) > c5_1_4);
                                        temp = ls2[i];
                                    }
                                }
                                else
                                {
                                    ok = false;
                                }


                                if (ok)
                                {
                                    if (mailconhs.ContainsKey(dataid))
                                    {
                                        if (mailconhs[dataid].ToString() != "c5_1")
                                        {
                                            mailconhs[dataid] = "c5_1";
                                            ls.Add(mm);
                                        }
                                    }
                                    else
                                    {
                                        mailconhs.Add(dataid, "c5_1");
                                        ls.Add(mm);
                                    }
                                }
                                
                            }
                            if ((timenow > 46) && (Convert.ToInt32(scoretotal) == Convert.ToInt32(scorehalf)) && (Convert.ToDouble(ballcount) == (Convert.ToDouble(scoretotal)+c6_1_1)))//c6_1
                            {

                                Tools.log.Debug("*********************");
                                Tools.log.Debug("matchinfo:" + matchinfo + "|matchscore:" + matchscore + "|teaminfo:" + teaminfo + "|matchtime:" + matchtime + "|matchodds:" + matchodds + "|smallodds:" + smallodds + "|dataid:" + dataid + "|scoretotal:" + scoretotal + "|scorehalf:" + scorehalf + "|ballcount:" + ballcount + "|chupanballcount:" + chupanballcount + "|goalslist:" + goalslist);
                            
                                Tools.log.Debug("When条件：" + timenow.ToString() + "|scoretotal=" + scoretotal + "|scorehalf=" + scorehalf + "|ballcount=" + ballcount + "|" + (Convert.ToDouble(scoretotal) + c6_1_1).ToString());

                                if (gunbigsmallhs.ContainsKey(dataid))
                                {
                                    Tools.log.Debug("gunbigsmallhs|"+dataid+"|" + gunbigsmallhs[dataid].ToString());

                                    if (((Convert.ToDouble(chupanballcount) / 2) + Convert.ToDouble(scorehalf)) <= (Convert.ToDouble(gunbigsmallhs[dataid].ToString()) - c6_1_2))
                                    {
                                        Tools.log.Debug((Convert.ToDouble(chupanballcount) / 2).ToString() + "+" + Convert.ToDouble(scorehalf).ToString() + "<=" + Convert.ToDouble(gunbigsmallhs[dataid].ToString()).ToString()+"-"+c6_1_2.ToString());
                                        
                                        if (mailconhs.ContainsKey(dataid))
                                        {
                                            if (mailconhs[dataid].ToString() != "c6_1")
                                            {
                                                mailconhs[dataid] = "c6_1";
                                                ls.Add(mm);
                                            }
                                        }
                                        else
                                        {
                                            mailconhs.Add(dataid, "c6_1");
                                            ls.Add(mm);
                                        }
                                    }
                                }


                            }


                            //条件对比逻辑结束


                            //前一分钟滚球大小球数
                            if (gunbigsmallprehs.ContainsKey(dataid))
                            {
                                gunbigsmallprehs[dataid] = ballcount;
                            }
                            else
                            {
                                gunbigsmallprehs.Add(dataid, ballcount);
                            }

                            //前一分钟小球赔率
                            if (smalloddshs.ContainsKey(dataid))
                            {
                                smalloddshs[dataid] = smallodds;
                            }
                            else
                            {
                                smalloddshs.Add(dataid, smallodds);
                            }

                        }
                        catch (Exception ex)
                        {

                            Tools.log.Error(ex.ToString()+item.InnerHtml);
                            continue;
                        }
                        

                        

                   }


                    //return test;

                       
                        
                }

                //命中比赛提醒处理逻辑
                if (ls.Count>0)
                {

                    string resultlog = string.Empty;
                    foreach (Match item in ls)
                    {
                        result += "\n"+item.Matchinfo + ":时间：" + item.Matchtime + "|比分：" + item.Matchscore + "|球队：" + item.Teaminfo + "|欧滚球赔率：" + item.Matchodds+"|大小球赔率："+item.Smallodds+"|上半场进球数："+item.Scorehalf+"|滚球大小球数："+item.Ballcount+"|初盘大小球数："+item.Chupanballcount+"|进球时间点："+item.Goalslist+"|其它数据："+item.Stats+"\n";
                        result += "\n*******************************************\n";
                        resultlog = "\n" + item.Matchinfo + ":时间：" + item.Matchtime + "|比分：" + item.Matchscore + "|球队：" + item.Teaminfo + "|欧滚球赔率：" + item.Matchodds + "|大小球赔率：" + item.Smallodds + "|上半场进球数：" + item.Scorehalf + "|滚球大小球数：" + item.Ballcount + "|初盘大小球数：" + item.Chupanballcount + "|进球时间点：" + item.Goalslist + "|其它数据：" + item.Stats + "\n";
                        resultlog += "\n*******************************************\n";
                        Tools.log.Info(resultlog);
                        string address = Tools.ReadIniData("application", "mailaddress", "476011884@qq.com", inifilepath).ToString();
                        string[] addressarray = address.Split('|');
                        for (int i = 0; i < addressarray.Length; i++)
                        {
                            if (mailsubhs.ContainsKey(mailconhs[item.Dataid]))
                            {
                                Tools.log.Info("发送结果:" + SendEmail(addressarray[i].Trim(), mailsubhs[mailconhs[item.Dataid].ToString()].ToString(), resultlog).ToString());
                            }
                            else
                            {
                                Tools.log.Info("发送结果:" + SendEmail(addressarray[i].Trim(), item.Teaminfo + ":" + item.Matchtime, resultlog).ToString());
                            }
                            
                        }
                       
                    }

                    

                    
                }


                if (string.IsNullOrEmpty(result))
                {
                    result = "采集分析" + crawlcount.ToString() + "项纪录\n";
                }

            }
            catch (Exception e)
            {
                Tools.log.Error(e.ToString());
                if (string.IsNullOrEmpty(result))
                {
                    result = "采集分析" + crawlcount.ToString() + "项纪录\n";
                }
            }

            return result;
        }


        



        /// <summary>

        /// 发送邮件

        /// </summary>

        /// <param name="mailTo">要发送的邮箱</param>

        /// <param name="mailSubject">邮箱主题</param>

        /// <param name="mailContent">邮箱内容</param>

        /// <returns>返回发送邮箱的结果</returns>

        public bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {

            // 设置发送方的邮件信息,例如使用网易的smtp

            string smtpServer = Tools.ReadIniData("application", "smtp", "smtp.qq.com", inifilepath).ToString(); //SMTP服务器

            string mailFrom = Tools.ReadIniData("application", "smtpuser", "smtp.qq.com", inifilepath).ToString(); //登陆用户名

            string userPassword = Tools.ReadIniData("application", "smtppwd", "smtp.qq.com", inifilepath).ToString();//登陆密码



            // 邮件服务设置

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式

            smtpClient.Host = smtpServer; //指定SMTP服务器

            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            //smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            // 发送邮件设置        

            MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人

            mailMessage.Subject = mailSubject;//主题

            mailMessage.Body = mailContent;//内容

            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码

            mailMessage.IsBodyHtml = true;//设置为HTML格式

            mailMessage.Priority = MailPriority.Low;//优先级



            try
            {

                smtpClient.Send(mailMessage); // 发送邮件

                return true;

            }

            catch (SmtpException ex)
            {

                return false;

            }

        }

    }
}
