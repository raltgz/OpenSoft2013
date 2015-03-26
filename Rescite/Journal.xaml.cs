using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Journal.xaml
    /// </summary>
    public partial class JournalPage : Page
    {
        TabPage tp;
        public JournalPage(TabPage tp)
        {
            this.tp = tp;
            InitializeComponent();
        }

        private void showStats(object sender, RoutedEventArgs e)
        {
            this.tp.showJourStats();
        }

        private void showProfile(object sender, RoutedEventArgs e)
        {
            this.tp.showJourProfile();
        }
    }
}
