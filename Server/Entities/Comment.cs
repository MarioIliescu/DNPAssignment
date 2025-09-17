namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public int PostId{ get; set; }
    public string Body{ get; set; } = "Default Body";
    public int UserId{ get; set; }

    public override string ToString()
    {
        return $"Id: {Id} - PostId: {PostId} - UserId: {UserId}\n  Body: {Body}";
    }
}