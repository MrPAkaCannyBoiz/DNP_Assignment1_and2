using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfcRepositories.Repositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext _appContext;

    public EfcPostRepository(AppContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<Post> AddAsync(Post post)
    {
        await _appContext.Posts.AddAsync(post);
        await _appContext.SaveChangesAsync();
        return post;
    }

    public async Task DeleteAsync(int id)
    {
        Post? existingPost = await _appContext.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existingPost == null)
        {
            throw new KeyNotFoundException($"Post with id {id} not found.");
        }
        _appContext.Posts.Remove(existingPost);
        await _appContext.SaveChangesAsync();
    }

    public IQueryable<Post> GetManyAsync()
    {
        return _appContext.Posts.AsQueryable();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        Post? existingPost = await _appContext.Posts.SingleOrDefaultAsync(p => p.UserId == id);
        if (existingPost == null)
        {
            throw new KeyNotFoundException($"Post with id {id} not found.");
        }
        return existingPost;
    }

    public async Task UpdateAsync(Post post)
    {
        bool exists = await _appContext.Posts.AnyAsync(p => p.Id == post.Id);
        if (!exists)
        {
            throw new KeyNotFoundException($"Post with id {post.Id} not found.");
        }
        _appContext.Posts.Update(post);
        await _appContext.SaveChangesAsync();
    }
}
