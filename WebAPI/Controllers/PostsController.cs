using Microsoft.AspNetCore.Mvc;
using Entities;
using ApiContracts.Dto.PostDto;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostsController(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public ActionResult<IQueryable<Post>> GetAllPosts() => Ok(_postRepository.GetManyAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPostById(int id)
    {
        try
        {
            var post = await _postRepository.GetSingleAsync(id);
            return Ok(post);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500);
        }
    }

    // post has user id as foreign key and post cannot exist without user
    // so the http link should be posts/users/{userId}
    [HttpPost("users/{id}")]
    public async Task<ActionResult<Post>> Post(int id, [FromBody] CreatePostDto request)
    {
        try
        {
            var user = await _userRepository.GetSingleAsync(id);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
            return NotFound($"Cannot find the user id {id} to create the post");
        }
        Post newPost = new(request.Title, request.Body, id)
        {
            Title = request.Title,
            Body = request.Body,
            UserId = id
        };
        await _postRepository.AddAsync(newPost);
        return Ok(newPost);
    }

    [HttpPut("{id}")] //this cover the use where client want to update both title and either of them
    public async Task<ActionResult> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post? existingPost = await _postRepository.GetSingleAsync(id);
            Post editedPost = new(existingPost.Id, request.Title, request.Body, existingPost.UserId)
            {
                Id = existingPost.Id,
                Title = request.Title,
                Body = request.Body,
                UserId = existingPost.UserId,
            };
            if (editedPost.Title == string.Empty)
            {
                editedPost.Title = existingPost.Title; //same as post.set(existingPost) in Java
            }
            if (editedPost.Body == string.Empty)
            {
                editedPost.Body = existingPost.Body;
            }
            await _postRepository.UpdateAsync(editedPost);
            return Ok(editedPost);
        }
        // since GetSingleAsync in FileRepository will throw InvalidOperationException if not found
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex);
            return NotFound(ex.Message);
        }
        // include unexpected exception
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
       try
       {
            Post? existingPost = await _postRepository.GetSingleAsync(id);
            if (existingPost is null)
            {
                return NotFound($"Cannot find the post id {id}");
            }
            await _postRepository.DeleteAsync(id);
            return Ok($"Deleted the post id {id}");
       }
       catch (InvalidOperationException ex)
       {
          Console.WriteLine(ex);
          return NotFound(ex.Message);
       }
       catch (Exception ex)
       {
          Console.WriteLine(ex);
          return StatusCode(500);
       }
    }
}
