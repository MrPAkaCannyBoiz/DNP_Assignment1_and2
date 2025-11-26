using System;

using Entities;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IUserRepository
{

    public List<Post>? Posts { get; set;}

    public PostInMemoryRepository()
    {
        Posts = new List<Post>(10);
    }
    public Task<Post> AddAsync(Post post)
    {
        post.Id = Posts.Any()
        ? Posts.Max(p => p.Id) + 1
        : 1;
        Posts.Add(post);
        return Task.FromResult(post);
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = Posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        Posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public IQueryable<Post> GetManyAsync()
    {
        return Posts.AsQueryable();
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? givenPost = Posts.SingleOrDefault(p => p.Id == id);
        if (givenPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        return Task.FromResult(givenPost);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = Posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id} ain't found");
        }
        Posts.Remove(existingPost);
        Posts.Add(post);
        return Task.CompletedTask;
    }
}
