namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Models;
using WebApi.Services;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
    {
        var user = await _userService.Authenticate(model.Username, model.Password);

        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(user);

    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserModel model)
    {
        var user = await _userService.Create(model);

        if (user == null)
            return BadRequest(new { message = "This username is already taken, lplease create a new one" });

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);

    }
    //get user by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = (await _userService.GetAll()).FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    //get all users
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }


    //update
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserModel model)
    {
        var user = await _userService.Update(id, model);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var delete = await _userService.Delete(id);

        if (delete == null)
            return NotFound();

        return NoContent();


    }

}
