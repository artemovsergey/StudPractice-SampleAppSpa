using System.Data.SqlTypes;
using SampleApp.API.Data;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;

namespace SampleApp.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SampleAppContext _db;
    public UserRepository(SampleAppContext db)
    {
       _db = db; 
    }

    public User CreateUser(User user)
    {
       try
       {
         _db.Add(user);
         _db.SaveChanges();
         return user;
       }
       catch(SqlTypeException ex)
       {
         throw new SqlTypeException($"Ошибка SQL: {ex.Message}");
       }
       catch (Exception ex)
       {
         throw new Exception($"Ошибка: {ex.Message}");
       }
    }

    public bool DeleteUser(int id)
    {
        throw new NotImplementedException();
    }

    public User EditUser(User user, int id)
    {
       throw new NotImplementedException();
    }

    public User FindUser(string login)
    {
        var user = _db.Users.Where(u =>u.Login == login).FirstOrDefault<User>();
        return user != null ? user : throw new Exception("Пользоватеь не найден!");
    }

    public User FindUserById(int id)
    {
      throw new NotImplementedException();
    }

    public List<User> GetUsers()
    {
       return _db.Users.ToList();
    }
}