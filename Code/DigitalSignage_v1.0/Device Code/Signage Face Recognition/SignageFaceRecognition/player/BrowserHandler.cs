using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SignageFaceRecognition
{
    public static class BrowserHandler
    {
        private const string IWebBrowserAppGUID = "0002DF05-0000-0000-C000-000000000046";
        private const string IWebBrowser2GUID = "D30C1661-CDAF-11d0-8A3E-00C04FC9E26E";

        public static void SetSilent(System.Windows.Controls.WebBrowser browser, bool silent)
        {
            if (browser == null)
                MessageBox.Show("No Internet Connection");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid(IWebBrowserAppGUID);
                Guid IID_IWebBrowser2 = new Guid(IWebBrowser2GUID);

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }

    }
}
