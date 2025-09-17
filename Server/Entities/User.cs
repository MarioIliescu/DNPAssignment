namespace Entities;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int Id { get; set; }
    public override string ToString()
    {
        return $"Username: {Username} Password: {Password} Id: {Id}";
    }
}