using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citeseer
{
    /**
     * Should have functionality to build a query URL for search using the Query object 
     */
    interface QueryURLBuilder
    {
        String buildQuery(Query query);
    }

    class GSQueryURLBuilder : QueryURLBuilder
    {
        public String buildQuery(Query query)
        {
            //normal search (sort by citations)
            // sort by citations not implemented
            String queryURL, queryURLStat1, queryURLStat2;
            queryURL = null;
            String queryPart1 = "http://scholar.google.co.in/scholar?as_q=&as_occt=any&";
            String searchString = null;
            searchString = query.getqueryString();

            searchString = searchString.Replace(" ", "+");

            if (query.resultType == ResultType.AUTHOR)
            {
                queryPart1 = queryPart1 + "as_sauthors=" + "\"" + searchString + "\"";
            }

            else if (query.resultType == ResultType.JOURNAL)
            {
                queryPart1 = queryPart1 + "as_publication=" + "\"" + searchString + "\"";
            }

            int patentChoice;
            String queryPart2 = "&hl=en&as_sdt=";
            if (!query.getincludePatents())
                patentChoice = 0;
            else
                patentChoice = 1;
            queryPart2 += patentChoice;
            queryPart2 += "%2C5&as_vis=1";

            //sort by citations
            if (query.sortOrder == SortOrder.CITATIONS && (query.issetyhi() && query.issetylo()))
            {
                queryURL = queryPart1 + queryPart2;
                queryURLStat1 = queryURL;
            }

            //sort by date
            else if (query.sortOrder == SortOrder.CITATIONS && !(query.issetyhi() && query.issetylo()))
            {
                int year = 2013;
                queryURL = queryPart1 + "&as_ylo=&as_yhi=" + queryPart2;
                year--;


            }

            //search for custom year range and sorted by citations
            else if ((query.issetyhi() && query.issetylo()) && query.sortOrder == SortOrder.CITATIONS)
            {

                queryURL = queryPart1 + "&as_ylo=" + query.getylo() + "&as_yhi=" + query.getyhi() + queryPart2;
                queryURLStat2 = queryURL;
            }

            //search for custome year range and sorted by date
            else if (query.sortOrder == SortOrder.DATE && (query.issetyhi() && query.issetylo()))
            {
                queryURL = queryPart1 + "&as_ylo=" + query.getylo() + "&as_yhi=" + query.getyhi() + queryPart2;
            }

            else if (query.sortOrder == SortOrder.DATE && !(query.issetyhi() && query.issetylo()))
            {
                queryURL = queryPart1 + "&as_ylo=&as_yhi=" + queryPart2;
            }

            //TODO : Need to get the correct URL.
            // #Srikar


            return queryURL;
        }
    }

    class MSASQueryURLBuilder : QueryURLBuilder
    {
        static String appId = "681cba79-529a-4a1b-b201-88ee6fc70344";
        //Example string
        static String queryURL;

        public String buildQuery(Query query)
        {

            // sort by citations not implemented
            queryURL = String.Format("http://academic.research.microsoft.com/json.svc/search?AppId={0}&ResultObjects={1}&PublicationContent={2}", appId,"Publication","AllInfo");
            String searchString = null;
            searchString = query.getqueryString();

            searchString = searchString.Replace(" ", "+");

            if (query.resultType == ResultType.AUTHOR)
            {
                queryURL+="&AuthorQuery="+searchString;
            }

            else if (query.resultType == ResultType.JOURNAL)
            {
                queryURL += "&JournalQuery=" + searchString;
            }

            //sort by citations
            if (query.sortOrder == SortOrder.CITATIONS)
            {
                queryURL += "&OrderBy=CitationCount";
            }
            else if(query.sortOrder==SortOrder.DATE)
            {
                queryURL += "&OrderBy=Year";
            }

            //search for custom year range and sorted by citations
            if (query.issetylo())
            {
                queryURL += "&YearStart=" + query.getylo();
            }
            if (query.issetyhi())
            {
                queryURL += "&YearEnd=" + query.getyhi();
            }

            String pageStart = ((query.getpageid() -1)*20 + 1).ToString();
            queryURL += "&StartIdx=" + pageStart;

            String pageEnd = (query.getpageid()*20).ToString(); 
            queryURL += "&EndIdx=" + pageEnd;
            
            //search for custome year range and sorted by date
            //TODO : Need to get the correct URL.
            // #Srikar
            Console.WriteLine("::::::::"+queryURL);
            return queryURL;
        }


        public String buildCitationUrl(int id,SortOrder so,int start)
        {
            String sortOrder;
            if (so == SortOrder.CITATIONS)
            {
                sortOrder = "Citation";
            }
            else
            {
                sortOrder = "Year";
            }
            
            queryURL = String.Format("http://academic.research.microsoft.com/json.svc/search?AppId={0}&ResultObjects={1}&PublicationContent={2}&PublicationId={3}&ReferenceType=Citation&EndIdx={6}&OrderBy={4}&startIdx={5}", appId, "Publication", "AllInfo",id,sortOrder,start,start+10);
            Console.WriteLine("Query :::: :::: ::::" + queryURL);
            return queryURL;
        }

        public String buildAuthorUrl(String author)
        {

            if (author.Contains(" "))
                author.Replace(" ", "+");
            queryURL = String.Format("http://academic.research.microsoft.com/json.svc/search?AppId={0}&ResultObjects={1}&EndIdx={2}&AuthorQuery={3}", appId, "Author",4,author);
            return queryURL;
        }

        public String buildJournalUrl(String author)
        {

            if (author.Contains(" "))
                author.Replace(" ", "+");
            queryURL = String.Format("http://academic.research.microsoft.com/json.svc/search?AppId={0}&ResultObjects={1}&EndIdx={2}&JournalQuery={3}", appId, "Journal", 4, author);
            return queryURL;
        }


    }
}
