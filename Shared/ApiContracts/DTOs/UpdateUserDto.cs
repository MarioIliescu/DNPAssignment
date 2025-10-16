namespace ApiContracts.DTOs;

public record UpdateUserDto(int Id, string Username, string Password, string? NewPassword);