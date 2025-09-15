using System;
using Entities;
namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    public List<User> Users { get; set; }

    public UserInMemoryRepository()
    {
        Users = new List<User>(10);
    }

    public Task<User> AddAsync(User user)
    {
        user.Id = Users.Any()
            ? Users.Max(p => p.Id) + 1
            : 1;
        Users.Add(user);
        return Task.FromResult(user);
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = Users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        Users.Remove(userToRemove);

        return Task.CompletedTask;
    }

    public IQueryable<User> GetManyAsync()
    {
        return Users.AsQueryable();
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? givenUser = Users.SingleOrDefault(u => u.Id == id);
        if (givenUser is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }
        return Task.FromResult(givenUser);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = Users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }
        Users.Remove(existingUser);

        Users.Add(user);

        return Task.CompletedTask;
    }
}
