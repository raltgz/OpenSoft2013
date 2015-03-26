using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Reflection;

namespace Citeseer
{
    /// <summary>
    /// Interaction logic for Webview.xaml
    /// </summary>
    public partial class Webview : Page
    {
        public Webview()
        {
            InitializeComponent();
            webView.Navigated += new NavigatedEventHandler(browser_Navigated);
        }

        void browser_Navigated(object sender, NavigationEventArgs e)
        {
            HideScriptErrors(webView, true);
        }

        public void setUrl(Uri url)
        {
            webView.Navigate(url);
        }

        public void setSource(String source)
        {
            webView.Navigate(source);
        }

        public void HideScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null)
                return;

            object objComWebBrowser = fiComWebBrowser.GetValue(wb);

            if (objComWebBrowser == null)
                return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });

        }
    }
}
