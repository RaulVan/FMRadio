using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Resources;
using System.IO;
using System.Diagnostics;


namespace FMRaidoLoca
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
            emailLink.Click += new RoutedEventHandler(emailLink_Click);
            this.Loaded += AboutPage_Loaded;
        }

        void AboutPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtVersion.Text ="版本："+  GetVersion();
            txtRemark.Text = "   （China）   目前本版本支持WP7手机，WP8手机部分(例如之前的Lumia520)不支持本地收音机，为了确保正常的使用调频收音机，需要设置国家和地区为“中国”，进入“设置”->“语言+区域”进行设置。请谅解！有新创意，也可邮件予我。";
        }

        void emailLink_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask ect = new EmailComposeTask();
            ect.To = "raulgblanco@live.com";
            ect.Subject = "电台反馈";
            ect.Show();

        }
         string version = null;

        internal  string GetVersion()
        {
            try
            {
                var manifestUri = new Uri("WMAppManifest.xml", UriKind.Relative);
                StreamResourceInfo manifestStream = Application.GetResourceStream(manifestUri);
                // string wmapp= manifestStream.ToString();
                if (manifestStream != null)
                {
                    using (var sr = new StreamReader(manifestStream.Stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (line.IndexOf("<App xmlns") != -1)
                            {
                                int st;
                                if ((st = line.IndexOf("Version=\"")) != -1)
                                {
                                    line = line.Substring(st + 9);
                                    st = line.IndexOf("\"");
                                    line = line.Substring(0, st);
                                    version = line;
                                    break;
                                }
                            }
                        }
                    }
                }
                //Debug.WriteLine(wmapp);
                //PKG = wmapp;]

                version = HttpUtility.UrlEncode(version);
               
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return version;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            UmengSDK.UmengAnalytics.TrackPageEnd("AboutPage");
            
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            UmengSDK.UmengAnalytics.TrackPageStart("AboutPage");
        }

        private void Link1_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        private void Link2_Click(object sender, RoutedEventArgs e)
        {

            MarketplaceSearchTask task = new MarketplaceSearchTask();
            task.ContentType = MarketplaceContentType.Applications;
            task.SearchTerms = "ElevenVan";
            task.Show();
        }
    }
}