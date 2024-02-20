using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.RepoSupport
{
    internal record RepoTypes
    {
        public const string Mongo = "Mongo";
        public const string SqlServer = "SqlServer";
        public const string Memory = "Memory";
        public const string Cosmos = "Cosmos";
        public const string Postgres = "Postgres";
        public const string MySql = "MySql";
        public const string Sqlite = "Sqlite";
        public const string ElasticSearch = "ElasticSearch";
        public const string Redis = "Redis";

    }
}
