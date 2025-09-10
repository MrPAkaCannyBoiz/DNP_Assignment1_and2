using System;
using Entities;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    public List<Comment>? comments { get; set; }
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
        ? comments.Max(c => c.Id) + 1
        : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task DeleteAsync(int id)
    {
        Comment? CommentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (CommentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        comments.Remove(CommentToRemove);
        return Task.CompletedTask;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return comments.AsQueryable();
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? givenComment = comments.SingleOrDefault(c => c.Id == id);
        if (givenComment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        return Task.FromResult(givenComment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Post with ID '{comment.Id} ain't found");
        }
        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }
}
