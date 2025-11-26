using Microsoft.AspNetCore.Mvc;
using Entities;
using System.Threading.Tasks;
using ApiContracts.Dto.CommentDto;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")] // means the route is /comments according to the controller class name (exclude "Controller" part)
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public CommentsController(ICommentRepository commentRepository, IPostRepository postRepository, 
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    //TODO : Implement CRUD operations for Comment entity
    // get (both all given post id): link should be posts/{postId}/comments
    [HttpGet("/posts/{id}/comments")] // The leading / in the route makes it absolute
                                      // , so it will not be prefixed with /comments
    public async Task<ActionResult<IQueryable<Comment>>> GetAllCommentsByPostId(int id)
    {
        var givenPost = await _postRepository.GetSingleAsync(id);
        if (givenPost is null)
        {
            return NotFound($"Cannot find the post id {id} to get the comments");
        }
        // don't forget that c# has LINQ to filter the IQueryable with where clause
        var comments = _commentRepository.GetManyAsync().Where(c => c.PostId == id);
        return Ok(comments);
    }

    [HttpGet("/posts/{postId}/comments/{commentId}")]
    public async Task<ActionResult<Comment>> GetCommentById(int postId, int commentId)
    {
        try
        {
            var givenPost = await _postRepository.GetSingleAsync(postId);
            if (givenPost is null)
            {
                return NotFound($"Cannot find the post id {postId} to get the comment");
            }
            var comment = await _commentRepository.GetSingleAsync(commentId);
            if (comment.PostId != postId)
            {
                return BadRequest($"The comment id {commentId} does not belong to the post id {postId}");
            }
            return Ok(comment);
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

    // create (post)
    [HttpPost("posts/{postId}/users/{userId}")]
    public async Task<ActionResult<Comment>> Post(int postId, int userId, [FromBody] DetailedCommentDto request)
    {
        try
        {
            var givenPost = await _postRepository.GetSingleAsync(postId);
            if (givenPost is null)
            {
                return NotFound($"Cannot find the post id {postId} to create the comment");
            }
            var givenUser = await _userRepository.GetSingleAsync(userId);
            if (givenUser is null)
            {
                return NotFound($"Cannot find the user id {userId} to create the comment");
            }
            Comment newComment = new(request.Body, postId, userId)
            {
                PostId = postId,
                Body = request.Body,
                UserId = userId
            };
            Comment createdComment = await _commentRepository.AddAsync(newComment);
            return Created($"/comments/posts/{postId}/{createdComment.Id}", createdComment);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500);
        }
    }

    //update (put) : only body can be updated for comment so no patch is needed
    [HttpPut("/posts/{postId}/comments/{commentId}")] 
    public async Task<ActionResult<Comment>> UpdateComment(int postId, int commentId
        , [FromBody] CommentDto request)
    {
        try
        {
            var givenPost = await _postRepository.GetSingleAsync(postId);
            if (givenPost is null)
            {
                return NotFound($"Cannot find the post id {postId} to update the comment");
            }
            var commentToUpdate = await _commentRepository.GetSingleAsync(commentId);
            if (commentToUpdate.PostId != postId)
            {
                return BadRequest($"The comment id {commentId} does not belong to the post id {postId}");
            }
            commentToUpdate.Body = request.Body;
            await _commentRepository.UpdateAsync(commentToUpdate);
            return Ok();
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

    [HttpDelete("/posts/{postId}/comments/{commentId}")]
    public async Task<ActionResult> DeleteComment(int postId, int commentId)
    {
        try
        {
            var givenPost = await _postRepository.GetSingleAsync(postId);
            if (givenPost is null)
            {
                return NotFound($"Cannot find the post id {postId} to delete the comment");
            }
            var commentToDelete = await _commentRepository.GetSingleAsync(commentId);
            if (commentToDelete.PostId != postId)
            {
                return BadRequest($"The comment id {commentId} does not belong to the post id {postId}");
            }
            await _commentRepository.DeleteAsync(commentId);
            return Ok();
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
