using Notifications.Wpf;
using RestSharp;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml;

namespace Watchdog
{
    public class MainWindowViewModel:ViewModelBase
    {
        #region Declarations
        private bool _NotRunning = true;
        private string _IsRunningContent = "掃描";
        private int _Frequence = 60;
        private string _Keyword = "石牌;明德;天母;信義";
        Thread runner = null;

        #endregion

        #region Property
        /// <summary>
        /// 偵測頻率
        /// </summary>
        public int Frequence
        {
            get
            {
                return _Frequence;
            }
            set
            {
                _Frequence = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 服務沒有執行中
        /// </summary>
        public bool NotRunning
        {
            get
            {
                return _NotRunning;
            }
            set
            {
                _NotRunning = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 執行服務按鈕Content
        /// </summary>
        public string IsRunningContent
        {
            get
            {
                return _IsRunningContent;
            }
            set
            {
                _IsRunningContent = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 啟動或停止服務Command
        /// </summary>
        /// <param name="obj"></param>
        public RelayCommand ButtonClickCommand 
        {
            get 
            {
                return new RelayCommand(ButtonClickCommandAction); 
            }
        }

        public RelayCommand WindowsClosingCommand
        {
            get { return new RelayCommand(WindowClosingCommandAction); }
        }

        private void WindowClosingCommandAction(object obj)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 搜尋關鍵字
        /// </summary>
        public string Keyword
        {
            get
            {
                return _Keyword;
            }
            set
            {
                _Keyword = value;
                OnPropertyChanged();
            }
        }
        
        #endregion

        #region Memberfunction
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel() 
        {
            runner = new Thread(Crawler);
            runner.Start();
        }

        private void ButtonClickCommandAction(object obj)
        {
            NotRunning = !NotRunning;
            if (NotRunning)
            {
                IsRunningContent = "掃描";
                ((App)Application.Current).ShowMessage("服務停止", "偵測服務停止", NotificationType.Error);
            }
            else
            {
                IsRunningContent = "停止";
                ((App)Application.Current).ShowMessage("服務啟動", "偵測服務啟動", NotificationType.Information);
            }
        }
        
        private void Crawler() 
        {
            while (true) 
            {
                if (NotRunning) 
                {
                    Thread.Sleep(1000);
                }
                else 
                {
                    CrawlerAction();
                    Thread.Sleep(Frequence * 1000);
                }
            }
        }

        [STAThread]
        private void CrawlerAction() 
        {
            string url = "https://www.ptt.cc/bbs/Rent_apart/index.html";
            string contentType = "application/json"; //Content-Type

            try
            {
                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cookie", "__cf_bm=6f507558ebd6ddc8d9c0fe5e06db883792b27221-1623726774-1800-Afucop2w9vtCkuhMptwarRckX9UK6d2IKR1LYQU6pWvqssmGVkZh9yQqjN1h4s2/ngWCM/dF0kEx70+GAvDS+Lo=");
                IRestResponse response = client.Execute(request);


                // setup SgmlReader
                Sgml.SgmlReader sgmlReader = new Sgml.SgmlReader();
                sgmlReader.DocType = "HTML";
                sgmlReader.WhitespaceHandling = WhitespaceHandling.All;
                sgmlReader.CaseFolding = Sgml.CaseFolding.ToLower;
                sgmlReader.InputStream = new StringReader(response.Content);

                // create document
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.XmlResolver = null;
                doc.Load(sgmlReader);

                var allATag = doc.GetElementsByTagName("a");
                string[] keywords = null;
                if (Keyword.Substring(Keyword.Length - 1, 1).Equals(";"))
                    keywords = Keyword.Substring(0, Keyword.Length - 1).Split(';');
                else
                    keywords = Keyword.Split(';');


                foreach (var item in allATag) 
                {
                    var currentText = ((XmlNode)item).InnerText;
                    foreach(var key in keywords) 
                    {
                        if (currentText.Contains(key) && currentText.Contains("無"))
                        {
                            ((App)Application.Current).ShowMessage("找到符合物件", key + "\r\n" + currentText + "\r\n" + "http:\\yahoo.com.tw", NotificationType.Success);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ((App)Application.Current).ShowMessage("連線伺服器出錯", ex.Message, NotificationType.Error);
                
            }
        }
        #endregion
    }
}
