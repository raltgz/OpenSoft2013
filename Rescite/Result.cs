using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Represents the result from a search
 * Result may contain an author/journal and contains a list of papers 
 */
namespace Citeseer
{

    public class ResultList : List<Result>
    {
        public ResultList()
        {
            this.resultType = ResultType.AUTHOR;//By Default
        }

        public ResultType resultType;

        // Only one should be set for each result list
        public SearchType type;

        private int totalcount;

        //Function to create objects according to ResultType
        public void loadType()
        {
            if (resultType.Equals(ResultType.AUTHOR))
            {
                type = new Author();
            }
            else
            {
                type = new Journal();
            }

        }

        //Getters
        public SearchType getType()
        {
            if (this.resultType.Equals(ResultType.AUTHOR))
            {
                return this.type;
            }
            return null;
        }

        public void setCount(int t)
        {

            this.totalcount = t;
        }

        public int getCount()
        {
            return this.totalcount;

        }
    }

    /* Result Class stores the list of results to be returned to the controller
     * Creates Author or Journal Type object on Runtime 
     */
    public class Result
    {


        //List of Papers
        private List<Paper> paperList;


        //Default Constructor
        public Result()
        {
            paperList = new List<Paper>();
        }



        public List<Paper> getResults()
        {
            return this.paperList;
        }

        public void addPaper(Paper paper)
        {
            this.paperList.Add(paper);
        }




    }


    /*SearchType interface gives the basic functions and definitions of 
     * common attributes for search query based on Authors and Journal*/

    abstract public class SearchType
    {
        
        protected String name;
        protected int hIndex;
        protected int iIndex;
        protected int numPapers;
        protected int statnumCitePerPaper;
        protected int statnumCitePerYear;

        //getters
        public String getname()
        {
            return this.name;
        }

       

        public int gethIndex()
        {
            return this.hIndex;
        }

        public int getiIndex()
        {
            return this.iIndex;
        }

        public int getnumPapers()
        {
            return this.numPapers;
        }

        public int getstatNumCitePerPaper()
        {
            return this.statnumCitePerPaper;
        }

        public int getstatnumCitePerYear()
        {
            return this.statnumCitePerYear;
        }

        //setters
        public void setname(String name)
        {
            this.name = name;
        }
       

        public void sethIndex(int hIndex)
        {
            this.hIndex = hIndex;
        }

        public void setiIndex(int iIndex)
        {
            this.iIndex = iIndex;
        }

        public void setnumPapers(int numPapers)
        {
            this.numPapers = numPapers;
        }

        public void setstatNumCitePerPaper(int statnumCitePerPaper)
        {
            this.statnumCitePerPaper = statnumCitePerPaper;
        }

        public void setstatnumCitePerYear(int statnumCitePerYear)
        {
            this.statnumCitePerYear = statnumCitePerYear;
        }


    }

    /*Author Class implements SearchType and represents the Author details*/

    public class Author : SearchType
    {
        private String institute;
        private String resArea;
        private Uri statsGraphUrl;


        //Getters
        public String getinstitute()
        {
            return this.institute;
        }

        public String getresArea()
        {
            return this.resArea;
        }

        public Uri getstatsGraphUrl()
        {
            return this.statsGraphUrl;
        }

        //setters
        public void setinstitute(String institute)
        {
            this.institute = institute;
        }

        public void setresArea(String resArea)
        {
            this.resArea = resArea;
        }

        public void setstatsGraphUrl(Uri statsGraphUrl)
        {
            this.statsGraphUrl = statsGraphUrl;
        }

    }


    /*Journal Class implements SearchType and represents the Journal details*/

    public class Journal : SearchType
    {
        private Uri viewStats;
        private String htmlStats;

        public void setStatsUrl(Uri view)
        {
            this.viewStats = view;
        }

        public Uri getStatsUrl()
        {
            return this.viewStats;
        }

        public void sethtmlStats(String htmlStats)
        {
            this.htmlStats = htmlStats;
        }

        public String gethtmlStats()
        {
            return this.htmlStats;
        }


    }

    /* Paper Class stores the details of each paper */

    public class Paper
    {
        public int id;
        private String title;
        private Uri url;
        private String description;
        private int numCitations;
        private String authors;
        private String citationsUrl;
        private String conferenceUrl;
        private int year;

        //Ctor
        public Paper()
        {

        }


        //Getters
        public String gettitle()
        {
            return this.title;
        }

        public Uri geturl()
        {
            return this.url;
        }
        public int getid()
        {
            return this.id;
        }

        public int getYear()
        {
            return this.year;

        }
        public String getdescription()
        {
            return this.description;
        }

        public String getauthors()
        {
            return this.authors;
        }

        public String getcitationsUrl()
        {
            return this.citationsUrl;
        }

        public String getConferenceUrl()
        {
            return this.conferenceUrl;
        }

        public int getnumCitations()
        {
            return this.numCitations;
        }

        //Setters
        public void settitle(String title)
        {
            Console.Write(title);
            this.title = System.Net.WebUtility.HtmlDecode(title);
        }

        public void setYear(int year)
        {

            this.year = year;
        }
        public void seturl(Uri url)
        {
            this.url = url;
        }

        public void setid(int id)
        {
            this.id = id;
        }

        public void setdescription(String description)
        {
            this.description = System.Net.WebUtility.HtmlDecode(description);
        }

        public void setauthors(String authors)
        {
            this.authors = System.Net.WebUtility.HtmlDecode(authors);
        }

        public void setcitationsUrl(String citationsUrl)
        {
            this.citationsUrl = citationsUrl;
        }

        public void setConferenceUrl(String conferenceUrl)
        {
            this.conferenceUrl = conferenceUrl;
        }

        public void setnumCitations(int numCitations)
        {
            this.numCitations = numCitations;
        }

        public String toString(char delim)
        {
            StringBuilder value = new StringBuilder("");
            value.Append(id);
            value.Append(delim);
            value.Append(title);
            value.Append(delim);
            if (url != null)
                value.Append(url.ToString());
            else
                value.Append("No URL Available");
            value.Append(delim);
            value.Append(description);
            value.Append(delim);
            value.Append(numCitations);
            value.Append(delim);
            value.Append(authors);
            value.Append(delim);
            value.Append(citationsUrl);
            value.Append(delim);
            value.Append(conferenceUrl);
            value.Append(delim);
            value.Append(year);
            return value.ToString();
        }

    }

}
