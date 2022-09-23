using System.Threading.Tasks;
// using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.ForAuth;
using Shared.DataTransferObjects.ForShow;

// using Shared.DataTransferObjects.ForShow;


namespace Service.Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        // Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
        Task<bool> ValidateEmail(string email);
        // Task<bool> SendRestoreLinkToEmail(ForgotPasswordDto forgotPasswordDto);
        Task<string> CreateToken();
         Task<UserDto> GetAuthUser(string id);

        // Task<bool> IsEmailConfirmed(UserForAuthenticationDto userForAuthentication);
        // Task<bool> IsUserLockOut(string email);
        // Task SetLockoutEndDateAsync(string email);
        // Task<IdentityResult> EmailConfirmation(string email, string token);


        // #region Google Authentication
        //
        // Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth);
        // Task<bool> ExternalLogin(ExternalAuthDto externalAuth, GoogleJsonWebSignature.Payload payload);
        //
        //     #endregion
        //
        // #region Two factor validation
        //
        // Task<bool> IsEmailInTwoFactorProvidersAsync(UserForAuthenticationDto userForAuthentication);
        // Task<bool> GetTwoFactorEnabledAsync(UserForAuthenticationDto userForAuthentication);
        // Task GenerateOTPFor2StepVerification(UserForAuthenticationDto userForAuthentication);
        //
        // Task<bool> VerifyTwoFactorToken(TwoFactorDto twoFactorDto);
        //
        // #endregion


    }
}