using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfcRepositories.Repositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext _appContext;

    public EfcUserRepository(AppContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<User> AddAsync(User user)
    {
        await _appContext.Users.AddAsync(user);
        await _appContext.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        User? existingUser = await _appContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
           throw new KeyNotFoundException($"User with id {id} not found.");
        }
        _appContext.Users.Remove(existingUser);
        await _appContext.SaveChangesAsync();
    }

    public IQueryable<User> GetManyAsync()
    {
        return _appContext.Users.AsQueryable();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? user =  await _appContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        // throw exception if user does not exist
        if (!await _appContext.Users.AnyAsync(u => u.Id == user.Id))
        {
            throw new KeyNotFoundException($"User with id {user.Id} not found.");
        }
        _appContext.Users.Update(user);
        await _appContext.SaveChangesAsync();
    }
}
