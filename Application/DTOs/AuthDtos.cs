using System;

namespace Application.DTOs;

public record RegisterRequest(string Email, string Password, string Name, string Phone);

public record LoginRequest(string Email, string Password);

public record UserAuthDto(Guid Id, string Email);

public record TokensDto(
    string AccessToken,
    DateTimeOffset AccessExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshExpiresAt);

public record LoginResponse(UserAuthDto User, TokensDto Tokens);

public record RegisterResponse(UserAuthDto User, TokensDto Tokens);

public record RefreshRequest(string RefreshToken);

public record RefreshResponse(
    string AccessToken,
    DateTimeOffset AccessExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshExpiresAt);

public record LogoutRequest(string RefreshToken);