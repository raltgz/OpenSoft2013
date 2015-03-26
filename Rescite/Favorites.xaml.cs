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
    /// Interaction logic for Favorites.xaml
    /// </summary>
    public partial class FavouritesPage : Page
    {
        MainWindow mw;
        Favourites f;
        public FavouritesPage(MainWindow mw, Favourites fav)
        {
            this.f = fav;
            InitializeComponent();
            this.mw = mw;
            FavoritesResult page;
            ListBoxItem item;
            int flag = 0;
            foreach (Query q in f.getFavourites())
            {
                flag = 1;
                page = new FavoritesResult(mw, f, this);
                item = new ListBoxItem();
                page.setQueryString(q.getqueryString());
                String s = "";
                if (q.sortOrder == SortOrder.CITATIONS)
                    s = s + "Sorted by Citations, ";
                else
                    s = s + "Sorted by Year, ";
                if (q.issetylo() == true)
                    s = s + "From " + q.getylo();
                if (q.issetyhi() == true)
                    s = s + " To " + q.getyhi();
                page.setQueryParams(s);
                page.setSearch(q);
                if (q.resultType == ResultType.AUTHOR)
                    page.setSearchType("Search by Author");
                else
                    page.setSearchType("Search by Journal");
                item = page.listItem;
                page.Content = null;
                this.favoritesPane.Items.Add(item);
            }
            if (flag == 0)
            {
                page = new FavoritesResult(mw, f, this);
                item = new ListBoxItem();
                page.setQueryString("            YOUR FAVORITES LIST IS EMPTY!");
                page.btnViewSearch.IsEnabled = false;
                page.btnViewSearch.Visibility = Visibility.Hidden;
                page.btnDelete.IsEnabled = false;
                page.btnDelete.Visibility = Visibility.Hidden;
                item = page.listItem;
                page.Content = null;
                this.favoritesPane.Items.Add(item);
            }
        }

        private void clear(object sender, RoutedEventArgs e)
        {
            f.clearFavourites();
            Grid g = new Grid();
            TabItem tab = mw.tabDynamic.SelectedItem as TabItem;
            FavouritesPage page = new FavouritesPage(mw, f);
            g = page.favorites;
            page.Content = null;
            tab.Content = g;
            mw.tabDynamic.SelectedItem = tab;
        }
    }
}
