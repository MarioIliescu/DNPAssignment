namespace Entities;

public class User
{
    public string Username { get; set; } = "DefaultUsername";
    public string Password { get; set; } = "DefaultPassword";
    public int Id { get; set; }
    public override string ToString()
    {
        return $"Username: {Username} Id: {Id}";
    }
}