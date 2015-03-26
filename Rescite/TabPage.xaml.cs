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
using System.ComponentModel;
using System.Timers;
namespace Citeseer
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    struct data
    {
        public String qs;
        public int yl;
        public int yh;
        public ResultType rt;
        public SortOrder so;
        public Boolean ic;
        public int pageId;
        public BackgroundWorker b;
    }
    struct soid
    {
        public BackgroundWorker c;
        public SortOrder sOrder;
        public int i;
        public int start;
    }
    public partial class TabPage : Page
    {
        public int flag = 0;
        data d;
        Favourites fav;
        List<Controller> controllerList;
        Controller test;    //for testing purposes
        SearchType type;
        ResultList resultList;
        MainWindow mw;
        Uri linkUrl;
        Uri statsUrl;
        private System.ComponentModel.BackgroundWorker Searcher, viewCitation;
        // public event ProgressChangedEventHandler ProgressChanged;
        public String error;
        List<Button> pageList;
        private int currentPageSelected = 1;
        private int newResultFlag = 1;
        private int firstSearchFlag = 0;
        private Boolean isCitation = false;
        private int maxCount;
        soid s;

        // related to statusbar
        public delegate void UpdateStatusBarEventHandler(string message);
        public event UpdateStatusBarEventHandler UpdateStatusBar;
        private Favourites f;
        private Query q;
        private Timer aTimer;


        public TabPage(MainWindow mw, Favourites f)
        {
            this.mw = mw;
            error = "";
            fav = f;
            InitializeComponent();
            Page5 welcome = new Page5(mw);
            Grid g1 = new Grid();
            g1 = welcome.welcome;
            welcome.Content = null;
            this.ResultsPane.Items.Add(g1);
            this.Searcher = new System.ComponentModel.BackgroundWorker();
            this.Searcher.WorkerReportsProgress = true;
            this.Searcher.WorkerSupportsCancellation = true;


            this.Searcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Searcher_DoWork);
            this.Searcher.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Searcher_ProgressChanged);
            this.Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Searcher_RunWorkerCompleted);


            this.viewCitation = new System.ComponentModel.BackgroundWorker();
            this.viewCitation.WorkerReportsProgress = true;
            this.viewCitation.WorkerSupportsCancellation = true;
            this.viewCitation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Citation_DoWork);
            this.viewCitation.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Citation_ProgressChanged);
            this.viewCitation.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Citation_RunWorkerCompleted);
            controllerList = new List<Controller>();

            progressBar.Visibility = System.Windows.Visibility.Hidden;
            this.searchBox.Opacity = 1;


            this.UpdateStatusBar += updateStatusBar;  // related to statusbar

            updateStatusBar("Ready To Search...");

            
            /*
             * Initially two tabs are created:
             * One for the search
             * Other for adding a new tab
           */

            //Dummy to test
            /*pageList = new List<Button>();
            Button b;
            for (int i = 0; i < 12; i++)
            {
                b = new Button();
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.Background = new SolidColorBrush(Colors.White);
                b.Foreground = new SolidColorBrush(Colors.Blue);
                b.BorderBrush = Brushes.White;
                b.BorderThickness = new Thickness(0);
                b.Cursor = Cursors.Hand;
                b.Uid = i + "";
                if (i == 0)
                    b.Content = b.Uid = "previous";
                else if (i == 11)
                    b.Content = b.Uid = "next";
                else
                    b.Content = (i) + "";
                if (i == 1)
                {
                    b.Foreground = new SolidColorBrush(Colors.Black);
                    b.Background = Brushes.LightGray;
                }
                else
                {
                    b.Background = Brushes.White;
                    b.Foreground = new SolidColorBrush(Colors.Blue);
                }
                b.Click += b_Click;
                pageList.Add(b);
                Pagination.Children.Add(pageList[i]);
            }*/
            //Dummy to test
        }

        public TabPage(MainWindow mw, Favourites f, Query q)
        {
            this.mw = mw;
            this.f = f;
            this.q = q;
            InitializeComponent();

            this.Searcher = new System.ComponentModel.BackgroundWorker();
            this.Searcher.WorkerReportsProgress = true;
            this.Searcher.WorkerSupportsCancellation = true;


            this.Searcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Searcher_DoWork);
            this.Searcher.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Searcher_ProgressChanged);
            this.Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Searcher_RunWorkerCompleted);


            this.viewCitation = new System.ComponentModel.BackgroundWorker();
            this.viewCitation.WorkerReportsProgress = true;
            this.viewCitation.WorkerSupportsCancellation = true;
            this.viewCitation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Citation_DoWork);
            this.viewCitation.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Citation_ProgressChanged);
            this.viewCitation.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Citation_RunWorkerCompleted);
            controllerList = new List<Controller>();

            progressBar.Visibility = System.Windows.Visibility.Hidden;
            this.searchBox.Opacity = 1;


            this.UpdateStatusBar += updateStatusBar;  // related to statusbar

            updateStatusBar("Ready To Search !!!");

            this.searchBox.Text = q.getqueryString();
            if (q.resultType == ResultType.AUTHOR)
            {
                this.radioAuthor.IsChecked = true;
                this.radioJournal.IsChecked = false;
            }
            else
            {
                this.radioJournal.IsChecked = true;
                this.radioAuthor.IsChecked = false;
            }
            if (q.sortOrder == SortOrder.CITATIONS)
            {
                this.radioCitations.IsChecked = true;
                this.radioDate.IsChecked = false;
            }
            else
            {
                this.radioDate.IsChecked = true;
                this.radioCitations.IsChecked = false;
            }
            int flag = 0;
            if (q.issetyhi())
            {
                flag = 1;
                this.checkBoxCustomRange.IsChecked = true;
                this.checkBoxCustomRange.IsEnabled = true;
                this.yearEnd.IsEnabled = true;
                this.yearStart.IsEnabled = true;
                this.yearEnd.Text = q.getyhi() + "";
            }
            if (q.issetylo())
            {
                flag = 1;
                this.checkBoxCustomRange.IsChecked = true;
                this.checkBoxCustomRange.IsEnabled = true;
                this.yearEnd.IsEnabled = true;
                this.yearStart.IsEnabled = true;
                this.yearStart.Text = q.getylo() + "";
            }
            if (flag == 0)
                this.checkBoxCustomRange.IsChecked = false;
            currentPageSelected = q.getpageid();
            searchFav();

        }

        // Update the message passed in this function in statusbar
        public void updateStatusBar(String message)
        {
            textbox.Text = message;
        }

        //Returns the text in the search bar
        public String getSearchString()
        {
            return null;
        }
        public static void displayError(String e)
        {
            MessageBox.Show(e);

        }

        private void Searcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            aTimer.Enabled = false;
            aTimer.Stop();
            if (e.Cancelled)
            {

                if (flag == 2)
                {/*
                    Searcher.Dispose();
                    displayError("Loading Please Wait !!!");
                    Searcher = new System.ComponentModel.BackgroundWorker();
                    Searcher.WorkerReportsProgress = true;
                    Searcher.WorkerSupportsCancellation = true;
                   */
                    updateStatusBar(" ");
                    flag = 1;
                    this.Author.Children.Clear();
                    this.Paper.Children.Clear();
                    this.Journal.Children.Clear();

                    progressBar.Value = 0;

                    TabItem selectedTab = mw.tabDynamic.SelectedItem as TabItem;
                    if (getSearchBoxText() != "")
                        selectedTab.Header = getSearchBoxText();


                    d.qs = this.getSearchBoxText(); Console.WriteLine(d.qs);
                    d.yl = this.getYlo(); Console.WriteLine(d.yl);
                    d.yh = this.getYhi(); Console.WriteLine(d.yh);
                    d.ic = this.getIncludePatents(); Console.WriteLine(d.ic);
                    d.so = this.getSortOrder(); Console.WriteLine(d.so);
                    d.rt = this.getResultType(); Console.WriteLine(d.rt);
                    d.b = Searcher;
                    d.pageId = currentPageSelected;
                    test = new Controller(this);
                    updateStatusBar("Looking up for " + d.qs+"...");
                    Searcher.RunWorkerAsync(d);
                }
                else
                {
                    updateStatusBar(" ");
                    flag = 0; progressBar.Value = 0;
                    cancel.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                updateStatusBar(" ");
                if (resultList != null)
                {
                    updateStatusBar("Fetched Results.");
                    Console.WriteLine(this.resultList.getCount());
                    this.setResult(resultList); // searchCompleted(e);
                }
                else
                {
                    //TabPage.displayError("No Results found !!");
                    updateStatusBar("No Results Found.Please check your Search Query.");
                    NoResults welcome = new NoResults();
                    Grid g1 = new Grid();
                    g1 = welcome.grid;
                    welcome.Content = null;
                    this.ResultsPane.Items.Add(g1);
                    Console.WriteLine("it's null so sad"); progressBar.Value = 0; cancel.Visibility = Visibility.Hidden;
                    progressBar.Visibility = Visibility.Hidden;
                    searchBox.Opacity = 1;
                }
                flag = 0;
            }
        }

        private void Searcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (ProgressChanged != null)
            progressBar.Visibility = System.Windows.Visibility.Visible;
            progressBar.Value = e.ProgressPercentage;

            if (progressBar.Value >= 100)
            {
                progressBar.Visibility = System.Windows.Visibility.Hidden;
                this.searchBox.Opacity = 1;
                this.cancel.Visibility = System.Windows.Visibility.Hidden;
            }


        }

        private void Citation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (ProgressChanged != null)
            progressBar.Visibility = System.Windows.Visibility.Visible;
            progressBar.Value = e.ProgressPercentage;

            if (progressBar.Value >= 100)
            {
                progressBar.Visibility = System.Windows.Visibility.Hidden;
                this.searchBox.Opacity = 1;
                this.cancel.Visibility = System.Windows.Visibility.Hidden;
            }


        }
        private void Citation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ;
            }
            else
            {
                if (resultList == null) { Console.WriteLine("it's null so sad"); }
                this.setResult(resultList); // searchCompleted(e);
            }
        }

        private void Searcher_DoWork(object sender, DoWorkEventArgs e)
        {

            Console.WriteLine("Thread started");
            data di = (data)e.Argument;
            BackgroundWorker b = sender as BackgroundWorker;
            //updateStatusBar("Looking up for");
            test = new Controller(this);

            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            // Set the Interval to 1 minute.
            aTimer.Interval = 140000;
            aTimer.Enabled = true;
            //updateStatusBar(" ");

            resultList = test.initiateSearch(di);
            //updateStatusBar("fetched results for "+di.qs);
            if (d.b.CancellationPending) e.Cancel = true;
            Console.WriteLine("why dis kolaveri");
            //TabPage tab = new TabPage(mw);

        }

        private void Citation_DoWork(object sender, DoWorkEventArgs e)
        {

            Console.WriteLine("Thread started");
            /* data d = (data)e.Argument;
             BackgroundWorker b = sender as BackgroundWorker;
             test = new Controller(this);


             resultList = test.initiateSearch(d);

             Console.WriteLine("why dis kolaveri");
             //TabPage tab = new TabPage(mw);
             */
            soid s = (soid)e.Argument;

            int id = s.i;
            SortOrder so = s.sOrder;
            int start = s.start;

            resultList = test.getMSASCitations(s);

            if (s.c.CancellationPending) e.Cancel = true;

        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
      
            Searcher.CancelAsync();
            aTimer.Enabled = false;
            aTimer.Stop();
            TabPage.displayError("Connection Timed Out. Please Check your Internet Connection.");
            
            

        }

        private void searchFav()
        {
            updateStatusBar(" ");
            this.searchBox.Opacity = 0.50;
            this.cancel.Visibility = System.Windows.Visibility.Visible;
            progressBar.Visibility = System.Windows.Visibility.Visible;
            this.Author.Children.Clear();
            this.Paper.Children.Clear();
            this.Journal.Children.Clear();
            newResultFlag = 0;
            btnAddFav.Background = new SolidColorBrush(Colors.White);
            btnAddFav.Foreground = new SolidColorBrush(Colors.Black);
            btnAddFav.Content = "+ Add to Favorites";

            pageList = new List<Button>();
            Button b;
            //Console.WriteLine(resultList.getCount());
            //maxCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(resultList.getCount() / 20)));

            //updateStatusBar(maxCount.ToString());


            //Console.WriteLine(maxCount);
            //status.Text = "" + maxCount;
            for (int i = 0; i < 12; i++)
            {
                b = new Button();
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.Background = new SolidColorBrush(Colors.White);
                b.Foreground = new SolidColorBrush(Colors.Blue);
                b.BorderBrush = Brushes.White;
                b.MaxWidth = 20;
                b.BorderThickness = new Thickness(0);
                b.Cursor = Cursors.Hand;
                b.Uid = i + "";
                if (i == 0)
                    b.Content = b.Uid = "<<";
                else if (i == 11)
                    b.Content = b.Uid = ">>";
                else
                    b.Content = (i) + "";
                if (i == 1)
                {
                    b.Foreground = new SolidColorBrush(Colors.Black);
                    b.Background = Brushes.LightGray;
                }
                else
                {
                    b.Background = Brushes.White;
                    b.Foreground = new SolidColorBrush(Colors.Blue);
                }
                b.Click += b_Click;
                /*
                if (i > maxCount && i != 11)
                {

                    b.IsEnabled = false;
                    b.Visibility = Visibility.Hidden;
                }*/
                pageList.Add(b);
                if (currentPageSelected == 1)
                {
                    pageList[0].Visibility = Visibility.Hidden;
                    pageList[0].IsEnabled = false;
                }
                else
                {
                    pageList[0].IsEnabled = true;
                    pageList[0].Visibility = Visibility.Visible;
                }
                //Pagination.Children.Add(pageList[i]);
            }
            try
            {
                changePagination(currentPageSelected);
            }
            catch (Exception)
            { 
            }
            
            if (this.flag == 0)
            {
                flag = 1;
                updateStatusBar("Looking up for " + this.getSearchBoxText()+" ...");
                TabItem selectedTab = mw.tabDynamic.SelectedItem as TabItem;
                if (getSearchBoxText() != "")
                    selectedTab.Header = getSearchBoxText();


                d.qs = this.getSearchBoxText(); Console.WriteLine(d.qs);
                d.yl = this.getYlo(); Console.WriteLine(d.yl);
                d.yh = this.getYhi(); Console.WriteLine(d.yh);
                d.ic = this.getIncludePatents(); Console.WriteLine(d.ic);
                d.so = this.getSortOrder(); Console.WriteLine(d.so);
                d.rt = this.getResultType(); Console.WriteLine(d.rt);
                d.b = Searcher;
                d.pageId = currentPageSelected;

                test = new Controller(this);

                Searcher.RunWorkerAsync(d);

            }
            else
            {
                flag = 2;
                Searcher.CancelAsync();
            }
            /*  try
          {
              test.initiateSearch();
          }
          catch (Exception)
          {
              MessageBox.Show(error);
          }
                  * */
        }

        private void search(object sender = null, RoutedEventArgs e = null)
        {

            this.ResultsPane.Items.Clear();
            Pagination.Visibility = Visibility.Hidden;
            Pagination.IsEnabled = false;
            updateStatusBar(" ");

            this.searchBox.Opacity = 0.50;
            this.cancel.Visibility = System.Windows.Visibility.Visible;
            progressBar.Visibility = System.Windows.Visibility.Visible;
            this.Author.Children.Clear();
            this.Paper.Children.Clear();
            this.Journal.Children.Clear();
            newResultFlag = 1;
           
            btnAddFav.Background = new SolidColorBrush(Colors.White);
            btnAddFav.Foreground = new SolidColorBrush(Colors.Black);
            btnAddFav.Content = "+ Add to Favorites";
            currentPageSelected = 1;
            if (firstSearchFlag != 0)
            {
                try
                {
                    changePagination(currentPageSelected);
                }
                catch (Exception)
                {
                }
            }
            if (this.flag == 0)
            {
                flag = 1;
                updateStatusBar("Looking up for " + this.getSearchBoxText()+"...");
                TabItem selectedTab = mw.tabDynamic.SelectedItem as TabItem;
                if (getSearchBoxText() != "")
                    selectedTab.Header = getSearchBoxText();


                d.qs = this.getSearchBoxText(); Console.WriteLine(d.qs);
                d.yl = this.getYlo(); Console.WriteLine(d.yl);
                d.yh = this.getYhi(); Console.WriteLine(d.yh);
                d.ic = this.getIncludePatents(); Console.WriteLine(d.ic);
                d.so = this.getSortOrder(); Console.WriteLine(d.so);
                d.rt = this.getResultType(); Console.WriteLine(d.rt);
                d.b = Searcher;
                d.pageId = currentPageSelected;

                test = new Controller(this);

                Searcher.RunWorkerAsync(d);

            }
            else
            {
                flag = 2;
                Searcher.CancelAsync();
            }
            /*  try
          {
              test.initiateSearch();
          }
          catch (Exception)
          {
              MessageBox.Show(error);
          }
                  * */
        }

        public void displayCitations(SortOrder so, int id)
        {
            test = new Controller(this);
            TabItem selectedTab = mw.tabDynamic.SelectedItem as TabItem;
            selectedTab.Header = "Citations List";
            //test.getGSCitations(url);
            this.isCitation = true;


            s.sOrder = this.getSortOrder();
            s.i = id;
            s.start = currentPageSelected * 10;
            s.c = viewCitation;
            if (viewCitation.IsBusy)
            {
                viewCitation.CancelAsync();
            }
            viewCitation.RunWorkerAsync(s);
        }

        private void checkBoxCustomRange_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxCustomRange.IsChecked == true)
            {
                yearStart.IsEnabled = true;
                yearEnd.IsEnabled = true;
            }
            else
            {
                yearStart.IsEnabled = false;
                yearEnd.IsEnabled = false;
            }
        }

        public String getSearchBoxText()
        {
            return searchBox.Text;
        }

        public int getYlo()
        {
            if (checkBoxCustomRange.IsChecked == true)
            {
                try
                {
                    return Convert.ToInt32(yearStart.Text);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 0;
        }

        public int getYhi()
        {
            if (checkBoxCustomRange.IsChecked == true)
            {
                try
                {
                    return Convert.ToInt32(yearEnd.Text);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 0;
        }

        public Boolean getIncludePatents()
        {
            return true;
        }

        public SortOrder getSortOrder()
        {
            if (radioDate.IsChecked == true)
                return SortOrder.DATE;
            else if (radioCitations.IsChecked == true)
                return SortOrder.CITATIONS;
            return SortOrder.DATE;
        }

        public ResultType getResultType()
        {
            if (radioAuthor.IsChecked == true)
                return ResultType.AUTHOR;
            else if (radioJournal.IsChecked == true)
                return ResultType.JOURNAL;
            return ResultType.AUTHOR;
        }

        public void setResult(ResultList resultList)
        {
            this.ResultsPane.Items.Clear();

            firstSearchFlag = 1;
            if (newResultFlag == 1 && resultList != null)
            {
                btnAddFav.IsEnabled = true;
                btnAddFav.Visibility = Visibility.Visible;
                sep1.Visibility = Visibility.Visible;
                sep2.Visibility = Visibility.Visible;
                btnExport.IsEnabled = true;
                btnExport.Visibility = Visibility.Visible;
                newResultFlag = 0;
                pageList = new List<Button>();
                Button b;
                Console.WriteLine(resultList.getCount());
                maxCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(resultList.getCount() / 20)));

                //updateStatusBar(maxCount.ToString());


                Console.WriteLine(maxCount);
                //status.Text = "" + maxCount;
                for (int i = 0; i < 12; i++)
                {
                    b = new Button();
                    b.HorizontalAlignment = HorizontalAlignment.Stretch;
                    b.Background = new SolidColorBrush(Colors.White);
                    b.Foreground = new SolidColorBrush(Colors.Blue);
                    b.BorderBrush = Brushes.White;
                    b.MaxWidth = 20;
                    b.BorderThickness = new Thickness(0);
                    b.Cursor = Cursors.Hand;
                    b.Uid = i + "";
                    if (i == 0)
                        b.Content = b.Uid = "<<";
                    else if (i == 11)
                        b.Content = b.Uid = ">>";
                    else
                        b.Content = (i) + "";
                    if (i == 1)
                    {
                        b.Foreground = new SolidColorBrush(Colors.Black);
                        b.Background = Brushes.LightGray;
                    }
                    else
                    {
                        b.Background = Brushes.White;
                        b.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                    b.Click += b_Click;
                    if (i > maxCount && i != 11)
                    {

                        b.IsEnabled = false;
                        b.Visibility = Visibility.Hidden;
                    }
                    pageList.Add(b);
                    if (currentPageSelected == 1)
                    {
                        pageList[0].Visibility = Visibility.Hidden;
                        pageList[0].IsEnabled = false;
                    }
                    else
                    {
                        pageList[0].IsEnabled = true;
                        pageList[0].Visibility = Visibility.Visible;
                    }
                    Pagination.Children.Add(pageList[i]);
                }
            }
            try
            {
                changePagination(currentPageSelected);
            }
            catch (Exception)
            { 
            }

            if (resultList != null)
            {

                Pagination.Visibility = Visibility.Visible;
                Pagination.IsEnabled = true;
                foreach (Result r in resultList)
                {
                    List<Paper> paperList = new List<Paper>();
                    paperList = r.getResults();
                    Page2 page;
                    ListBoxItem item;

                    foreach (Paper p in paperList)
                    {
                        page = new Page2(mw, this, p, fav);
                        item = new ListBoxItem();
                        page.setAuthors(p.getauthors() + p.getYear());
                        page.setTitle(p.gettitle());
                        page.setDescription(p.getdescription());
                        page.setNumberOfCitations(p.getnumCitations());
                        page.setlinkUrl(p.geturl());
                        page.setCitationList(p.getcitationsUrl());


                        item = page.listItem;

                        page.Content = null;
                        this.ResultsPane.Items.Add(item);
                    }

                }


                type = resultList.type;
                ResultType resultType = resultList.resultType;
                if ((type != null && type.getname() != null) && resultType == ResultType.AUTHOR)
                {
                    //Console.WriteLine("Ohhhhhhhhhhhhhhhhh I didnt expect");
                    Author auth = new Author();
                    auth = (Author)type;
                    this.statsUrl = auth.getstatsGraphUrl();
                    Page3 auth_profile = new Page3(this);
                    auth_profile.auth_name.Text = type.getname();
                    auth_profile.num_cite.Text = type.getnumPapers().ToString();
                    auth_profile.hindex.Text = type.gethIndex().ToString();
                    auth_profile.iindex.Text = type.getiIndex().ToString();
                    Grid g = new Grid();
                    g = auth_profile.author_profile;
                    auth_profile.Content = null;
                    this.Author.Children.Clear();
                    this.Author.Children.Add(g);

                }
                else if (type != null && resultList.resultType == ResultType.JOURNAL)
                {
                    //this.Author.Text = "";
                    type = new Journal();
                    type = (Journal)resultList.type;
                    JournalPage auth_profile = new JournalPage(this);
                    auth_profile.journal_name.Text = this.searchBox.Text;
                    Journal j = (Journal)type;
                    Grid g = new Grid();
                    g = auth_profile.author_profile;
                    auth_profile.Content = null;
                    this.Author.Children.Clear();
                    this.Author.Children.Add(g);
                    this.statsUrl = j.getStatsUrl();
                    //  Console.WriteLine("==========="+j.getStatsUrl().ToString());
                }
            }

        }

        void b_Click(object sender, RoutedEventArgs e)
        {
            btnAddFav.Background = new SolidColorBrush(Colors.White);
            btnAddFav.Foreground = new SolidColorBrush(Colors.Black);
            btnAddFav.Content = "+ Add to Favorites";
            Button b = (Button)sender;
            String id = b.Uid;
            if (id.Equals("<<"))
            {
                if (currentPageSelected != 1)
                {
                    currentPageSelected--;
                    //start query for currentPageSelected
                }
            }
            else if (id.Equals(">>"))
            {
                currentPageSelected++;
                //start query for currentPageSelected
            }
            else
            {
                currentPageSelected = Convert.ToInt32(id);
                //start query for currentPageSelected
            }
            if (searchPaginate())
            {
                //searchBox.Text = currentPageSelected + "";
                try
                {
                    changePagination(currentPageSelected);
                }
                catch (Exception)
                {
                }
            }
        }

        private Boolean searchPaginate()
        {
            if (isCitation == false)
            {
                flag = 2;
                Searcher.CancelAsync();
                if (Searcher.IsBusy == false)
                {
                    this.cancel.Visibility = System.Windows.Visibility.Visible;
                    progressBar.Visibility = System.Windows.Visibility.Visible;
                    this.searchBox.Opacity = 0.50;
                    d.pageId = currentPageSelected;
                    updateStatusBar("Fetching More Results...");
                    test = new Controller(this);
                    Searcher.RunWorkerAsync(d);
                    return true;
                }
            }
            else if (isCitation)
            {
                this.cancel.Visibility = System.Windows.Visibility.Visible;
                progressBar.Visibility = System.Windows.Visibility.Visible;
                this.searchBox.Opacity = 0.50;
                s.sOrder = this.getSortOrder();
                updateStatusBar("Fetching Citations...");
                displayCitations(s.sOrder, s.i);
                return true;
            }
            return false;

        }

        private void changePagination(int currentPageSelected)
        {
            Pagination.Children.Clear();
            int k = 1;
            // status.Text = "" + maxCount;
            if (currentPageSelected <= 5)
            {
                k = 1;
                for (int i = 1; i <= 10; i++)
                {
                    pageList[k].Content = i;
                    pageList[k].Uid = i + "";
                    if (i == currentPageSelected)
                    {
                        pageList[k].Foreground = new SolidColorBrush(Colors.Black);
                        pageList[k].Background = Brushes.LightGray;
                    }
                    else
                    {
                        pageList[k].Foreground = new SolidColorBrush(Colors.Blue);
                        pageList[k].Background = Brushes.White;
                    }
                    if (i > maxCount)
                    {
                        pageList[k].IsEnabled = false;
                        pageList[k].Visibility = Visibility.Hidden;
                    }
                    if (currentPageSelected == maxCount)
                    {
                        pageList[11].IsEnabled = false;
                        pageList[11].Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        pageList[11].IsEnabled = true;
                        pageList[11].Visibility = Visibility.Visible;
                    }
                    k++;
                }
            }
            else
            {
                k = 1;
                for (int i = currentPageSelected - 4; i <= currentPageSelected + 5; i++)
                {

                    pageList[k].Content = i;
                    pageList[k].Uid = i + "";
                    if (i == currentPageSelected)
                    {
                        pageList[k].Foreground = new SolidColorBrush(Colors.Black);
                        pageList[k].Background = Brushes.LightGray;
                    }
                    else
                    {
                        pageList[k].Foreground = new SolidColorBrush(Colors.Blue);
                        pageList[k].Background = Brushes.White;
                    }
                    if (i > maxCount)
                    {
                        pageList[k].IsEnabled = false;
                        pageList[k].Visibility = Visibility.Hidden;
                    }
                    if (currentPageSelected == maxCount)
                    {
                        pageList[11].IsEnabled = false;
                        pageList[11].Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        pageList[11].IsEnabled = true;
                        pageList[11].Visibility = Visibility.Visible;
                    }
                    k++;
                }
            }
            if (currentPageSelected == 1)
            {
                pageList[0].IsEnabled = false;
                pageList[0].Visibility = Visibility.Hidden;
            }
            else
            {
                pageList[0].Visibility = Visibility.Visible;
                pageList[0].IsEnabled = true;
            }
            for (int i = 0; i < 12; i++)
                Pagination.Children.Add(pageList[i]);
        }

        public void setLink(Uri url)
        {
            this.linkUrl = url;
        }



        private void onKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                search(sender, e);
            }
        }

        private void linkClick(object sender, RoutedEventArgs e)
        {
            mw.AddTabWebView(this.linkUrl);
        }

        public void showJourStats()
        {
            if (resultList.resultType == ResultType.JOURNAL)
            {
                Journal j = (Journal)resultList.type;
                Uri url = new Uri("http://google.com");
                if (j.getStatsUrl() != null)
                    url = j.getStatsUrl();
                mw.AddTabWebView(url);
            }
        }

        public void showStats()
        {
            AuthorProfile auth = new AuthorProfile();
            Author aut = (Author)this.type;
            auth.setInstitute(aut.getinstitute());
            auth.setName(type.getname());
            auth.setCitations(type.getnumPapers());
            auth.setHIndex(type.gethIndex());
            auth.setTotal(resultList.getCount().ToString());
            auth.setCitesPaper(String.Format("{0:F3}",(((double)type.getnumPapers() / resultList.getCount()))));
            auth.setIIndex(type.getiIndex());
            auth.setInterests(System.Net.WebUtility.HtmlDecode(aut.getresArea()));
            auth.setPicture(this.statsUrl);

            mw.tabDynamic.DataContext = null;
            TabItem tab = mw.AddTabProfilePage(auth);
            mw.tabDynamic.DataContext = mw._tabItems;

            // select newly added tab item
            mw.tabDynamic.SelectedItem = tab;
        }

        private void keyFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(searchBox);
        }

        private void ProgressBar_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void cancelSearch(object sender, RoutedEventArgs e)
        {

            Searcher.CancelAsync();
            this.cancel.Visibility = System.Windows.Visibility.Hidden;
            progressBar.Visibility = System.Windows.Visibility.Hidden;
            this.searchBox.Opacity = 1;
        }

        private void addFav(object sender, RoutedEventArgs e)
        {
            btnAddFav.Background = new SolidColorBrush(Color.FromRgb(71, 135, 237));
            btnAddFav.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            btnAddFav.BorderThickness = new Thickness(1);
            btnAddFav.Content = "Favourite";
            btnAddFav.Foreground = new SolidColorBrush(Colors.White);
            Query q = new Query();
            q.setqueryString(this.getSearchBoxText());
            q.setyhi(this.getYhi());
            q.setylo(this.getYlo());
            if (this.getSortOrder() == SortOrder.DATE)
                q.sortOrder = SortOrder.DATE;
            else
                q.sortOrder = SortOrder.CITATIONS;
            if (this.getResultType() == ResultType.AUTHOR)
                q.resultType = ResultType.AUTHOR;
            else
                q.resultType = ResultType.JOURNAL;
            q.setpageid(currentPageSelected);
            fav.addFavourite(q);
        }

        private void export(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.SaveFileDialog();
            fileDialog.Title = "Export Results file";
            fileDialog.FileName = "Export";
            fileDialog.Filter = "Tab separated values|*.tsv";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && fileDialog.FileName != "")
            {
                if (Exporter.exportResults(this.resultList, fileDialog.FileName))
                {
                    TabPage.displayError("Export Success !!");
                    updateStatusBar("Export Success !!!");
                }
                else
                {
                    TabPage.displayError("Export Failed !!");
                    updateStatusBar("Export Failed !!!");
                }
            }
        }

        private void showFav(object sender, RoutedEventArgs e)
        {
            mw.tabDynamic.DataContext = null;

            TabItem newTab = mw.AddTabFavorites();

            // bind tab control
            mw.tabDynamic.DataContext = mw._tabItems;

            // select newly added tab item
            mw.tabDynamic.SelectedItem = newTab;
        }

        private void help(object sender, RoutedEventArgs e)
        {
            Uri url = new Uri(System.IO.Path.GetFullPath("Help/rescite.html"), UriKind.RelativeOrAbsolute);
            mw.AddTabWebView(url, 1);
        }

        public void showJourProfile()
        {
            Journal j = new Journal();
            j = (Journal)resultList.type;
            mw.AddTabHtmlView(j);
        }
    }
}
