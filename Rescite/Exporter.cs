using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/**
 * Exprt search results to tsv file
 */
namespace Citeseer
{
    static class Exporter
    {
        public static Boolean exportResults(ResultList resultList,String filePath){
            if (resultList == null)
            {
                Console.WriteLine("result list is null");
                return false;
            }
            try
            {
                System.IO.StreamWriter tsvFile = new System.IO.StreamWriter(filePath);
                //tsvFile.WriteLine(resultList.getType())
                foreach (Result result in resultList)
                {
                    foreach (Paper paper in result.getResults()){
                        tsvFile.WriteLine(paper.toString('\t'));
                    }
                }
                tsvFile.Close();
                return true;
            }
            catch
            {

            }
            return false;
        }

        public static Boolean exportResults(ResultList resultList)
        {
            StringBuilder filePath = new StringBuilder("Export_");
            filePath.Append(getTimestamp(DateTime.Now));
            filePath.Append(".tsv");
            return exportResults(resultList, filePath.ToString()); 
        }

        public static String getTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }
    }
}
