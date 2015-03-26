using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Citeseer
{
    /**
     * Receives a URL, gets the corresponding HTML, parses it and returns the Result
     * The result may contain an author/journal.
     */
    interface Parser
    {
        //Result getResults(String URL);
    }

    class GSParser : Parser
    {
        Query query;

        private ResultList resultList;
        public GSParser()
        {
            resultList = new ResultList();

        }



        public void addResult(Result result)
        {
            this.resultList.Add(result);
        }

        public ResultList GSbuildQuery(Query query)
        {

            GSQueryURLBuilder QB = new GSQueryURLBuilder();
            String URL = QB.buildQuery(query);
            this.query = query;

            var strUrl = new Uri(URL);



            int i;


            for (i = 0; i < 50; i += 10)
            {
                Result result = new Result();

                String newURL;
                newURL = strUrl + "&start=" + i.ToString();
                var newUrl = new Uri(newURL);
                Console.WriteLine(newUrl);
                result = GSConnect(newUrl);
                if (result != null)
                    this.resultList.Add(result);
                else
                    return null;

            }

            checkProfile(strUrl);

            return this.resultList;



        }

        public Result GSContinue(Uri URL, int i)
        {
            Result result = new Result();
            return result;



        }

        public String GSTryConnect(Uri URL)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            //WebProxy myproxy = new WebProxy("144.16.192.245:8080",false);

            // request.Proxy = myproxy;
            request.Method = "GET";

            //request.Headers["user-agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; ;

            request.Method = "GET";

            String strResponse = "";
            try
            {

                WebResponse response = (HttpWebResponse)request.GetResponse();

                Stream receiveStream = response.GetResponseStream();

                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                StreamReader readStream = new StreamReader(receiveStream, encode);

                strResponse = readStream.ReadToEnd();

                response.Close();
            }

            catch (Exception e)
            {


            }
            return strResponse;


        }

        public void checkProfile(Uri URL)
        {
            Console.WriteLine("Author Url :: ::" + URL.ToString());
            String strResponse = this.GSTryConnect(URL);
            HtmlDocument document = new HtmlDocument();
            document.Load(new StringReader(strResponse));

            Result result = new Result();
            String profileLink = "";
            HtmlNode profileNode = document.DocumentNode.SelectSingleNode("//div[@id='gs_ccl']/div[@class='gs_r']/table");

            if (profileNode != null)
            {

                String au = profileNode.InnerText;
                //  Console.WriteLine(au);


                HtmlNode tempNode = profileNode.SelectSingleNode("//h4[@class='gs_rt2']");
                // Console.WriteLine(tempNode.InnerText);
                HtmlNode authorLinkNode = tempNode.SelectSingleNode("a[@href]");
                profileLink = authorLinkNode.Attributes["href"].Value;
                // Console.WriteLine(profileLink);
                profileLink = "http://scholar.google.co.in" + profileLink;
                var strURL = new Uri(profileLink);

                Console.WriteLine(strURL);

                GSParseProfile(strURL);


            }
            else
            {
                Console.Write("Profile page not parsed");
            }


        }

        public Result GSConnect(Uri URL)
        {

            //Console.WriteLine(URL.ToString());


            String strResponse = this.GSTryConnect(URL);
            HtmlDocument document = new HtmlDocument();
            document.Load(new StringReader(strResponse));

            String resTitle = "";
            String resAuthor = "";
            String resContent = "";
            String resCite = "";
            String resLink = "";
            String profileLink = "";
            String resTitleLink = "";

            Result result = new Result();



            HtmlNode testNode = document.DocumentNode.SelectSingleNode("//div[@id='gs_ccl']/p[@class='gs_med']");

            if (testNode != null)
            {

                TabPage.displayError("No Results Found ! Please Try Again!!!");
                return null;
            }





            testNode = document.DocumentNode.SelectSingleNode("//div[@class='gs_r']/div[@class='gs_ri']");
            if (testNode != null)
            {
                foreach (HtmlNode startNode in document.DocumentNode.SelectNodes("//div[@class='gs_r']/div[@class='gs_ri']"))
                {
                    Paper paper = new Paper();

                    HtmlNode titleNode = startNode.SelectSingleNode("h3[@class='gs_rt']");

                    resTitle = titleNode.InnerText;
                    paper.settitle(resTitle);

                    HtmlNode authorLinkNode = titleNode.SelectSingleNode("a[@href]");

                    resTitleLink = authorLinkNode.Attributes["href"].Value;

                    paper.seturl(new Uri(resTitleLink));

                    HtmlNode authorNode = startNode.SelectSingleNode("div[@class='gs_a']");


                    resAuthor = authorNode.InnerText;
                    paper.setauthors(resAuthor);


                    HtmlNode contentNode = startNode.SelectSingleNode("div[@class='gs_rs']");

                    if (contentNode != null)
                    {
                        resContent = contentNode.InnerText;
                        if (resContent != null)
                        {
                            paper.setdescription(resContent);
                        }
                    }


                    HtmlNode citeNode = startNode.SelectSingleNode("div[@class='gs_fl']");

                    int numCite;
                    if (citeNode != null)
                    {

                        HtmlNode citeInfoNode = citeNode.SelectSingleNode("a[@href]");

                        resCite = citeInfoNode.InnerText;
                        if (resCite.Contains("Cited by"))
                        {
                            resCite = resCite.Replace("Cited by ", "");
                            numCite = Convert.ToInt32(resCite);
                            paper.setnumCitations(numCite);
                            resLink = citeInfoNode.Attributes["href"].Value;

                            paper.setcitationsUrl(resLink);
                        }
                        else
                        {

                            numCite = 0;
                            paper.setnumCitations(numCite);
                        }
                    }
                    else
                    {
                        numCite = 0;
                        paper.setnumCitations(numCite);

                    }


                    result.addPaper(paper);

                }
                return result;

            }

            else
            {

                //TabPage.displayError("GS Blocking!!!");
                return null;
            }





        }


        public void GSParseProfile(Uri URL)
        {

            String strResponse = this.GSTryConnect(URL);

            HtmlDocument document = new HtmlDocument();
            document.Load(new StringReader(strResponse));

            String authorName = "";
            String residence = "";
            String interest = "";
            String hindex = "";
            String iindex = "";
            String totalcitations = "";

            this.resultList.resultType = this.query.resultType;
            this.resultList.loadType();

            SearchType type;


            Author author = new Author();
            author.setresArea("");
            author.setstatsGraphUrl(new Uri("http://google.com"));
            author.setinstitute("");





            HtmlNode profileNode = document.DocumentNode.SelectSingleNode("//div[@class='g-section g-tpl-250-alt']//div[@class='g-unit']");
            if (profileNode != null)
            {
                //Console.WriteLine("hey");
                // Console.WriteLine(profileNode.InnerText);
                //   Author author = new Author();
                HtmlNode userInfoNode = profileNode.SelectSingleNode("//div[@class='cit-user-info']//div[@class='g-unit']");

                HtmlNode userNameNode = userInfoNode.SelectSingleNode("//span[@id='cit-name-read']");

                authorName = userNameNode.InnerText;
                author.setname(authorName);

                HtmlNode userResidenceNode = userInfoNode.SelectSingleNode("//span[@id='cit-affiliation-read']");

                residence = userResidenceNode.InnerText;
                author.setinstitute(residence);

                HtmlNode userInterestNode = userInfoNode.SelectSingleNode("//span[@id='cit-int-read']");

                interest = userInterestNode.InnerText;
                author.setresArea(interest);

                Console.WriteLine("Author " + authorName + residence + interest);

                HtmlNode dataNode = profileNode.SelectSingleNode("div[@class='cit-lbb']//table//tr//table[@id='stats']");

                if (dataNode != null)
                {
                    int i = 0;
                    foreach (HtmlNode hindexNode in dataNode.SelectNodes("tr//td[@class='cit-borderleft cit-data']"))
                    {
                        if (i == 0)
                        {
                            totalcitations = hindexNode.InnerText;
                            author.setnumPapers(Convert.ToInt32(totalcitations));
                        }

                        else if (i == 2)
                        {
                            hindex = hindexNode.InnerText;
                            author.sethIndex(Convert.ToInt32(hindex));
                        }

                        else if (i == 4)
                        {
                            iindex = hindexNode.InnerText;
                            author.setiIndex(Convert.ToInt32(iindex));
                        }

                        i++;
                    }

                    HtmlNode imageNode = profileNode.SelectSingleNode("div[@class='cit-lbb']//table//td[@valign='top']//img[@src]");

                    String url = imageNode.Attributes["src"].Value;
                    Console.WriteLine(hindex + "hey " + iindex + "hi" + totalcitations + " " + url);
                    url = System.Net.WebUtility.HtmlDecode(url);
                    var strUrl = new Uri(url);
                    author.setstatsGraphUrl(strUrl);
                    type = author;
                    this.resultList.type = type;



                }

            }
            else
            {
                Console.WriteLine("Node Null");
            }





        }
    }

    class MSASParser : Parser
    {
        Query query;
        private ResultList resultList;
        public MSASParser()
        {
            resultList = new ResultList();

        }


        public ResultList MSASbuildQuery(Query query)
        {

            MSASQueryURLBuilder MSB = new MSASQueryURLBuilder();
            String URL = MSB.buildQuery(query);
            this.query = query;

            var strUrl = new Uri(URL);
            Console.WriteLine(URL);
            this.resultList.resultType = query.resultType;
            this.resultList.loadType();
            this.resultList = MSASConnectFetch(strUrl);
            return this.resultList;


        }


        public ResultList MSASConnectFetch(Uri URL)
        {

            WebClient wc = new WebClient();
            wc.Proxy = WebProxy.GetDefaultProxy();

            ResultList resList = new ResultList();


            var data = " ";
            try
            {
                data = wc.DownloadString(URL);
                //Console.WriteLine(data);
            }
            catch (Exception e)
            {
                //TabPage.displayError("MS Also blocking Dude !!! YY !!!!");
                return null;
            }
            JObject obj = JObject.Parse(data);

            int paperCount = 0;

            if (obj["d"]["Publication"]["Result"].Count() == 0)
            {

                //TabPage.displayError("No Results");
                return null;
            }
            int total = (int)obj["d"]["Publication"]["TotalItem"];
            Console.WriteLine("sdfsd" + total);
            resList.setCount(total);
            while (paperCount < obj["d"]["Publication"]["Result"].Count())
            {

                Result result = new Result();
                Paper p = new Paper();



                String title = (String)obj["d"]["Publication"]["Result"][paperCount]["Title"];
                p.settitle(title);

                String content = (String)obj["d"]["Publication"]["Result"][paperCount]["Abstract"];
                p.setdescription(content);
                int authorCount = 0;

                String authors = "";

                while (authorCount < obj["d"]["Publication"]["Result"][paperCount]["Author"].Count() && authorCount < 5)
                {

                    authors += (String)obj["d"]["Publication"]["Result"][paperCount]["Author"][authorCount]["FirstName"] + " ";

                    if ((String)obj["d"]["Publication"]["Result"][paperCount]["Author"][authorCount]["MiddleName"] != "")

                        authors += (String)obj["d"]["Publication"]["Result"][paperCount]["Author"][authorCount]["MiddleName"] + " " + (String)obj["d"]["Publication"]["Result"][paperCount]["Author"][authorCount]["LastName"] + " , ";
                    else
                        authors += (String)obj["d"]["Publication"]["Result"][paperCount]["Author"][authorCount]["LastName"] + " , ";

                    authorCount++;
                }

                p.setYear((int)obj["d"]["Publication"]["Result"][paperCount]["Year"]);
                p.setauthors(authors);

                //if (obj["d"]["Publication"]["Result"][paperCount]["Conference"] != null)
                //  p.setConferenceUrl(((String)(obj["d"]["Publication"]["Result"][paperCount]["Conference"][0])));


                int citations = Convert.ToInt32((String)obj["d"]["Publication"]["Result"][paperCount]["CitationCount"]);
                p.setnumCitations(citations);

                String url = "";

                if (obj["d"]["Publication"]["Result"][paperCount]["FullVersionURL"].Count() > 0)
                {
                    url = (String)obj["d"]["Publication"]["Result"][paperCount]["FullVersionURL"][0];
                }


                if (url != "")
                {
                    Uri titleURL = new Uri(url);
                    p.seturl(titleURL);
                }
                else
                {
                    p.seturl(null);
                }
                int id = (int)obj["d"]["Publication"]["Result"][paperCount]["ID"];
                p.setid(id);
                paperCount++;

                result.addPaper(p);
                resList.Add(result);
            }
            return resList;

        }

        public SearchType MSParseProfile(Uri URL, String compareName, ResultType resType)
        {
            Author author = new Author();
            String type = "Author";
            if (resType == ResultType.AUTHOR)
            {
                type = "Author";
            }
            else if (resType == ResultType.JOURNAL)
            {
                type = "Journal";
            }
            WebClient wc = new WebClient();
            wc.Proxy = WebProxy.GetDefaultProxy();

            var data = wc.DownloadString(URL);
            //Console.WriteLine(data);

            JObject obj = JObject.Parse(data);
            int resultCount = 0;

            /*  while (resultCount < obj["d"]["Author"]["Result"].Count())
              {
                


                 String firstName = (String)obj["d"]["Author"]["Result"][resultCount]["FirstName"];
                  String lastName = (String)obj["d"]["Author"]["Result"][resultCount]["LastName"];
                  String middleName = (String)obj["d"]["Author"]["Result"][resultCount]["MiddleName"];

                  if (compareName.Contains(firstName) || compareName.Contains(lastName) || compareName.Contains(middleName))
                  {
                      String authorname = (String)obj["d"]["Author"]["Result"][resultCount]["FirstName"] + " " + (String)obj["d"]["Author"]["Result"][resultCount]["MiddleName"] + " " + (String)obj["d"]["Author"]["Result"][resultCount]["LastName"];

                      Console.WriteLine(authorname);
                      author.setname(authorname);

                      String hindex = (String)obj["d"]["Author"]["Result"][resultCount]["HIndex"];

                      author.sethIndex(Convert.ToInt32(hindex));

                      String iindex = (String)obj["d"]["Author"]["Result"][resultCount]["GIndex"];

                      author.setiIndex(Convert.ToInt32(iindex));

                      String count = (String)obj["d"]["Author"]["Result"][resultCount]["CitationCount"];

                      author.setnumPapers(Convert.ToInt32(count));
                  }


                
                  resultCount++;

              }*/
            int num = 5;
            if (obj["d"][type]["Result"].Count() < num)
            {
                num = obj["d"][type]["Result"].Count();
            }
            if (obj["d"][type]["Result"].Count() > 0)
            {
                String authorname = "";
                int fl = 0;
                for (int i = 0; i < num; i++)
                {
                    authorname = (String)obj["d"][type]["Result"][i]["FirstName"] + " " + (String)obj["d"][type]["Result"][i]["MiddleName"] + " " + (String)obj["d"][type]["Result"][i]["LastName"];
                    Console.WriteLine(authorname+"----====----"+compareName);
                    String[] comp;
                    char [] ch={' '};
                    comp = compareName.ToLower().Split(ch);
                    foreach (String s in comp)
                    {
                        if (authorname.ToLower().Contains(s))
                        {
                            Console.WriteLine(authorname + "----====----" + compareName);
                            resultCount = i;
                            fl=1;
                            break;
                        }
                    }
                    if (fl == 1)
                    {
                        break;
                    }

                }
                authorname = (String)obj["d"][type]["Result"][resultCount]["FirstName"] + " " + (String)obj["d"][type]["Result"][resultCount]["MiddleName"] + " " + (String)obj["d"][type]["Result"][resultCount]["LastName"];
                Console.WriteLine(authorname);
                author.setname(authorname);

                String hindex = (String)obj["d"][type]["Result"][resultCount]["HIndex"];

                author.sethIndex(Convert.ToInt32(hindex));

                String iindex = (String)obj["d"][type]["Result"][resultCount]["GIndex"];

                author.setiIndex(Convert.ToInt32(iindex));

                String count = (String)obj["d"][type]["Result"][resultCount]["CitationCount"];

                author.setnumPapers(Convert.ToInt32(count));


            }


            return author;

        }

        public SearchType MSParseJournal(String search)
        {


            Journal journal = new Journal();
            String url = "http://scimagojr.com/journalsearch.php?q=" + search + "&tip=jou";

            Uri URL = new Uri(url);
            Console.WriteLine(URL.ToString());
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            //WebProxy myproxy = new WebProxy("144.16.192.245:8080",false);

            // request.Proxy = myproxy;
            request.Method = "GET";

            //request.Headers["user-agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; ;

            request.Method = "GET";

            String strResponse = "";
            try
            {

                WebResponse response = (HttpWebResponse)request.GetResponse();

                Stream receiveStream = response.GetResponseStream();

                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                StreamReader readStream = new StreamReader(receiveStream, encode);

                strResponse = readStream.ReadToEnd();

                response.Close();
            }

            catch (Exception e)
            {


            }

            Console.WriteLine(strResponse);
            //copied till here

            HtmlDocument document = new HtmlDocument();
            document.Load(new StringReader(strResponse));

            HtmlNode startNode = document.DocumentNode.SelectSingleNode("//div[@id='contenedor']/div[@id='subcontenedor']/div[@id='derecha']/div[@id='derecha_contenido']");

            if (startNode != null)
            {
                int i = 0;
                String profileLink = "";
                foreach (HtmlNode linkNode in startNode.SelectNodes("p"))
                {
                    if (i == 2)
                    {
                        HtmlNode authorLinkNode = linkNode.SelectSingleNode("a[@href]");
                        profileLink = authorLinkNode.Attributes["href"].Value;
                    }
                    i++;
                }

                profileLink = "http://scimagojr.com/" + profileLink;

                Uri u = new Uri(profileLink);
                Console.WriteLine(u.ToString());
                request = (HttpWebRequest)WebRequest.Create(u);
                //WebProxy myproxy = new WebProxy("144.16.192.245:8080",false);

                // request.Proxy = myproxy;
                request.Method = "GET";

                //request.Headers["user-agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
                request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; ;

                request.Method = "GET";

                strResponse = "";
                try
                {

                    WebResponse response = (HttpWebResponse)request.GetResponse();

                    Stream receiveStream = response.GetResponseStream();

                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                    StreamReader readStream = new StreamReader(receiveStream, encode);

                    strResponse = readStream.ReadToEnd();

                    response.Close();
                }

                catch (Exception e)
                {


                }

                document.Load(new StringReader(strResponse));
                
                String table = "";
                String name = "";
                String hIndex = "";
                if (document.DocumentNode.SelectSingleNode("//div[@id='contenedor']/div[@id='subcontenedor']/div[@id='derecha']/div[@id='derecha_contenido']/div[@id='grupo_data']")!=null)
                {
                    startNode = document.DocumentNode.SelectSingleNode("//div[@id='contenedor']/div[@id='subcontenedor']/div[@id='derecha']/div[@id='derecha_contenido']/div[@id='grupo_data']");
                    name = document.DocumentNode.SelectNodes("//div[@id='contenedor']/div[@id='subcontenedor']/div[@id='derecha']/div[@id='derecha_contenido']/h1[@class='menor']")[1].WriteTo();
                    name=name.Replace("<h1 class=\"menor\">​","");
                    name=name.Replace("</h1>​", "");
                    HtmlNode d = document.DocumentNode.SelectSingleNode("//div[@id='contenedor']/div[@id='subcontenedor']/div[@id='derecha']/div[@id='derecha_contenido']");
                    hIndex = document.DocumentNode.SelectNodes("//div[@id='contenedor']/div[@id='subcontenedor']/div[@id='derecha']/div[@id='derecha_contenido']/p")[5].WriteTo();
                    table = startNode.WriteTo();
                }
                
                String html = "<html><head></head><body style=\"text-align:center\" class=\"metrouicss bg-color-blueLight\"><h2>"+name+"</h2>"+hIndex+"<div id= \"brli\" ></div>"+table+"</body></html>";
                journal.setname(name);
                journal.sethtmlStats(html);
                journal.setStatsUrl(u);



            }

            return journal;
        }
    }
}
