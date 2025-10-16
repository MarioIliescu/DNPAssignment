namespace ApiContracts.DTOs;

public record UpdateUserDto(int Id, string Password, string? NewPassword);