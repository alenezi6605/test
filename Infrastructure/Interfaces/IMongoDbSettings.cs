using System;
namespace TestsService.Infrastructure.Interfaces
{
	public interface IMongoDbSettings
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string UsersCollectionName { get; set; }
    }
}

