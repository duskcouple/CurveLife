using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CefSharp.WinForms;
using CefSharp;
using System.Collections;
using System.IO;
using System.Net.Mail;



namespace Leisucrawler
{
    public partial class Form1 : Form
    {
        //public WebBrowser wb;
        public System.Timers.Timer timer1;
        public string inifilepath;
        public Crawler crawler;
        public bool isloaded = false;//是否已经载入过页面


        public ChromiumWebBrowser cb;

        public Form1()
        {
            InitializeComponent();
            
            InitConfig();
            InitTimer();
            
            //wb = new WebBrowser();
            crawler = new Crawler();
            //wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);

            CefSettings set = new CefSettings();
            set.WindowlessRenderingEnabled = true;
            CefSharp.Cef.Initialize(set);
            //Cef.Initialize();
            cb = new ChromiumWebBrowser("_blank");
            this.groupBox2.Controls.Add(cb);
            cb.FrameLoadEnd += new EventHandler<CefSharp.FrameLoadEndEventArgs>(cb_FrameLoadEnd);

            groupBox2.Width = 1;
            groupBox2.Height = 1;

            
        }

        void cb_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            //var task02 = e.Frame.GetSourceAsync();
            //task02.ContinueWith(t =>
            //{
            //    if (!t.IsFaulted)
            //    {
            //        string resultStr = t.Result;
            //        UpdateUI(() =>
            //    {
            //        //this.richTextBox1.AppendText(resultStr);
            //        string result = string.Empty;
            //        result = crawler.Crawl(resultStr);
            //        this.richTextBox1.AppendText(result);
            //    });
            //    }
            //});

            //string html = e.Frame.GetSourceAsync().Result;
            //this.richTextBox1.AppendText(html);

        }

        private void InitTimer()
        {
            //设置定时间隔(毫秒为单位)
            int interval = Convert.ToInt32(this.textBox11.Text);
            timer1 = new System.Timers.Timer(interval);
            //设置执行一次（false）还是一直执行(true)
            timer1.AutoReset = true;
            //设置是否执行System.Timers.Timer.Elapsed事件
            timer1.Enabled = false;
            //绑定Elapsed事件
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(TimerUp);
        }

        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            
            //this.richTextBox1.AppendText(this.webBrowser1.Document.Body.OuterHtml);
            //string matchhtml = wb.Document.Body.OuterHtml;
            //List<Match> ls = new List<Match>();
            //string result = string.Empty;
            //Task task = new Task(() =>
            //{

            //    result = crawler.Crawl(matchhtml);
            //    UpdateUI(() =>
            //    {
            //        this.richTextBox1.AppendText(result);
            //    });

