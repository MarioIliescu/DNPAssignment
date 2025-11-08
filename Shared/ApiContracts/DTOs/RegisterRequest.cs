namespace ApiContracts.DTOs;

public record RegisterRequest(string Username, string Password, string ConfirmPassword);