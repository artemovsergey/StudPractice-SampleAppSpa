using System.Data.SqlTypes;
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


}