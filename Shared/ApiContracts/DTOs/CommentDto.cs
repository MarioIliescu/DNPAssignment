namespace ApiContracts.DTOs;

public record CommentDto(int Id, string Body, string Username, int PostId);