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
using System.Xml;
using System.Xml.Linq;
//using FMRaidoLoca.Date;
using FMRaidoLoca.Data;
using Microsoft.Devices.Radio;
using Microsoft.Phone.Info;
using System.Diagnostics;
using Com.MSN.Ads.WindowsPhone.SDK.View;


namespace FMRaidoLoca
{
    public partial class RadioListPageLongList : PhoneApplicationPage
    {

        FMRadio radio;
        string adSpaceId1 = "ODA2fDkzN3wyOTA5fDE=";
        string adSpaceId2 = "ODA2fDkzN3wyOTEwfDE=";
        string adSpaceId3 = "ODA2fDkzN3wyOTExfDE=";

        public RadioListPageLongList()
        {
            InitializeComponent();

            //TODO:Add AD 
            Thickness adPosition = new Thickness();//定义广告banner的位置。
            adPosition.Left = 0;
            adPosition.Top = 0;
            adPosition.Right = 0;
            adPosition.Bottom = 0;
            AdControl adControl = new AdControl(this);
            adControl.InitAd(adPosition, adSpaceId1);//初始化广告banner

            adControl.AdReveiceEvent += adControl_AdReveiceEvent;
            adControl.AdFailedEvent += adControl_AdFailedEvent;
            adControl.LeaveApplicationEvent += adControl_LeaveApplicationEvent;
            adControl.PresentScreenEvent += adControl_PresentScreenEvent;
            adControl.DismissScreenEvent += adControl_DismissScreenEvent;

            gridAD.Children.Add(adControl);
            adControl.LoadAd();


            //StartRadio();

           // MessageBox.Show(System.Environment.Version.ToString());

            LoadedData();

            this.radioListGropus.GroupViewOpened += new EventHandler<GroupViewOpenedEventArgs>(radioListGropus_GroupViewOpened);
            this.radioListGropus.GroupViewClosing += new EventHandler<GroupViewClosingEventArgs>(radioListGropus_GroupViewClosing);

            this.radioListGropus.SelectionChanged += new SelectionChangedEventHandler(radioListGropus_SelectionChanged);

            this.Loaded += new RoutedEventHandler(RadioListPageLongList_Loaded);
            this.pivot.SelectionChanged += new SelectionChangedEventHandler(pivot_SelectionChanged);
        }

