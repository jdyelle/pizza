namespace AspirationalPizza.Library.Configuration
{
    public class ServiceConfig<T> where T : class
    {
        public Dictionary<String, RepoConfig> Repositories { get; set; } = new Dictionary<String, RepoConfig>();

    }

    public record RepoConfig
    {
        public string RepositoryType { get; set; } = String.Empty;
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
