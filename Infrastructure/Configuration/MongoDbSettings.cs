using System;
using TestsService.Infrastructure.Interfaces;

namespace TestsService.Infrastructure.Configuration
{
	public class MongoDbSettings : IMongoDbSettings
    {
        public string ConnectionString { get; set; } = String.Empty;

        public string DatabaseName { get; set; } = String.Empty;

        public string UsersCollectionName { get; set; } = String.Empty;
    }
}