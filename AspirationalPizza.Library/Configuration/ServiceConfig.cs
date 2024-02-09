using AspirationalPizza.Library.Services.Customers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.Configuration
{
    public class ServiceConfig<T> where T : class
    {
        public RepoConfig? Repository { get; set; }

    }

    public record RepoConfig
    {
        public string RepositoryType { get; set; } = String.Empty;
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
