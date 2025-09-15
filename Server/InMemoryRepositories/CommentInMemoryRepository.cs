using System;
using Entities;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    public List<Comment>? Comments { get; set; }

    public CommentInMemoryRepository()
    {
        Comments = new List<Comment>(10);
    }
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = Comments.Any()
        ? Comments.Max(c => c.Id) + 1
        : 1;
        Comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task DeleteAsync(int id)
    {
        Comment? CommentToRemove = Comments.SingleOrDefault(c => c.Id == id);
        if (CommentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        Comments.Remove(CommentToRemove);
        return Task.CompletedTask;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return Comments.AsQueryable();
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? givenComment = Comments.SingleOrDefault(c => c.Id == id);
        if (givenComment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        return Task.FromResult(givenComment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = Comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Post with ID '{comment.Id} ain't found");
        }
        Comments.Remove(existingComment);
        Comments.Add(comment);
        return Task.CompletedTask;
    }
}
