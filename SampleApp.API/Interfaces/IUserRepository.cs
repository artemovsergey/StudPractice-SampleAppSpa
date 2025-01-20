using SampleApp.API.Entities;

namespace SampleApp.API.Interfaces;

public interface IUserRepository
{
   User FindUser(string login);
   User CreateUser(User user);
   List<User> GetUsers();
   User EditUser(User user, int id);
   bool DeleteUser(int id);
   User FindUserById(int id);
}