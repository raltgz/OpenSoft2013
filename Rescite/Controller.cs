using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace Citeseer
{
    /**
     * Controller will be instantiated by the tab and contains a reference to it
     * tab will call functions like initiateSearch() getFavorites()
     * these are all void functions, no data will be returned,
     * instead the controller will call functions of the tab to update results
     */
    class Controller
    {
        TabPage tab;
        //MainWindow mw;

        public Controller(TabPage tab)
        {
            this.tab = tab;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public ResultList initiateSearch(data d)
        {
            String queryString = d.qs;
                       //tab.getSearchBoxText();
            if (d.b.CancellationPending)
            {
                return null;
            }
            if (queryString == null || queryString == "")
                   {
                       TabPage.displayError("Please Enter a Query");
                       return null;
                   }
                   
                   d.b.ReportProgress(0, null);
                   int ylo = d.yl;
                   int yhi = d.yh;  //tab.getYhi();
                   Boolean includeCitations = d.ic;  // tab.getIncludePatents();
                   SortOrder sortOrder = d.so;  //tab.getSortOrder();
                   ResultType resultType = d.rt; // tab.getResultType();

                   d.b.ReportProgress(10, null);
                   Query query = new Query();
                   query.setqueryString(queryString);
                   if (ylo != 0)
                       query.setylo(ylo);
                   if (yhi != 0)
                       query.setyhi(yhi);
                   query.setincludePatents(includeCitations);
                   query.sortOrder = sortOrder;
                   query.resultType = resultType;
                   query.setpageid(d.pageId);

                   if (d.b.CancellationPending)
                   {
                       return null;
                   }

                   //bool networkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                   bool networkUp = Controller.CheckForInternetConnection();
                   Console.WriteLine(networkUp);
                   if (networkUp)
                   {
                       Searcher searcher = new Searcher();
                       ResultList resultListGS = new ResultList();

                       ResultList resultListMS = new ResultList();

                       ResultList resultList = new ResultList();


                       SearchType type;
                       d.b.ReportProgress(20, null);
                       resultListGS = searcher.GSsearch(query);
                       if (d.b.CancellationPending)
                       {
                           return null;
                       }
                       d.b.ReportProgress(50, null);
                       resultListMS = searcher.MSASsearch(query);
                       if (d.b.CancellationPending)
                       {
                           return null;
                       }
                       d.b.ReportProgress(70, null);

                       if (resultListMS != null)
                           resultList = resultListMS;
                       else if (resultListMS == null && resultListGS != null)
                           resultList = resultListGS;
                       else
                       {
                           TabPage.displayError("Cannot connect to Data Sources.");
                           return null;
                       }
                       if (d.b.CancellationPending)
                       {
                           return null;
                       }

                       d.b.ReportProgress(85, null);

                       if (resultListGS != null)
                       {
                           if (resultListGS.type != null)
                           resultList.type = resultListGS.type;
                           
                       }
                       else
                       {
                           MSASQueryURLBuilder MSQ = new MSASQueryURLBuilder();
                           String URL = "";
                           if (resultType == ResultType.AUTHOR)
                           {
                               URL = MSQ.buildAuthorUrl(queryString);
                           }
                           else if (resultType == ResultType.JOURNAL)
                           {
                               URL = MSQ.buildJournalUrl(queryString);
                           }
                           
                           Uri url = new Uri(URL);
                           Console.WriteLine(url.ToString());
                           MSASParser parser = new MSASParser();
                           if (resultType == ResultType.AUTHOR)
                           {
                               resultList.resultType = ResultType.AUTHOR;
                               resultList.type = parser.MSParseProfile(url, queryString, resultType);
                           }
                           else
                           {
                               resultList.resultType = ResultType.JOURNAL;
                               resultList.loadType();
                               Console.WriteLine("Reached");
                               resultList.type =(Journal) parser.MSParseJournal(queryString);
                               Console.WriteLine(";;;;;"+resultList.type);
                           }
                           if (d.b.CancellationPending)
                           {
                               return null;
                           }

                       }

                       //      tab.setResult(resultList);    

                       if (d.b.CancellationPending)
                       {
                           
                           return null;
                       }

                       d.b.ReportProgress(100, null);
                       return resultList;
                   }

                   else
                   {
                       TabPage.displayError("Oops !! No Internet Connection !!!");

                       return null;

                   }
        }

        public ResultList getGSCitations(String url)
        {
            Searcher searcher = new Searcher();
            ResultList resultList = new ResultList();

            resultList = searcher.GSsearchCitationUrl(url);

            return resultList;
            //tab.setResult(resultList);
        }

        public ResultList getMSASCitations(soid s)
       {
           Searcher searcher = new Searcher();
           ResultList resultList = new ResultList();

           String URL;
           s.c.ReportProgress(10, null);
           if (s.c.CancellationPending) return null;
           MSASQueryURLBuilder citationBuild = new MSASQueryURLBuilder();
           s.c.ReportProgress(40, null);
           URL = citationBuild.buildCitationUrl(s.i,s.sOrder,s.start);
           if (s.c.CancellationPending) return null;
           s.c.ReportProgress(60, null);
           resultList = searcher.MSASsearchCitationUrl(URL);
           s.c.ReportProgress(90, null);
           if (s.c.CancellationPending) return null;
           return resultList;
            //tab.setResult(resultList);


       }
        
    }
}
