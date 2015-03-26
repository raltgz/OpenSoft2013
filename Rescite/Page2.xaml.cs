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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        MainWindow mw;
        TabPage page;
        Paper paper;
        public String citationUrl;
        public Uri linkUrl;
        Favourites fav;


        public Page2(MainWindow mw, TabPage page, Paper paper, Favourites fav)
        {
            InitializeComponent();
            this.fav = fav;
            this.mw = mw;
            this.page = page;
            this.paper = paper;
        }

        private void btnViewCitations(object sender, RoutedEventArgs e)
        {
            //add new tab
            mw.tabDynamic.DataContext = null;

            TabItem newTab = mw.AddTabItem();

            // bind tab control
            mw.tabDynamic.DataContext = mw._tabItems;

            // select newly added tab item
            mw.tabDynamic.SelectedItem = newTab;
            TabPage tp = new TabPage(mw, fav);
            Console.WriteLine(paper.getcitationsUrl());
            tp.displayCitations(SortOrder.DATE, paper.getid());

            ScrollViewer g = new ScrollViewer();
            g = tp.MainPane;
            tp.Content = null;
            newTab.Content = g;

        }



        public void setTitle(String title)
        {
            resultTitle.Text = title;
        }

        public void setAuthors(String authors)
        {
            if (authors.Length > 100)
            {
                authors = authors.Substring(0, 100) + "...";
            }
            resultAuthors.Text = authors;
        }


        public void setCitationList(String url)
        {
            citationUrl = url;
        }


        public void setNumberOfCitations(int numberOfCitations)
        {

            resultCitedBy.Text = "Cited By: " + numberOfCitations;
            if (numberOfCitations == 0)
            {
                btnViewCites.Opacity = 0;
                btnViewCites.IsEnabled = false;
                btnViewCites.Content = null;
            }
            if (numberOfCitations != 0)
                resultCitedBy.Text = "Cited By " + numberOfCitations;
            else
                resultCitedBy.Text = "No Citations";

        }

        public void setDescription(String des)
        {

        }

        public void setlinkUrl(Uri url)
        {
            this.linkUrl = url;
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
            //page.link.IsEnabled = true;
            //page.link.Visibility = Visibility.Visible;
            Page4 pageDesc = new Page4(mw);
            pageDesc.Title.Text = this.paper.gettitle();
            if (this.paper.getdescription() != "")
                pageDesc.Description.Text = this.paper.getdescription();
            else
                pageDesc.Description.Text = "-- No Abstract Available --";

            pageDesc.Authors.Text = this.paper.getauthors();
            if (this.paper.geturl() == null)
            {
                Console.WriteLine("hehre");
                pageDesc.Url.Text = "No Link Available";
            }
            else
                pageDesc.Url.Text = this.paper.geturl().ToString();
            pageDesc.setLink(this.linkUrl);
            Uri url = new Uri("http://g");

            if (this.paper.getConferenceUrl() != null)
            {
                url = null;
                url = new Uri(this.paper.getConferenceUrl());
            }
            pageDesc.setYear(this.paper.getYear());

            ScrollViewer sc = new ScrollViewer();
            sc = pageDesc.page_desc;
            pageDesc.Content = null;

            page.Paper.Children.Clear();
            page.Paper.Children.Add(sc);

            page.setLink(linkUrl);
        }

    }

}

