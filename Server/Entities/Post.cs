namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title{ get; set; } = "Default Title";
    public string Body{ get; set; } = "Default Body";
    public int UserId{ get; set; }

    public override string ToString()
    {
        return $"Post\n" +
               $"Id: {Id}, Title: {Title}, UserId: {UserId}";
    }

    public string FullPostToString()
    {
        return $"Post\n" +
               $"Id: {Id}, Title: {Title}, UserId: {UserId}\n" +
               $"Body: {Body}";;
    }
}