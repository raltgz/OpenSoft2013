using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
/*
 * Class to represent the query structure
 * Should have getters and setters
 */
namespace Citeseer
{

    public enum SortOrder
    {
        DATE, CITATIONS
    }

    public enum ResultType
    {
        AUTHOR, JOURNAL
    }

    [Serializable]
    public class Query : ISerializable
    {
        private String queryString;
        private int ylo;
        private int yhi;
        private Boolean includePatents;
        private int id;

        public SortOrder sortOrder;

        public ResultType resultType;

        public Query()
        {
        }

        public String getqueryString()
        {
            return this.queryString;
        }

        public Boolean issetylo()
        {
            return this.ylo!=0;
        }

        public Boolean issetyhi()
        {
            return this.yhi!=0;
        }

        public Boolean issetqueryString()
        {
            return this.queryString != null;
        }

        public int getylo()
        {
            return this.ylo;
        }

        public int getpageid()
        {
            return this.id;
        }

        public int getyhi()
        {
            return this.yhi;
        }

        public Boolean getincludePatents()
        {
            return this.includePatents;
        }

        public void setqueryString(String queryString)
        {
            this.queryString = queryString;
        }

        public void setylo(int ylo)
        {
            this.ylo = ylo;
        }

        public void setpageid(int id)
        {
            this.id = id;
        }

        public void setyhi(int yhi)
        {
            this.yhi = yhi;
        }

        public void setincludePatents(Boolean includePatents)
        {
            this.includePatents = includePatents;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("queryString", queryString, typeof(string));
            info.AddValue("ylo", ylo, typeof(int));
            info.AddValue("yhi", yhi, typeof(int));
            info.AddValue("patBool", includePatents, typeof(int));
            info.AddValue("qid", id, typeof(int));
            info.AddValue("sortOrder", sortOrder, typeof(SortOrder));
            info.AddValue("resultType", resultType, typeof(ResultType));
        }

        // The special constructor is used to deserialize values. 
        public Query(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            queryString = (string) info.GetValue("queryString", typeof(string));
            ylo = (int)info.GetValue("ylo", typeof(int));
            yhi = (int)info.GetValue("yhi", typeof(int));
            id = (int)info.GetValue("qid", typeof(int));
            includePatents = (Boolean)info.GetValue("patBool", typeof(Boolean));
            sortOrder = (SortOrder)info.GetValue("sortOrder",typeof(SortOrder));
            resultType = (ResultType)info.GetValue("resultType", typeof(ResultType));
        }

        public Boolean Equals(Query q)
        {
            if (!(queryString.Equals(q.getqueryString())))
                return false;
            if (!(ylo.Equals(q.getylo())))
                return false;
            if (!(yhi.Equals(q.getyhi())))
                return false;
            if (!(includePatents.Equals(q.getincludePatents())))
                return false;
            if (!(id.Equals(q.getpageid())))
                return false;
            if (!(sortOrder.Equals(q.sortOrder)))
                return false;
            if (!(resultType.Equals(q.resultType)))
                return false;
            return true;
        }

    }
}

