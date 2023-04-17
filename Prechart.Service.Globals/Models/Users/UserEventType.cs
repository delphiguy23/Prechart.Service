namespace Prechart.Service.AuditLog.Models.Users;

public enum UserEventType
{
    None = 0,
    UserCreated = 1,
    UserUpdated = 2,
    UserDeleted = 3,
    UserPasswordChanged = 4,
    UserLockoutChanged = 5,
    UserTwoFactorChanged = 6,
    UserRoleAdded = 7,
    UserRoleRemoved = 8,
    UserClaimAdded = 9,
    UserClaimRemoved = 10,
    UserLogin = 11,
    UserLogout = 12,
    UserLockout = 13,
    UserTwoFactor = 14,
    UserPasswordReset = 15,
    UserPasswordResetFailed = 16,
    UserPasswordResetSuccess = 17,
    UserPasswordResetTokenExpired = 18,
    UserPasswordResetTokenInvalid = 19,
    UserPasswordResetTokenValid = 20,
    UserPasswordResetTokenGenerated = 21,
    UserPasswordResetTokenGenerationFailed = 22,
    UserPasswordResetTokenGenerationSuccess = 23,
}