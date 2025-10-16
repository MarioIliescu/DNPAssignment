namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public int PostId{ get; set; }
    public string Body{ get; set; } = "Default Body";
    public int UserId{ get; set; }

    public override string ToString()
    {
        return $"UserId: {UserId}  Body: {Body}";
    }
}