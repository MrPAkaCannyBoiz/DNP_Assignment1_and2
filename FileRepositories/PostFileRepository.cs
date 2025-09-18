using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FileRepositories
{
    public class PostFileRepository : IPostRepository
    {
        private readonly string _filePath = "posts.json";
        public PostFileRepository()
        {
            // Initialize file if it doesn't exist
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public async Task<Post> AddAsync(Post post)
        {
            string postAsJson = await File.ReadAllTextAsync(_filePath);
            //Deserialize the existing posts from the file
            List<Post> posts = System.Text.Json.JsonSerializer.Deserialize<List<Post>>(postAsJson)!;
            //If the list is not empty, get the max id, else start from 1
            int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 1;
            post.Id = maxId + 1; //Then add 1 to it
            posts.Add(post);
            //Serialize back to json and save to file after adding new post
            postAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(_filePath, postAsJson);
            return post;
        }

        public async Task DeleteAsync(int id)
        {
            string postAsJson = await File.ReadAllTextAsync(_filePath);
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson)!;
            Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
            if(postToRemove is null)
        {
                throw new InvalidOperationException($"Post with ID '{id}' not found");
            }
            posts.Remove(postToRemove);
            postAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(_filePath, postAsJson);
            return;
        }

        public IQueryable<Post> GetManyAsync()
        {
            string postAsJson = File.ReadAllTextAsync(_filePath).Result;
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson)!;
            return posts.AsQueryable();
        }

        public async Task<Post> GetSingleAsync(int id)
        {
            string postAsJson = await File.ReadAllTextAsync(_filePath);
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson)!;
            Post? givenPost = posts.SingleOrDefault(p => p.Id == id);
            if (givenPost is null)
            {
                throw new InvalidOperationException($"Post with ID '{id}' not found");
            }
            return givenPost;
        }

        public async Task UpdateAsync(Post post)
        {
          string postAsJson = await File.ReadAllTextAsync(_filePath);
          List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson)!;
          Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
            if (existingPost is null)
            {
                throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
            }
            posts.Remove(existingPost);
            posts.Add(post);
            postAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(_filePath, postAsJson);
        }
    }
}