            //});
            //task.Start();
            //task.Wait();
  


        }



        private void button2_Click(object sender, EventArgs e)
        {

            
        }




        /// <summary>
        /// 读取文件，返回相应字符串
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>返回文件内容</returns>
        private string ReadFile(string fileName)
        {
            StringBuilder str = new StringBuilder();
            using (FileStream fs = File.OpenRead(fileName))
            {
                long left = fs.Length;
                int maxLength = 100;//每次读取的最大长度
                int start = 0;//起始位置
                int num = 0;//已读取长度
                while (left > 0)
                {
                    byte[] buffer = new byte[maxLength];//缓存读取结果
                    char[] cbuffer = new char[maxLength];
                    fs.Position = start;//读取开始的位置
                    num = 0;
                    if (left < maxLength)
                    {
                        num = fs.Read(buffer, 0, Convert.ToInt32(left));
                    }
                    else
                    {
                        num = fs.Read(buffer, 0, maxLength);
                    }
                    if (num == 0)
                    {
                        break;
                    }
                    start += num;
                    left -= num;
                    str = str.Append(Encoding.UTF8.GetString(buffer));
                }
            }
            return str.ToString();
        }


         /// <summary>
        /// Timer类执行定时到点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerUp(object sender, System.Timers.ElapsedEventArgs e)
        {

            UpdateUI(() =>
            {
                //WebBrowser wb = new WebBrowser();
                //wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);



                if (!isloaded)
                {
                    //this.richTextBox1.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "【执行采集】\n");
                    //wb.Navigate("https://live.leisu.com?a=1&t="+DateTime.Now.Ticks);

                    cb.Load("https://live.leisu.com?a=1&t=" + DateTime.Now.Ticks);


                    Delay(15000);
                    isloaded = true;
                }

                String script =
                @"(function() {
                var tags = document.body;
                var result=tags.outerHTML;
                return result;
             })()";

                cb.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    var response = x.Result;

                    //this.richTextBox1.AppendText(response.Result.ToString());

                    if (response.Success && response.Result != null)
                    {

                        string result = string.Empty;
                        result = crawler.Crawl(response.Result.ToString());
                        UpdateUI(() =>
                        {
                            
                            this.richTextBox1.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "【执行采集】\n");
                            this.richTextBox1.AppendText(result);
                        });

                    }
                });

                
            });
        }


        /// <summary>
        /// 延时函数
        /// </summary>
        /// <param name="milliSecond">延时毫秒数</param>
        public void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }

        //多线程环境下修改主线程里面的控件需要用到此方法，如果用.net 4.5的await/async特性则无需此方法，具体请自己狗哥
        private void UpdateUI(Action uiAction)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(uiAction));
            }
            else
            {
                uiAction();
            }
        }

        private void InitConfig()
        {
            this.inifilepath = AppDomain.CurrentDomain.BaseDirectory + "AppConfig.ini";
            try
            {
                this.textBox1.Text = Tools.ReadIniData("application", "c1_1_1", "70", inifilepath);
                this.textBox2.Text = Tools.ReadIniData("application", "c1_1_2", "2", inifilepath);
                this.textBox3.Text = Tools.ReadIniData("application", "c1_2_1", "74", inifilepath);
                this.textBox4.Text = Tools.ReadIniData("application", "c1_2_2", "1.72", inifilepath);
                this.textBox5.Text = Tools.ReadIniData("application", "c1_3_1", "80", inifilepath);
                this.textBox6.Text = Tools.ReadIniData("application", "c1_3_2", "1.53", inifilepath);
                this.textBox12.Text = Tools.ReadIniData("application", "c1_4_1", "null", inifilepath);
                this.textBox13.Text = Tools.ReadIniData("application", "c1_4_2", "null", inifilepath);
                //
                this.textBox7.Text = Tools.ReadIniData("application", "c2_1_1", "70", inifilepath);
                this.textBox8.Text = Tools.ReadIniData("application", "c2_1_2", "3.5", inifilepath);
                this.textBox9.Text = Tools.ReadIniData("application", "c2_2_1", "80", inifilepath);
                this.textBox10.Text = Tools.ReadIniData("application", "c2_2_2", "4", inifilepath);
                this.textBox14.Text = Tools.ReadIniData("application", "c2_3_1", "null", inifilepath);
                this.textBox15.Text = Tools.ReadIniData("application", "c2_3_2", "null", inifilepath);
                this.textBox23.Text = Tools.ReadIniData("application", "c2_4_1", "null", inifilepath);
                this.textBox24.Text = Tools.ReadIniData("application", "c2_4_2", "null", inifilepath);
                //
                this.textBox11.Text = Tools.ReadIniData("application", "rate", "60000", inifilepath);
                this.textBox16.Text = Tools.ReadIniData("application", "mailaddress", "476011884@qq.com", inifilepath);
                this.textBox17.Text = Tools.ReadIniData("application", "smtp", "smtp.qq.com", inifilepath);
                this.textBox18.Text = Tools.ReadIniData("application", "smtpuser", "476011884@qq.com", inifilepath);
                this.textBox19.Text = Tools.ReadIniData("application", "smtppwd", "123456", inifilepath);
                //
                this.textBox20.Text = Tools.ReadIniData("application", "c3_1_1", "85", inifilepath);
                this.textBox21.Text = Tools.ReadIniData("application", "c3_1_2", "1", inifilepath);
                this.textBox22.Text = Tools.ReadIniData("application", "c3_1_3", "0.5", inifilepath);
                //
                this.textBox25.Text = Tools.ReadIniData("application", "c4_1_1", "31", inifilepath);
                this.textBox26.Text = Tools.ReadIniData("application", "c4_1_2", "3", inifilepath);
                this.textBox27.Text = Tools.ReadIniData("application", "c4_2_1", "74", inifilepath);
                this.textBox28.Text = Tools.ReadIniData("application", "c4_2_2", "6", inifilepath);
                this.textBox29.Text = Tools.ReadIniData("application", "c4_3_1", "null", inifilepath);
                this.textBox30.Text = Tools.ReadIniData("application", "c4_3_2", "null", inifilepath);
                //
                this.textBox31.Text = Tools.ReadIniData("application", "c5_1_1", "55", inifilepath);
                this.textBox32.Text = Tools.ReadIniData("application", "c5_1_2", "3", inifilepath);
                this.textBox33.Text = Tools.ReadIniData("application", "c5_1_3", "3", inifilepath);
                this.textBox34.Text = Tools.ReadIniData("application", "c5_1_4", "7", inifilepath);
                //
                this.textBox35.Text = Tools.ReadIniData("application", "c6_1_1", "3", inifilepath);
                this.textBox36.Text = Tools.ReadIniData("application", "c6_1_2", "7", inifilepath);
              
            }
            catch (Exception)
            {

                //throw;
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBox1.Text))
            {
                Tools.WriteIniData("application", "c1_1_1", this.textBox1.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox2.Text))
            {
                Tools.WriteIniData("application", "c1_1_2", this.textBox2.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox3.Text))
            {
                Tools.WriteIniData("application", "c1_2_1", this.textBox3.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox4.Text))
            {
                Tools.WriteIniData("application", "c1_2_2", this.textBox4.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox5.Text))
            {
                Tools.WriteIniData("application", "c1_3_1", this.textBox5.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox6.Text))
            {
                Tools.WriteIniData("application", "c1_3_2", this.textBox6.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox12.Text))
            {
                Tools.WriteIniData("application", "c1_4_1", this.textBox12.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox13.Text))
            {
                Tools.WriteIniData("application", "c1_4_2", this.textBox13.Text.Trim(), inifilepath);
            }
            //
            if (!string.IsNullOrEmpty(this.textBox7.Text))
            {
                Tools.WriteIniData("application", "c2_1_1", this.textBox7.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox8.Text))
            {
                Tools.WriteIniData("application", "c2_1_2", this.textBox8.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox9.Text))
            {
                Tools.WriteIniData("application", "c2_2_1", this.textBox9.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox10.Text))
            {
                Tools.WriteIniData("application", "c2_2_2", this.textBox10.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox14.Text))
            {
                Tools.WriteIniData("application", "c2_3_1", this.textBox14.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox15.Text))
            {
                Tools.WriteIniData("application", "c2_3_2", this.textBox15.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox23.Text))
            {
                Tools.WriteIniData("application", "c2_4_1", this.textBox23.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox24.Text))
            {
                Tools.WriteIniData("application", "c2_4_2", this.textBox24.Text.Trim(), inifilepath);
            }
            //
            if (!string.IsNullOrEmpty(this.textBox11.Text))
            {
                Tools.WriteIniData("application", "rate", this.textBox11.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox16.Text))
            {
                Tools.WriteIniData("application", "mailaddress", this.textBox16.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox17.Text))
            {
                Tools.WriteIniData("application", "smtp", this.textBox17.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox18.Text))
            {
                Tools.WriteIniData("application", "smtpuser", this.textBox18.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox19.Text))
            {
                Tools.WriteIniData("application", "smtppwd", this.textBox19.Text.Trim(), inifilepath);
            }
            //
            if (!string.IsNullOrEmpty(this.textBox20.Text))
            {
                Tools.WriteIniData("application", "c3_1_1", this.textBox20.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox21.Text))
            {
                Tools.WriteIniData("application", "c3_1_2", this.textBox21.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox22.Text))
            {
                Tools.WriteIniData("application", "c3_1_3", this.textBox22.Text.Trim(), inifilepath);
            }
            //
            if (!string.IsNullOrEmpty(this.textBox25.Text))
            {
                Tools.WriteIniData("application", "c4_1_1", this.textBox25.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox26.Text))
            {
                Tools.WriteIniData("application", "c4_1_2", this.textBox26.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox27.Text))
            {
                Tools.WriteIniData("application", "c4_2_1", this.textBox27.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox28.Text))
            {
                Tools.WriteIniData("application", "c4_2_2", this.textBox28.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox29.Text))
            {
                Tools.WriteIniData("application", "c4_3_1", this.textBox29.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox30.Text))
            {
                Tools.WriteIniData("application", "c4_3_2", this.textBox30.Text.Trim(), inifilepath);
            }
            //
            if (!string.IsNullOrEmpty(this.textBox31.Text))
            {
                Tools.WriteIniData("application", "c5_1_1", this.textBox31.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox32.Text))
            {
                Tools.WriteIniData("application", "c5_1_2", this.textBox32.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox33.Text))
            {
                Tools.WriteIniData("application", "c5_1_3", this.textBox33.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox34.Text))
            {
                Tools.WriteIniData("application", "c5_1_4", this.textBox34.Text.Trim(), inifilepath);
            }
            //
            if (!string.IsNullOrEmpty(this.textBox35.Text))
            {
                Tools.WriteIniData("application", "c6_1_1", this.textBox35.Text.Trim(), inifilepath);
            }
            if (!string.IsNullOrEmpty(this.textBox36.Text))
            {
                Tools.WriteIniData("application", "c6_1_2", this.textBox36.Text.Trim(), inifilepath);
            }

            this.richTextBox1.AppendText(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "  配置修改保存成功！\n");
            MessageBox.Show("配置更改后，必须重启软件后才能生效！");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.button1.Text == "停止监控")
            {
                this.timer1.Stop();
                this.button1.Text = "开始监控";
            }
            else if (this.button1.Text == "开始监控")
            {
                this.timer1.Start();
                this.button1.Text = "停止监控";
                TimerUp(null, null);

            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Show();
                //还原窗体显示    
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                notifyIcon1.Visible = false;
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                this.Hide();
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        
    }
}
