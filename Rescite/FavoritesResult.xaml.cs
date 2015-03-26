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
    /// Interaction logic for FavoritesResult.xaml
    /// </summary>
    public partial class FavoritesResult : Page
    {
        MainWindow mw;
        Query q;
        Favourites f;
        FavouritesPage favPage;

        public FavoritesResult(MainWindow mw, Favourites f, FavouritesPage favPage)
        {
            InitializeComponent();
            this.mw = mw;
            this.f = f;
            this.favPage = favPage;
        }

        private void btnSearch(object sender, RoutedEventArgs e)
        {
            //add new tab
            mw.tabDynamic.DataContext = null;

            TabItem newTab = mw.AddTabItem();

            // bind tab control
            mw.tabDynamic.DataContext = mw._tabItems;

            // select newly added tab item
            mw.tabDynamic.SelectedItem = newTab;
            TabPage tp = new TabPage(mw, f, q);
            /*Console.WriteLine(paper.getcitationsUrl());
            tp.displayCitations(SortOrder.DATE,paper.getid());
            */
            ScrollViewer g = new ScrollViewer();
            g = tp.MainPane;
            tp.Content = null;
            newTab.Content = g;

        }



        public void setQueryString(String title)
        {
            if (title.Length == 0)
                queryString.Text = "Citations List";
            else
                queryString.Text = title;
        }

        public void setQueryParams(String authors)
        {
            queryParams.Text = authors;
        }


        public void setSearch(Query q)
        {
            this.q = q ;
        }


        public void setSearchType(String type)
        {
            queryType.Text = type;
        }
        


        private void addShade(object sender, MouseEventArgs e)
        {

            this.Background = Brushes.LightGray;

            this.listItem.Background = Brushes.LightGray;

        }

        private void removeShade(object sender, MouseEventArgs e)
        {
            this.listItem.Background = Brushes.Transparent;
        }

        private void listItem_Selected(object sender, RoutedEventArgs e)
        {
        }

        private void delete(object sender, RoutedEventArgs e)
        {
            f.deleteFavourite(this.q);
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
