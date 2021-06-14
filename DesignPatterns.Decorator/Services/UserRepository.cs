using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DesignPatterns.Decorator.Interfaces;
using DesignPatterns.Decorator.Models;
using Newtonsoft.Json;
namespace DesignPatterns.Decorator.Services
{
    public class UserRepository : IUserRepository
    {

        private readonly HttpClient _httpClient;

        public UserRepository(HttpClient httpClient) => _httpClient = httpClient;

        async Task<IEnumerable<User>> IUserRepository.GetUsers()
        {
            var users = new List<User>();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/users");

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return users;
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            users = JsonConvert.DeserializeObject<List<User>>(responseString);
            return users;
        }
    }
}
