namespace WebApi.Services;
using WebApi.Models;
using WebApi.Entities;

public interface IUserService
{
    Task<User> Authenticate(string username, string password);
    Task<IEnumerable<User>> GetAll();
    Task<User> Create(CreateUserModel model);
    Task<User> Update(int id, UpdateUserModel model);
    Task<User> Delete(int id);
}

public class UserService : IUserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private List<User> _users = new List<User>
    {
        new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
    };

    public async Task<User> Authenticate(string username, string password)
    {
        // wrapped in "await Task.Run" to mimic fetching user from a db
        var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

        // on auth fail: null is returned because user is not found
        // on auth success: user object is returned
        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        // wrapped in "await Task.Run" to mimic fetching users from a db
        return await Task.Run(() => _users);
    }

    public async Task<User> Create(CreateUserModel model)
    {
        if (_users.Any(x => x.Username == model.Username))
            return null;

        var user = new User
        {
            Id = _users.Max(x => x.Id) + 1,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Username = model.Username,
            Password = model.Password
        };

        _users.Add(user);

        return await Task.FromResult(user);


    }
    public async Task<User> Update(int id, UpdateUserModel model)
    {
        var user = _users.FirstOrDefault(x => x.Id == id);

        if (user == null)
            return null;


        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Username = model.Username;
        user.Password = model.Password;
        

        return await Task.FromResult(user);
    }

    public async Task<User> Delete(int id)
    {
        var user = _users.FirstOrDefault(x => x.Id == id);

        if (user == null)
            return null;

        _users.Remove(user);

        return await Task.FromResult(user);

    }


}
