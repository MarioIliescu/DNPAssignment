namespace ApiContracts.DTOs;

public record PostWithCommentsDto(PostDto Post, List<CommentDto> Comments);