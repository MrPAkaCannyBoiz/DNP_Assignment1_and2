using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FileRepositories
{
    public class CommentFileRepository : ICommentRepository
    {
        private readonly string _filePath = "comments.json";

        public CommentFileRepository()
        {
            // Initialize file if it doesn't exist
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }
        public async Task<Comment> AddAsync(Comment comment)
        {
          string commentAsJson = await File.ReadAllTextAsync(_filePath);
          List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson)!;
          int? maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 1;
          comment.Id = maxId + 1;
          comments.Add(comment);
          string commentsJson = JsonSerializer.Serialize(comments);
          await File.WriteAllTextAsync(_filePath, commentsJson);
          return comment;
        }

        public async Task DeleteAsync(int id)
        {
            string commentAsJson = await File.ReadAllTextAsync(_filePath);
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson)!;
            Comment? commentToDelete = comments.FirstOrDefault(c => c.Id == id);
            if (commentToDelete is null)
            {
                throw new InvalidOperationException($"Comment with ID '{id}' not found");
            }
            comments.Remove(commentToDelete);
            commentAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(_filePath, commentAsJson);
        }

        public IQueryable<Comment> GetManyAsync()
        {
            string commentAsJson = File.ReadAllTextAsync(_filePath).Result;
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson)!;
            return comments.AsQueryable();
        }

        public async Task<Comment> GetSingleAsync(int id)
        {
            string commentAsJson = await File.ReadAllTextAsync(_filePath);
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson)!;
            Comment? comment = comments.FirstOrDefault(c => c.Id == id);
            if (comment is null)
            {
                throw new InvalidOperationException($"Comment with ID '{id}' not found");
            }
            return comment;
        }

        public async Task UpdateAsync(Comment comment)
        {
            string commentAsJson = await File.ReadAllTextAsync(_filePath);
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson)!;
            Comment? existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
            if (existingComment is null)
            {
                throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
            }
            comments.Remove(existingComment);
            comments.Add(comment);
            commentAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(_filePath, commentAsJson);
        }
    }
}