        void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot.SelectedIndex == 0)
            {
                this.ApplicationBar.IsVisible = false;
            }
            else if (pivot.SelectedIndex == 1)
            {

                this.ApplicationBar.IsVisible = true;

                

            }
        }

        void adControl_DismissScreenEvent(AdControl adControl)
        {
            Debug.WriteLine("---------MSNLog--------adControl_DismissScreenEvent");
        }

        void adControl_PresentScreenEvent(AdControl adControl)
        {
            Debug.WriteLine("---------MSNLog--------adControl_PresentScreenEvent");
        }

        void adControl_LeaveApplicationEvent(AdControl adControl)
        {
            Debug.WriteLine("---------MSNLog--------adControl_LeaveApplicationEvent");
        }

        void adControl_AdFailedEvent(ErrorCode errorCode, AdControl adControl)
        {
            Debug.WriteLine("---------MSNLog--------adControl_AdFailedEvent");
        }

        void adControl_AdReveiceEvent(AdControl adControl)
        {
            Debug.WriteLine("---------MSNLog--------adView_AdReveiceEvent");
        }

        void RadioListPageLongList_Loaded(object sender, RoutedEventArgs e)
        {
            this.pivot.SelectedIndex = 1;
            try
            {
                //是否是第一次使用
                if (!AppConfig.IsUsedFirst)
                {
                    MessageBoxResult msgResult = MessageBox.Show("为了确保正常的使用调频收音机，需要设置国家和地区为“中国”，进入“设置”->“语言+区域”进行设置。", "使用提示", MessageBoxButton.OKCancel);
                    if (msgResult.Equals(MessageBoxResult.OK))
                    {
                        AppConfig.IsUsedFirst = true;
                       // System.Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-emailandaccounts"));
                       
                    }
                    
                }
            }
            catch (Exception ex )
            {
                Debug.WriteLine(ex.Message);
                UmengSDK.UmengAnalytics.TrackException(ex);
            }
            //if (pivot.SelectedIndex == 0)
            //{
            //    this.ApplicationBar.IsVisible = false;
            //}
            //else if (pivot.SelectedIndex == 1)
            //{
            //    this.ApplicationBar.IsVisible = true;
            //}
        }

        void radioListGropus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Radios radios = radioListGropus.SelectedItem as Radios;

            try
            {
                var app = App.Current as App;
                app.selectRasios = radioListGropus.SelectedItem as Radios;
                if (app.selectRasios != null)
                {
                    //this.NavigationService.Navigate(new Uri("/RadioMainPage.xaml", UriKind.Relative));
                    pivot.SelectedIndex = 1;

                    this.radioPivtoItem.Header = app.selectRasios.RadioName;
                    StartRadio();
                    radio = FMRadio.Instance;
                    //radio.PowerMode = RadioPowerMode.On;
                    radio.CurrentRegion = RadioRegion.Europe;
                    radio.Frequency = app.selectRasios.RadioFrequency;
                }
            }
            catch (Exception ex)
            {
                
                  UmengSDK.UmengAnalytics.TrackException(ex);
            }

           
        }

        void radioListGropus_GroupViewClosing(object sender, GroupViewClosingEventArgs e)
        {
            e.Cancel = true;

            SwivelTransition transition = new SwivelTransition();
            ItemContainerGenerator itemContainerGenerator = e.ItemsControl.ItemContainerGenerator;

            int animationFinished = 0;
            int itemCount = e.ItemsControl.Items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                UIElement element = itemContainerGenerator.ContainerFromIndex(i) as UIElement;

                ITransition animation = transition.GetTransition(element);
                animation.Completed += delegate
                {
                    // close the group view when all animations have completed
                    if ((++animationFinished) == itemCount)
                    {
                        radioListGropus.CloseGroupView();
                        radioListGropus.ScrollToGroup(e.SelectedGroup);
                    }
                };
                animation.Begin();
            }
        }

        void radioListGropus_GroupViewOpened(object sender, GroupViewOpenedEventArgs e)
        {
            ItemContainerGenerator itemContainerGenerator = e.ItemsControl.ItemContainerGenerator;
			TurnstileTransition turnstileTransition = new TurnstileTransition();
			turnstileTransition.Mode = TurnstileTransitionMode.ForwardIn;

			int itemCount = e.ItemsControl.Items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                UIElement element = itemContainerGenerator.ContainerFromIndex(i) as UIElement;
                ITransition animation = turnstileTransition.GetTransition(element);
                animation.Begin();
            }
        }

        /// <summary>
        /// 绑定电台数据
        /// </summary>
        private void LoadedData()
        {
            //List<Radios> source = new List<Radios>();

            XDocument loadedDate = XDocument.Load("RadioList.xml");
            var data = from query in loadedDate.Descendants("radio")
                       select new Radios
                       {
                           RadioName = (string)query.Element("name"),
                           RadioFrequency = (double)query.Element("frequency"),
                           Area = (string)query.Element("age"),
                           Group = (string)query.Element("group")
                       };
            
            var radioByGroup = from radio in data
                               group radio by radio.Group into c
                               orderby c.Key
                               select new Group<Radios>(c.Key, c);

            //var radioByGroup = from radio in data
            //                   group radio by radio.Area into c
            //                   orderby c.Key
            //                   select new Group<Radios>(c.Key, c);

            this.radioListGropus.ItemsSource = radioByGroup;

            

        }

        void StartRadio()
        {
            try
            {
                FMRadio.Instance.PowerMode = RadioPowerMode.On;
            }
           catch(Exception ex)
            {
          //      MessageBox.Show("This phone uses your headphones as an FM radio " +
          //"antenna. To listen to radio, connect your headphones.", "No antenna",
          //MessageBoxButton.OK);
                UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }

        void StopRadio()
        {
            try
            {
                FMRadio.Instance.PowerMode = RadioPowerMode.Off;
            }
            catch (Exception ex)
            {
                UmengSDK.UmengAnalytics.TrackException(ex);
            }
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FMRaidoLoca;component/AboutPage.xaml", UriKind.Relative));
        }

        //暂停播放
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            StopRadio();
        }
        //继续播放
        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            StartRadio();
            
        }

      
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
             
            double current = (double)Application.Current.Resources["PhoneDarkThemeOpacity"];
            //ThemeManager.OverrideOptions = ThemeManagerOverrideOptions.None;
            //ThemeManager.ToDarkTheme();
            if (current == 1)
            {
                gridLight.Visibility = Visibility.Visible;
                gridBlack.Visibility = Visibility.Collapsed;
                //currentTheme = Theme.Light;
            }
            else
            {
                gridBlack.Visibility = Visibility.Visible;
                gridLight.Visibility = Visibility.Collapsed;
            }
            base.OnNavigatedTo(e);
            UmengSDK.UmengAnalytics.TrackPageStart("RadioListPageLongList");

        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            UmengSDK.UmengAnalytics.TrackPageEnd("RadioListPageLongList");
        }
    }
}