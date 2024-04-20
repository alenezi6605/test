using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Libmongocrypt;
using TestsService.AppCore.Respositories.Models;
using Collections = TestsService.Domain.Collections;
using TestsService.Domain.Common;
using TestsService.Domain.Exceptions;
using TestsService.Infrastructure.Interfaces;
using TestsService.AppCore.Respositories.Interfaces;
using System.Net;
using TestsService.External;
using MongoPaging = TestsService.AppCore.Respositories.Models;
using TestsService.Domain.Collections;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using TestsService.Infrastructure.Configuration;
using User = TestsService.Domain.Collections.User;
using System.Text.RegularExpressions;

namespace TestsService.AppCore.Respositories
{
    public class UserRespository : IUserRespository
    {
        private readonly IMongoCollection<Collections.User> _userCollection;
        private readonly IMapper _mapper;
        IMongoDbSettings _settings;

        public UserRespository(IMongoDbSettings settings, IMongoClient mongoClient, IMapper mapper)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _userCollection = database.GetCollection<Collections.User>(settings.UsersCollectionName);
            _mapper = mapper;
            _settings = settings;
        }

        public async Task<EntityResponseModel> GetUsers()
        {
            var filter = Builders<User>.Filter.Empty;

            var response = await _userCollection.Find(filter).ToListAsync();

            return new EntityResponseModel()
            {
                Data = response
            };
        }
    }
}
