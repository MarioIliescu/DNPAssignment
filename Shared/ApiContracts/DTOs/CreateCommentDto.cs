namespace ApiContracts.DTOs;

public record CreateCommentDto(int PostId, string Body,  int UserId);