using System.Collections.Generic;
using System.Threading.Tasks;
using DesignPatterns.Decorator.Models;
namespace DesignPatterns.Decorator.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
    }

}
