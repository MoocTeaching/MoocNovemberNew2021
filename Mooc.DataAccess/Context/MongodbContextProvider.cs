using MongoDB.Driver;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.DataAccess.Context
{
    public class MongodbContextProvider : IMongodbContextProvider
    {



        public IMongoDatabase GetMongodbContext()
        {
            //string connnect= ConfigurationManager.AppSettings["mongodb"];
            string connnect= "mongodb://localhost:27017";
            var client = new MongoClient(connnect);
            return client.GetDatabase("ImageDB");
        }
    }
}
