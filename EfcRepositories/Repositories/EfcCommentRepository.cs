using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EfcRepositories.Repositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext _appContext;

    public EfcCommentRepository(AppContext appContext)
    {
        _appContext = appContext;
    }
    public async Task<Comment> AddAsync(Comment comment)
    {
       await _appContext.Comments.AddAsync(comment);
       await _appContext.SaveChangesAsync();
       return comment;
    }

    public async Task DeleteAsync(int id)
    {
        Comment? existingComment = await _appContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
        if (existingComment == null)
        {
            throw new KeyNotFoundException($"Comment id {id} not found");
        }
        _appContext.Comments.Remove(existingComment);
        await _appContext.SaveChangesAsync();
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return _appContext.Comments.AsQueryable();
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        Comment? existingComment = await _appContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
        if (existingComment == null)
        {
            throw new KeyNotFoundException($"Comment id {id} not found");
        }
        return existingComment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        bool exists = await _appContext.Comments.AnyAsync(c => c.Id == comment.Id);
        if (!exists)
        {
            throw new KeyNotFoundException($"Comment id {comment.Id} not found");
        }
        _appContext.Comments.Update(comment);
        await _appContext.SaveChangesAsync();
    }
}
