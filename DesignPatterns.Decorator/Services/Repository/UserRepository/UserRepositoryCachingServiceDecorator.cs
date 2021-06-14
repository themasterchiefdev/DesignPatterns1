using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesignPatterns.Decorator.Interfaces;
using DesignPatterns.Decorator.Models;
using Microsoft.Extensions.Caching.Memory;
namespace DesignPatterns.Decorator.Services.Repository.UserRepository
{
    public class UserRepositoryCachingServiceDecorator : IUserRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _innerUserRepository;

        public UserRepositoryCachingServiceDecorator(IUserRepository innerUserRepository, IMemoryCache cache)
        {
            _innerUserRepository = innerUserRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            const string cacheKey = "Users";
            if (_cache.TryGetValue<IEnumerable<User>>(cacheKey, out var listOfUsers))
                return listOfUsers;
            var newListOfUsers = await _innerUserRepository.GetUsers().ConfigureAwait(false);
            _cache.Set(cacheKey, newListOfUsers, TimeSpan.FromSeconds(30));
            return newListOfUsers;
        }
    }
}
