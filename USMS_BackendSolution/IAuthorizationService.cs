namespace Author.Services
{
    public interface IAuthorizationService
    {
        Task<APIResponse> ValidateUserRole(string[] allowedRoles);
        string? GetCurrentUserId();
    }
}