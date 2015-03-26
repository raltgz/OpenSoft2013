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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<TabItem> _tabItems; //list of all tabs
        //public List<Page> _pageitems;
        private TabItem _tabAdd; //holds reference to the last tab to add new tab
        /*private Grid searchPanel;
        private TextBox searchBox; //the search box
        private RadioButton radioAuthor, radioJournal; //the search type radio buttons
        private RadioButton radioCitations, radioDate; //the sort type radio buttons
        private CheckBox checkBoxCustomRange; //the checkbox for custom ranges
        private TextBox txtBoxYearStart, txtBoxYearEnd; //the custom range search*/

        private static int counter = 0;
        private static int temp = 1;
        private Favourites favourites;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                favourites = new Favourites();
                /*
                 * Initially two tabs are created:
                 * One for the search
                 * Other for adding a new tab
                 */

                // initialize tabItem array
                _tabItems = new List<TabItem>();

                // add a tabItem with + in header 
                _tabAdd = new TabItem();
                _tabAdd.Header = "+";
                _tabAdd.Background = Brushes.LightGray;
                _tabAdd.BorderBrush = Brushes.DarkGray;

                _tabItems.Add(_tabAdd);

                // add first tab
                this.AddTabItem();

                // bind tab control
                tabDynamic.DataContext = _tabItems;

                tabDynamic.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /*
         * Used to initialize content of a new tab
         */
        public TabItem AddTabItem()
        {
            int count = _tabItems.Count;
            counter++;
            temp++;
            // create new tab item
            TabItem tab = new TabItem();

            tab.Header = string.Format("New Search");
            tab.Name = string.Format("tab{0}", counter);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;

            tab.Background = new SolidColorBrush(Color.FromRgb(230, 234, 238));
            tab.BorderBrush = Brushes.LightGray;
            tab.MinWidth = 100;
            tab.Width = 150;
            tab.MaxWidth = 150;


            TabPage page = new TabPage(this, favourites);
            ScrollViewer g = new ScrollViewer();
            g = page.MainPane;
            page.Content = null;
            _tabItems.Insert(count - 1, tab);

            tab.Content = g;

            return tab;
        }

        public TabItem AddTabFavorites()
        {
            int count = _tabItems.Count;
            counter++;
            temp++;
            // create new tab item
            TabItem tab = new TabItem();

            tab.Header = string.Format("Favorites");
            tab.Name = string.Format("tab{0}", counter);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;

            tab.Background = new SolidColorBrush(Color.FromRgb(230, 234, 238));
            tab.BorderBrush = Brushes.LightGray;
            tab.MinWidth = 100;
            tab.Width = 150;
            tab.MaxWidth = 150;


            FavouritesPage page = new FavouritesPage(this, favourites);
            Grid g = new Grid();
            g = page.favorites;
            page.Content = null;
            _tabItems.Insert(count - 1, tab);

            tab.Content = g;

            return tab;
        }

        public TabItem AddTabWebView(Uri url, int flag = 0)
        {
            tabDynamic.DataContext = null;
            temp++;

            int count = _tabItems.Count;
            counter++;
            // create new tab item
            TabItem tab = new TabItem();

            if (flag == 0)
                tab.Header = string.Format(url.ToString());
            else
                tab.Header = "Help";
            tab.Name = string.Format("tab{0}", counter);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;

            tab.Background = new SolidColorBrush(Color.FromRgb(230, 234, 238));
            tab.BorderBrush = Brushes.LightGray;
            tab.MinWidth = 100;
            tab.Width = 150;
            tab.MaxWidth = 150;



            Webview page = new Webview();
            Grid g = new Grid();
            page.setUrl(url);
            g = page.browser;
            page.Content = null;
            tab.Content = g;
            _tabItems.Insert(count - 1, tab);

            tabDynamic.DataContext = _tabItems;

            // select newly added tab item
            tabDynamic.SelectedItem = tab;

            return tab;

        }


        public TabItem AddTabHtmlView(Journal j)
        {
            tabDynamic.DataContext = null;
            temp++;

            int count = _tabItems.Count;
            counter++;
            // create new tab item
            TabItem tab = new TabItem();
            
            
            tab.Name = string.Format("tab{0}", counter);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;
            tab.Header = j.getname();
            tab.Background = new SolidColorBrush(Color.FromRgb(230, 234, 238));
            tab.BorderBrush = Brushes.LightGray;
            tab.MinWidth = 100;
            tab.Width = 150;
            tab.MaxWidth = 150;



            Webview page = new Webview();
            Grid g = new Grid();
            page.webView.NavigateToString(j.gethtmlStats());
            g = page.browser;
            page.Content = null;
            tab.Content = g;
            _tabItems.Insert(count - 1, tab);

            tabDynamic.DataContext = _tabItems;

            // select newly added tab item
            tabDynamic.SelectedItem = tab;

            return tab;

        }

        public TabItem AddTabProfilePage(AuthorProfile auth)
        {
            int count = _tabItems.Count;
            counter++;
            temp++;
            // create new tab item
            TabItem tab = new TabItem();

            tab.Header = string.Format(auth.getName());
            tab.Name = string.Format("tab{0}", counter);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;

            tab.Background = new SolidColorBrush(Color.FromRgb(230, 234, 238));
            tab.BorderBrush = Brushes.LightGray;
            tab.MinWidth = 100;
            tab.Width = 150;
            tab.MaxWidth = 150;




            StackPanel g = new StackPanel();

            g = auth.authorProfile;
            auth.Content = null;
            tab.Content = g;
            _tabItems.Insert(count - 1, tab);

            return tab;
        }

        private void tabAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // clear tab control binding
            tabDynamic.DataContext = null;

            TabItem tab = this.AddTabItem();

            // bind tab control
            tabDynamic.DataContext = _tabItems;

            // select newly added tab item
            tabDynamic.SelectedItem = tab;
        }

        private void tabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tab = tabDynamic.SelectedItem as TabItem;
            if (tab == null) return;

            if (tab.Equals(_tabAdd))
            {
                //status.Text = "added tab";
                // clear tab control binding
                tabDynamic.DataContext = null;

                TabItem newTab = this.AddTabItem();

                // bind tab control
                tabDynamic.DataContext = _tabItems;

                // select newly added tab item
                tabDynamic.SelectedItem = newTab;
            }
            else
            {

            }
        }

        //delete tab
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();
            temp--;
            var item = tabDynamic.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();

            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (_tabItems.Count < 3)
                {
                    this.Close();
                }

                // get selected tab
                TabItem selectedTab = tabDynamic.SelectedItem as TabItem;

                int i = _tabItems.IndexOf(tab);
                // clear tab control binding
                tabDynamic.DataContext = null;

                _tabItems.Remove(tab);
                // bind tab control
                tabDynamic.DataContext = _tabItems;

                // select previously selected tab. if that is removed then select first tab

                if (i < _tabItems.Count - 1)
                    selectedTab = _tabItems[i];
                else
                {
                    try
                    {
                        selectedTab = _tabItems[i - 1];
                    }
                    catch (Exception)
                    {
                    }
                }

                tabDynamic.SelectedItem = selectedTab;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.T && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                tabDynamic.DataContext = null;

                TabItem newTab = this.AddTabItem();

                // bind tab control
                tabDynamic.DataContext = _tabItems;

                // select newly added tab item
                tabDynamic.SelectedItem = newTab;
            }

            if (e.Key == Key.W && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                if (_tabItems.Count < 3)
                {
                    this.Close();
                }

                // get selected tab
                TabItem selectedTab = tabDynamic.SelectedItem as TabItem;
                int i = _tabItems.IndexOf(selectedTab);
                // clear tab control binding
                tabDynamic.DataContext = null;

                _tabItems.Remove(selectedTab);
                // bind tab control
                tabDynamic.DataContext = _tabItems;

                // select previously selected tab. if that is removed then select first tab
                if (i < _tabItems.Count - 1)
                    selectedTab = _tabItems[i];
                else
                {
                    try
                    {
                        selectedTab = _tabItems[i - 1];
                    }
                    catch (Exception)
                    {
                    }
                }

                tabDynamic.SelectedItem = selectedTab;
            }


            if (e.Key == Key.Tab && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
            {

                int i = tabDynamic.SelectedIndex;
                if (i == 0)
                {
                    tabDynamic.SelectedIndex = _tabItems.Count - 2;
                    e.Handled = true;
                }
                else
                {
                    tabDynamic.SelectedIndex = (i - 1);
                }
                e.Handled = true;
                return;

            }
            else
            {

                if (e.Key == Key.Tab &&
                         (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {


                    int i = tabDynamic.SelectedIndex;
                    if (i == _tabItems.Count - 2)
                    {
                        tabDynamic.SelectedIndex = 0;
                        e.Handled = true;
                    }
                    else
                    {
                        tabDynamic.SelectedIndex = (i + 1);
                    }
                    e.Handled = true;

                }
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {

                switch (e.Key)
                {
                    case Key.D1:
                        try
                        {
                            if (0 != (_tabItems.Count - 1))
                                tabDynamic.SelectedIndex = 0;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case Key.D2:
                        try
                        {
                            if (1 != (_tabItems.Count - 1))
                                tabDynamic.SelectedIndex = 1;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case Key.D3:
                        try
                        {
                            if (2 != (_tabItems.Count - 1))
                                tabDynamic.SelectedIndex = 2;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case Key.D4:
                        try
                        {
                            if (3 != (_tabItems.Count - 1))
                                tabDynamic.SelectedIndex = 3;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case Key.D5:
                        try
                        {
                            if (4 != (_tabItems.Count - 1))
                                tabDynamic.SelectedIndex = 4;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case Key.D6:
                        try
                        {
                            if (5 != (_tabItems.Count - 1))
                                tabDynamic.SelectedIndex = 5;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case Key.D7:
                        try
                        {
                            if (6 != (_tabItems.Count - 1))
                                tabDynamic.SelectedIndex = 6;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case Key.D8:
                        try
                        {
                            if (7 != (_tabItems.Count - 1))
                                tabDynamic.SelectedIndex = 7;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case Key.D9:
                        try
                        {
                            tabDynamic.SelectedIndex = _tabItems.Count - 2;
                        }
                        catch (Exception)
                        {
                        }
                        break;
                }
            }
        }
    }
}
