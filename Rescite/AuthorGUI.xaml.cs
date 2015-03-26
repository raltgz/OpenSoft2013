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
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        TabPage tabpage;
        public Page3(TabPage tabpage)
        {
            this.tabpage = tabpage;
            InitializeComponent();
        }

        private void showStats(object sender, RoutedEventArgs e)
        {
            this.tabpage.showStats();
        }

    }

    
}
