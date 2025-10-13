namespace ApiContracts.DTOs;

public record UpdatePostDto (int Id, string Title, string Body , int UserId)
{
}