using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citeseer
{
    /**
     * Gets a query object from controller and resturns a result object
     * Multiple searches/queries/parses would be required
     * Searcher has to handle this and consolidate the result
     * We can use the same result object and add to the list of papers from new search results
     */
    class Searcher
    {
        

        
      

        public ResultList GSsearch(Query query)
        {
            //String queryURL = querybuilder.getURL
            //Result = parser.getResult

            //Dummy Results
            ResultList resultList = new ResultList();
            GSParser parser = new GSParser();

           resultList = parser.GSbuildQuery(query);
           /* Paper paper = new Paper();
            paper.settitle("A hybrid feature set based maximum entropy Hindi named entity recognition");
            paper.setdescription("hsjkasdhkjshjsak");
            paper.seturl(new Uri("http://google.co.in"));
            paper.setauthors("S Dandapat, S Sarkar, A Basu - Proceedings of the International …, 2004 - pdf.aminer.org");
            paper.setcitationsUrl("skhadkjshadksjdhkj");
            result.addPaper(paper);
            result.addPaper(paper);
            result.addPaper(paper);
            result.addPaper(paper);
            * */


           return resultList;
            //
          //  return null;            //TODO
        }


        public ResultList GSsearchCitationUrl(String url)
        {
            ResultList resultList = new ResultList();
            GSParser parser = new GSParser();

            url = "http://scholar.google.co.in" + url;
            var citationUrl = new Uri(url);
            int i;
            for (i = 0; i < 10; i += 10)
            {
                Result result = new Result();

                String newURL;
                newURL = citationUrl + "&start=" + i.ToString();
                var newUrl = new Uri(newURL);
                Console.WriteLine(newUrl);
                resultList.Add(parser.GSConnect(newUrl));

            }
            return resultList;

        }
        public ResultList MSASsearchCitationUrl(String url)
        {
            ResultList resultList = new ResultList();
            MSASParser parser = new MSASParser();

            Console.WriteLine(url);
            Uri URL = new Uri(url);
            resultList = parser.MSASConnectFetch(URL);
            
            return resultList;
        }
        public ResultList MSASsearch(Query query)
        {
            ResultList resultList = new ResultList();
            MSASParser parser = new MSASParser();


            resultList = parser.MSASbuildQuery(query);
            return resultList;

        }
 

        
    }
}
