namespace ApiContracts.DTOs;

public record CommentDto(int Id, string Body, int UserId, int PostId);