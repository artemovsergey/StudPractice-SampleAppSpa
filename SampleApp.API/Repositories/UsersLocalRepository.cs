using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using SampleApp.API.Data;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;

namespace SampleApp.API.Repositories;

public class UsersLocalRepository(SampleAppContext db) : IUserRepository
{
    public List<User> GetUsers()
    {
        return db.Users.ToList();
    }

    public User GetUserWithMicropost(int id){
        var user = db.Users.Include(u => u.Microposts).Where(u => u.Id == id).FirstOrDefault();
        return user != null ? user : throw new Exception($"Нет пользователя с login = {id}");
    }

    public User CreateUser(User user)
    {
        try
        {
            db.Add(user);
            db.SaveChanges();
            return user;
        }
        catch (SqlTypeException ex)
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

    public User FindUserById(int id)
    {
        throw new NotImplementedException();
    }

    public User FindUser(string login)
    {
        var user = db.Users.Where(u => u.Login == login).FirstOrDefault();
        return user != null ? user : throw new Exception($"Нет пользователя с login = {login}");
    }
}