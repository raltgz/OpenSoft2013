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
using System.IO;

namespace Citeseer
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        MainWindow mw;
        public Page5(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
        }

        private void viewHelp(object sender, RoutedEventArgs e)
        {
            Uri url = new Uri(System.IO.Path.GetFullPath("Help/rescite.html"), UriKind.RelativeOrAbsolute);
            mw.AddTabWebView(url, 1);
        }
    }
}
