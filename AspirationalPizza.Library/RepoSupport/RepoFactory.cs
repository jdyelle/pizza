using AspirationalPizza.Library.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
/*
namespace AspirationalPizza.Library.RepoSupport
{
    //TODO: Remove this dead code -- it's not used anywhere but was fun.
    //This is an experiment in generic repos, but there might be multiple repos for a service so I'm not sure what to do with this yet.
    public static class RepoFactory
    {
        public static IRepository<TModel> GetRepository<TService, TModel, TDbContext, TEFRepository, TMongoRepository>(
            ILogger<IRepository<TModel>> logger, 
            ServiceConfig<TService> config)
            where TService : class
            where TDbContext : DbContext
            where TEFRepository : IRepository<TModel>, new()
            where TMongoRepository : IRepository<TModel>, new()
        {
            IRepository<TModel>? returnValue = null;
            DbContextOptionsBuilder<TDbContext> optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
            TDbContext? context = null;
            String connectionString = String.Empty;
            Configuration.RepoConfig? repoConfig = config!.Repositories[typeof(TModel).Name];
            if (repoConfig == null) { throw new Exception("Could not find repository config, please configure the service."); }
            switch (repoConfig.RepositoryType)
            {
                case RepoTypes.Mongo:
                    returnValue = new TMongoRepository();
                    return returnValue;
                case RepoTypes.Memory:
                    optionsBuilder.UseInMemoryDatabase(typeof(TModel).Name);
                    context = (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options)!;
                    returnValue = (TEFRepository)Activator.CreateInstance(typeof(TEFRepository), logger, context)!;
                    return returnValue;
                case RepoTypes.Sqlite:
                    connectionString = $"Data Source={repoConfig.Parameters["Filename"]}";
                    optionsBuilder.UseSqlite(connectionString);
                    context = (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options)!;
                    returnValue = (TEFRepository)Activator.CreateInstance(typeof(TEFRepository), logger, context)!;
                    return returnValue;
                case RepoTypes.Postgres:
                    connectionString = new StringBuilder()
                        .Append($"Host={repoConfig.Parameters["DBHost"]};")
                        .Append($"Database={repoConfig.Parameters["DBName"]};")
                        .Append($"Username={repoConfig.Parameters["DBUser"]};")
                        .Append($"Password={repoConfig.Parameters["DBPass"]}")
                        .ToString();
                    context = (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options)!;
                    returnValue = (TEFRepository)Activator.CreateInstance(typeof(TEFRepository), logger, context)!;
                    return returnValue;
                default:
                    throw new NotImplementedException("The database specified in the config has not been implemented for this repository");
            }
        }
    }

    public interface IRepository<T>
    {
    }

    public interface EFRepository<T> : IRepository<T>
    {
    }

    public interface MongoRepository<T> : IRepository<T>
    {
    }
}
*/