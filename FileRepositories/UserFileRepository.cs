using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileRepositories
{
    public class UserFileRepository : IUserRepository
    {
        private readonly string _filePath = "users.json";

        public UserFileRepository()
        {
            // Initialize file if it doesn't exist
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }
        public async Task<User> AddAsync(User user)
        {
            string userAsJson = await File.ReadAllTextAsync(_filePath);
            //deserialize json to list of users
            List<User> users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(userAsJson)!;
            //if user list is not empty, get max id, else start from 1
            int maxId = users.Count > 0 ? users.Max(u => u.Id) : 1; 
            user.Id = maxId + 1; //then add 1 to it
            users.Add(user);
            //serialize back to json and save to file after adding new user
            userAsJson = System.Text.Json.JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(_filePath, userAsJson);
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            string userAsJson = await File.ReadAllTextAsync(_filePath);
            // deserialize json to list of users
            List<User> users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(userAsJson)!;
            // find user to remove with id with exception if not found
            User? userToRemove = users.SingleOrDefault(u => u.Id == id);
            if (userToRemove is null)
            {
                throw new InvalidOperationException(
                    $"User with ID '{id}' not found");
            }
            users.Remove(userToRemove);
            // serialize back to json and save to file after removing user
            userAsJson = System.Text.Json.JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(_filePath, userAsJson);
            return;
        }

        public IQueryable<User> GetManyAsync()
        {
            // Sometimes, you may not be able to await a Task
            // Instead you can call Result on a task, as in the first statement, at the end:
            string userAsJson = File.ReadAllTextAsync(_filePath).Result;
            List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson)!;
            //we only deserialize as we are not writing anything to the file, only reading
            return users.AsQueryable();
        }

        public async Task<User> GetSingleAsync(int id)
        {
            string userAsJson = await File.ReadAllTextAsync(_filePath);
            //Deserialize json to list of users
            List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson)!;
            User? givenUser = users.SingleOrDefault(u => u.Id == id); //look for user with id via lambda expression
            if (givenUser is null)
            {
                throw new InvalidOperationException($"User with ID '{id}' not found");
            }
            return givenUser;
        }

        public async Task UpdateAsync(User user)
        {
            string userAsJson = await File.ReadAllTextAsync(_filePath);
            //Deserialize json to list of users
            List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson)!;
            //find user using trasitive dependency via Id
            User? givenUser = users.SingleOrDefault(u => u.Id == user.Id); 
           if (givenUser is null)
            {
                throw new InvalidOperationException($"User ID {user.Id} not found");
            }
            //remove then add to update
            users.Remove(givenUser);
            users.Add(user);
            // serialize back to json and save to file after update user
            userAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(_filePath, userAsJson);
        }
    }
}
