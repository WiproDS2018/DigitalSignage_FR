using DigitalSignageDps.player;
using DigitalSignageDps.Provision;
using IWshRuntimeLibrary;
using Microsoft.Azure.Devices.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DigitalSignageDps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DeviceProvision provision = new DeviceProvision();
        DeviceClient deviceClient = null;
        queryResultsChannel weatherData = null;
        WeatherHelper weatherHelper;
        Thread playerThread = null;
        Thread weatherThread;
        Thread messengerThread = null;
        bool isMinimised = true;
        SignagePlayer player;
        private WebBrowserHostUIHandler _wbHandler;
        private bool isBrowserPlaying = false;
        public MainWindow()
        {
            InitializeComponent();
            DigitalSignageDps.Enviornment.PrepareEnviornment();
            Logger.LogToConnector("Enviornment Configured");
            SignagePlayer.CreateConfigXML();
            if (System.IO.File.Exists("C:/Signage/Config/settings.json"))
            {
                string content = System.IO.File.ReadAllText("C:/Signage/Config/settings.json");
                try
                {
                    Settings settings = Settings.FromJson(content);
                    if (settings.IotDpsIdScope != null && settings.NodeServerUrl != null
                        && settings.IotDpsIdScope.Value != null && settings.NodeServerUrl.Value != null)
                    {
                        Enviornment.WebApiAddress = settings.NodeServerUrl.Value;
                        Enviornment.DeviceProvisioningScopeId = settings.IotDpsIdScope.Value;
                        deviceId.Text = TpmGenerator.GetGenerator().RegistrationId;
                        Thread mainThread = new Thread(MainThread);
                        mainThread.IsBackground = true;
                        mainThread.Start();
                        deviceId.MouseRightButtonDown += DeviceId_MouseRightButtonDown;
                    }
                    else
                    {
                        message.Text = "Invalid settings.json file inside C:/Signage/Config";
                        Logger.LogToConnector("Invalid settings.json file inside C:/Signage/Config");
                        progress.Visibility = Visibility.Hidden;
                    }
                }
                catch (Exception e)
                {
                    message.Text = "Invalid settings.json file inside C:/Signage/Config";
                    Logger.LogToConnector("Invalid settings.json file inside C:/Signage/Config");
                    progress.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                message.Text = "Did not found settings.json file inside C:/Signage/Config";
                Logger.LogToConnector("Did not found settings.json file inside C:/Signage/Config");
                progress.Visibility = Visibility.Hidden;
            }
            _wbHandler = new WebBrowserHostUIHandler(meBrowser, this);
            _wbHandler.IsWebBrowserContextMenuEnabled = false;
            _wbHandler.ScriptErrorsSuppressed = true;
            // CreateShortcut();
        }



        private void MainThread()
        {
            while (!CheckForInternetConnection())
            {
                message.Dispatcher.Invoke(() =>
                {
                    message.Text = "Waiting for Internet Connection";
                });
            }
            try
            {
                progress.Dispatcher.Invoke(() =>
                {
                    progress.Visibility = Visibility.Visible;
                });
                if (TpmGenerator.GetGenerator().TpmAvailable)
                {
                    Task<ProvisionResult> resultTask = provision.EnrollDevice(message, progress);
                    ProvisionResult result = resultTask.Result;
                    provision.CreateDeviceClient(message, progress);
                    deviceClient = provision.IotClient;
                }
                else
                {
                    Logger.LogToConnector("No Tpm found");
                    message.Dispatcher.Invoke(() => { message.Text = "Connecting"; });
                    deviceClient = provision.CreateDeviceClient();
                    message.Dispatcher.Invoke(() => { message.Text = "Double click on device id to goto player\nRight Click on deviceId to copy to clipboard"; }); ;
                }
                DeviceMessenger messenger = new DeviceMessenger(deviceClient);
                messenger.StartMessenger();

                this.Dispatcher.Invoke(() =>
                {
                    Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;

                    border.Visibility = Visibility.Hidden;
                    message.Visibility = Visibility.Hidden;
                    deviceId.Visibility = Visibility.Hidden;
                    heading.Visibility = Visibility.Hidden;
                    weather.Visibility = Visibility.Visible;
                    progress.Visibility = Visibility.Hidden;
                });


                player = new SignagePlayer(mePlayer, meBrowser, weather);
                playerThread = new Thread(player.DisplayImageXML);

                playerThread.IsBackground = true;
                playerThread.Start();

                this.Dispatcher.Invoke(() =>
                {
                    weather.Visibility = Visibility.Visible;
                    this.timeText.Text = DateTime.Now.ToString("hh : mm tt", CultureInfo.InvariantCulture);
                    this.dateText.Text = DateTime.Now.ToString("dddd, MMM dd, yyyy");
                });

                DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 1, 0), DispatcherPriority.Normal, delegate
                {
                    this.timeText.Text = DateTime.Now.ToString("hh : mm tt", CultureInfo.InvariantCulture);
                    this.dateText.Text = DateTime.Now.ToString("dddd, MMM dd, yyyy");
                }, this.Dispatcher);
                weatherThread = new Thread(GetWeatherData);
                weatherThread.IsBackground = true;
                weatherThread.Start();
            }
            catch (Exception e)
            {
                this.Dispatcher.Invoke(() =>
                {
                    message.Text = "Please wait while application restarts";
                    progress.Visibility = Visibility.Hidden;
                });
            }

        }

        private void GetWeatherData()
        {

            try
            {
                weatherHelper = new WeatherHelper();
                weatherData = weatherHelper.GetWeatherData();
                this.Dispatcher.Invoke(() =>
                {
                    this.tempText.Text = weatherData.item.condition.temp.ToString() + "° F";
                    this.weatherIcon.Source = new BitmapImage(new Uri($"http://l.yimg.com/a/i/us/we/52/{weatherData.item.condition.code}.gif"));
                });

                DispatcherTimer weatherTimer = new DispatcherTimer(new TimeSpan(1, 0, 0), DispatcherPriority.Normal, delegate
                {
                    weatherData = weatherHelper.GetWeatherData();
                    Logger.LogToPlayer($"Temperature : {weatherData.item.condition.temp.ToString() } ° F");
                    this.tempText.Text = weatherData.item.condition.temp.ToString() + "° F";
                    this.weatherIcon.Source = new BitmapImage(new Uri($"http://l.yimg.com/a/i/us/we/52/{weatherData.item.condition.code}.gif"));
                }, this.Dispatcher);
            }
            catch (Exception e)
            {
                Logger.LogToPlayer(e.ToString());
            }

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2) return;
            Logger.LogToPlayer("Mouse Double Click");
            string prevMessage = message.Text;
            if (isMinimised && playerThread != null)
            {
                isMinimised = false;
                Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                border.Visibility = Visibility.Hidden;
                message.Visibility = Visibility.Hidden;
                deviceId.Visibility = Visibility.Hidden;
                prevMessage = message.Text;
                heading.Visibility = Visibility.Hidden;
                weather.Visibility = Visibility.Visible;
                progress.Visibility = Visibility.Hidden;
                if (isBrowserPlaying)
                {
                    isBrowserPlaying = false;
                    meBrowser.Visibility = Visibility.Visible;
                }
                else mePlayer.Visibility = Visibility.Visible;
                player.Pause = false;
                Logger.LogToPlayer("Maximised");
            }
            else if (!isMinimised)
            {
                isMinimised = true;
                Logger.LogToPlayer("Minimised");
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                Application.Current.MainWindow.Width = 800;
                Application.Current.MainWindow.Height = 200;
                Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
                Application.Current.MainWindow.WindowStyle = WindowStyle.ThreeDBorderWindow;
                border.Visibility = Visibility.Visible;
                deviceId.Visibility = Visibility.Visible;
                heading.Visibility = Visibility.Visible;
                message.Visibility = Visibility.Visible;
                message.Text = prevMessage;
                weather.Visibility = Visibility.Hidden;
                player.Pause = true;
                mePlayer.Visibility = Visibility.Hidden;
                meBrowser.Visibility = Visibility.Hidden;
            }
        }

        private void DeviceId_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) return;
            Clipboard.SetText(deviceId.Text);
            Thread thread = new Thread(() =>
              {
                  string prev = "";
                  message.Dispatcher.Invoke(() =>
                  {
                      prev = message.Text;
                      message.Text = " deviceId to copied to clipboard";
                  });
                  Thread.Sleep(3000);
                  message.Dispatcher.Invoke(() => { message.Text = prev; });
              });
            thread.IsBackground = true;
            thread.Start();
        }



        private void CreateShortcut()
        {
            string appname = Assembly.GetExecutingAssembly().FullName.Remove(Assembly.GetExecutingAssembly().FullName.IndexOf(","));
            string startupPathName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), appname + ".lnk");
            if (System.IO.File.Exists(startupPathName)) return;
            string shortcutTarget = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().Location), appname + ".exe");
            WshShell myShell = new WshShell();
            WshShortcut myShortcut = (WshShortcut)myShell.CreateShortcut(startupPathName);
            myShortcut.TargetPath = shortcutTarget; //The exe file this shortcut executes when double clicked 
            myShortcut.IconLocation = shortcutTarget + ",0"; //Sets the icon of the shortcut to the exe`s icon 
            myShortcut.WorkingDirectory = System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().Location); //The working directory for the exe 
            myShortcut.Arguments = ""; //The arguments used when executing the exe 
            myShortcut.Save(); //Creates the shortcut


        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "www.google.com";
                byte[] buffer = new byte[32];
                int timeout = 30000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {

            System.Environment.Exit(0);
        }

        private void meBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            // BrowserHandler.SetSilent(meBrowser, true); // make it silent
            // IHTMLDocument2 doc=meBrowser.Document;
            //mshtml.HTMLDocument doc = (mshtml.HTMLDocument)meBrowser.Document;
            //mshtml.HTMLDocumentEvents2_Event iEvent = (mshtml.HTMLDocumentEvents2_Event)doc;
            //iEvent.ondblclick += new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(ClickEventHandler);

        }

        public void ClickEventHandler()
        {
            try
            {
                Logger.LogToPlayer("Double click inside Browser");
                string prevMessage = message.Text;
                isMinimised = true;
                isBrowserPlaying = true;
                Logger.LogToPlayer("Minimised");
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                Application.Current.MainWindow.Width = 800;
                Application.Current.MainWindow.Height = 200;
                Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
                Application.Current.MainWindow.WindowStyle = WindowStyle.ThreeDBorderWindow;
                border.Visibility = Visibility.Visible;
                deviceId.Visibility = Visibility.Visible;
                heading.Visibility = Visibility.Visible;
                message.Visibility = Visibility.Visible;
                message.Text = prevMessage;
                weather.Visibility = Visibility.Hidden;
                player.Pause = true;
                mePlayer.Visibility = Visibility.Hidden;
                meBrowser.Visibility = Visibility.Hidden;
            }catch(Exception e)
            {
                Logger.LogToPlayer(e.ToString());
            }
        }
    



        private void meBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            
            //mshtml.HTMLDocument doc = (mshtml.HTMLDocument)meBrowser.Document;
            //doc.body.onclick += new HtmlElementEventHandler();
            //mshtml.HTMLDocumentEvents2_Event iEvent = (mshtml.HTMLDocumentEvents2_Event)doc;
            //iEvent.oncontrolselect += (obj) => { return false; };
            //iEvent.onclick += (obj) => { Logger.LogToPlayer(obj.)};
            //iEvent.ondblclick += new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(ClickEventHandler);
            //iEvent.oncontextmenu += (obj) => { return false; };

        }

        private void meBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            //mshtml.HTMLDocument doc = (mshtml.HTMLDocument)meBrowser.Document;
            //mshtml.HTMLDocumentEvents2_Event iEvent = (mshtml.HTMLDocumentEvents2_Event)doc;
            //iEvent.ondblclick += new mshtml.HTMLDocumentEvents2_ondblclickEventHandler(ClickEventHandler);
            //iEvent.oncontextmenu += (obj) => { return false; };
        }
    }
}
