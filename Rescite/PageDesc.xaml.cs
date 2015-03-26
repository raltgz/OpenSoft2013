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

namespace Citeseer
{
    /// <summary>
    /// Interaction logic for Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        MainWindow mw;
        Uri linkUrl;
        Uri linkConfUrl;

        public Page4(MainWindow mw)
        {
            this.mw = mw;
            InitializeComponent();
        }

        public void setYear(int year)
        {
            this.Year.Text = year.ToString();
        }

        public void setLink(Uri link)
        {
            this.linkUrl = link;
        }

        private void linkClick(object sender, RoutedEventArgs e)
        {
            this.mw.AddTabWebView(this.linkUrl);
        }


        private void linkClickConf(object sender, RoutedEventArgs e)
        {
            this.mw.AddTabWebView(this.linkConfUrl);
        }
    }
}
