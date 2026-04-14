namespace NoName.Shared.Contracts.Authentication;

public record AuthenticatedResponse(string AccessToken, string RefreshToken);