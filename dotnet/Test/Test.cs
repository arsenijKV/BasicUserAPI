using NUnit.Framework;
using WebApi.Services;
using WebApi.Models;

namespace WebApi.Tests;

[TestFixture]
public class UserServiceTests
{
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userService = new UserService();
    }

    [Test]
    public async Task Authenticate_WithValidCredentials_ReturnsUser()
    {
        var user = await _userService.Authenticate("test", "test");
        Assert.IsNotNull(user);
        Assert.AreEqual("test", user.Username);
    }

    [Test]
    public async Task Authenticate_WithInvalidCredentials_ReturnsNull()
    {
        var user = await _userService.Authenticate("wrong", "password");
        Assert.IsNull(user);
    }

    [Test]
    public async Task Create_NewUser_ReturnsCreatedUser()
    {
        var model = new CreateUserModel
        {
            FirstName = "Anna",
            LastName = "Smith",
            Username = "anna",
            Password = "1234"
        };

        var user = await _userService.Create(model);
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Username, Is.EqualTo("anna"));
    }

    [Test]
    public async Task GetAll_ReturnsAllUsers()
    {
        var users = await _userService.GetAll();
        Assert.That(users.Count(), Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public async Task Update_ExistingUser_ChangesUserData()
    {
        var model = new UpdateUserModel
        {
            FirstName = "Updated",
            LastName = "User",
            Username = "test",
            Password = "test"
        };

        var user = await _userService.Update(1, model);

        Assert.That(user, Is.Not.Null);
        Assert.That(user.FirstName, Is.EqualTo("Updated"));
    }

    [Test]
    public async Task Delete_ExistingUser_RemovesUser()
    {
        var deletedUser = await _userService.Delete(1);
        Assert.That(deletedUser, Is.Not.Null);

        var result = await _userService.GetAll();
        Assert.That(result.Any(u => u.Id == 1), Is.False);
    }
}

