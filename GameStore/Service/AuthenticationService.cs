using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using LoggingService;
using Microsoft.IdentityModel.Tokens;
using Shared.DataTransferObjects.ForAuth;
using Shared.DataTransferObjects.ForShow;

namespace Service
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        #region Properties

        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        private readonly IConfiguration _configuration;
        // private readonly IEmailSender _emailSender;
        private User? _user;

        #endregion

        #region Ctor

        public AuthenticationService(ILoggerManager logger, IMapper mapper,
            UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        #endregion

        //
        // #region VerifyGoogleToken
        //
        // public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth)
        // {
        //     var googleSettings = _configuration.GetSection("GoogleAuthSettings");
        //     var clientId = googleSettings.GetSection("clientId").Value;
        //     var settings = new GoogleJsonWebSignature.ValidationSettings()
        //     {
        //         Audience = new List<string>() { clientId }
        //     };
        //
        //     var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
        //     return payload;
        // }

        // #endregion

        #region Register User

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);


            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Authenticated");
                // await GenerateEmailConfirmationToken(user, userForRegistration);
            }


            return result;
        }

        #endregion

        #region Validate User
        
        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            _user = await _userManager.FindByNameAsync(userForAuth.UserName);
            // _user = await _userManager.FindByEmailAsync(userForAuth.UserName);
        
            if (_user == null) return false;
        
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
        
        
            // if (!result)
            // {
            //     await ImplementLockOutLogin(userForAuth);
            // }
            // else
            // {
            //     await _userManager.ResetAccessFailedCountAsync(_user);
            // }
        
            return result;
        }
        
        #endregion

        //
        // #region Lock Out Logic
        //
        // private async Task ImplementLockOutLogin(UserForAuthenticationDto userForAuthentication)
        // {
        //     await _userManager.AccessFailedAsync(_user);
        //     if (await _userManager.IsLockedOutAsync(_user))
        //     {
        //         var content =
        //             $"Your account is locked out. To reset the password click this link: {userForAuthentication.ClientURL}";
        //         var message = new Message(new string[] { userForAuthentication.UserName },
        //             "Locked out account information", content, null);
        //
        //         await _emailSender.SendEmailAsync(message);
        //     }
        // }
        //
        // #region Is User Lock Out
        //
        // public async Task<bool> IsUserLockOut(string email)
        // {
        //     _user = await _userManager.FindByEmailAsync(email);
        //     return await _userManager.IsLockedOutAsync(_user);
        // }
        //
        // #endregion
        //
        // public async Task SetLockoutEndDateAsync(string email)
        // {
        //     _user = await _userManager.FindByEmailAsync(email);
        //     await _userManager.SetLockoutEndDateAsync(_user, new DateTime(2000, 1, 1));
        // }
        //
        // #endregion

        // #region Is UserName Confirmed
        //
        // public async Task<bool> IsEmailConfirmed(UserForAuthenticationDto userForAuthentication)
        // {
        //     var user = await _userManager.FindByEmailAsync(userForAuthentication.UserName);
        //     var response = await _userManager.IsEmailConfirmedAsync(user);
        //     return response;
        // }
        //
        // #endregion

        #region Validate Email

        public async Task<bool> ValidateEmail(string email) => await _userManager.FindByEmailAsync(email) != null;

        #endregion

        #region Get Auth User
        
        public async Task<UserDto> GetAuthUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var mappedUser = _mapper.Map<UserDto>(user);
            mappedUser.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            return mappedUser;
        }
        
        #endregion

        #region Create Token
        
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        
            _logger.LogInfo($"Creating Token (CreateToken)");
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        
        
        
        private SigningCredentials GetSigningCredentials()
        {
            // var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var securityKey = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(securityKey.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
        
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                // new Claim(ClaimTypes.Name, _user.Email)
               new Claim(ClaimTypes.Name, _user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);
        
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        
            return claims;
        }
        
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
        
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials
            );
        
            return tokenOptions;
        }
        
        #endregion

        //
        // #region Send Restore Link To UserName
        //
        // public async Task<bool> SendRestoreLinkToEmail(ForgotPasswordDto forgotPasswordDto)
        // {
        //     var user = await _userManager.FindByEmailAsync(forgotPasswordDto.UserName);
        //     if (user == null)
        //     {
        //         return false;
        //     }
        //
        //
        //     var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //     var param = new Dictionary<string, string>
        //     {
        //         { "token", token },
        //         { "email", forgotPasswordDto.UserName }
        //     };
        //     var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientUrl, param);
        //     var message = new Message(new string[] { user.UserName }, "Reset password token", callback, null);
        //     await _emailSender.SendEmailAsync(message);
        //     return true;
        // }
        //
        // #endregion
        //
        // #region Reset Password
        //
        // public async Task<IdentityResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        // {
        //     var user = await _userManager.FindByEmailAsync(resetPasswordDto.UserName);
        //     return await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
        // }
        //
        // #endregion

        #region UserName Confirmation

        public async Task<IdentityResult> EmailConfirmation(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result;
        }

        #endregion

        // #region Generate UserName Confirmation Token
        //
        // private async Task GenerateEmailConfirmationToken(User user, UserForRegistrationDto userForRegistration = null)
        // {
        //     var url = userForRegistration == null
        //         ? "http://localhost:5000/authentication/emailconfirmation"
        //         : userForRegistration.ClientURL;
        //
        //     var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //     var param = new Dictionary<string, string>
        //     {
        //         { "token", token },
        //         { "email", user.UserName }
        //     };
        //     var callback = QueryHelpers.AddQueryString(url, param);
        //     var message = new Message(new string[] { user.UserName }, "UserName Confirmation token", callback, null);
        //     await _emailSender.SendEmailAsync(message);
        // }
        //
        // #endregion

        //
        // #region Two factor validation
        //
        // public async Task<bool> IsEmailInTwoFactorProvidersAsync(UserForAuthenticationDto userForAuthentication)
        // {
        //     _user = await _userManager.FindByEmailAsync(userForAuthentication.UserName);
        //     var providers = await _userManager.GetValidTwoFactorProvidersAsync(_user);
        //     return providers.Contains("UserName");
        // }
        //
        // public async Task<bool> GetTwoFactorEnabledAsync(UserForAuthenticationDto userForAuthentication)
        // {
        //     _user = await _userManager.FindByEmailAsync(userForAuthentication.UserName);
        //     return await _userManager.GetTwoFactorEnabledAsync(_user);
        // }
        //
        // public async Task GenerateOTPFor2StepVerification(UserForAuthenticationDto userForAuthentication)
        // {
        //     var token = await _userManager.GenerateTwoFactorTokenAsync(_user, "UserName");
        //     var message = new Message(new string[] { _user.UserName }, "Authentication token", token, null);
        //     await _emailSender.SendEmailAsync(message);
        // }
        //
        // public async Task<bool> VerifyTwoFactorToken(TwoFactorDto twoFactorDto)
        // {
        //     _user = await _userManager.FindByEmailAsync(twoFactorDto.UserName);
        //     var response =
        //         await _userManager.VerifyTwoFactorTokenAsync(_user, twoFactorDto.Provider, twoFactorDto.Token);
        //     return response;
        // }
        //
        // #endregion
        //
        // #region External login
        //
        // public async Task<bool> ExternalLogin(ExternalAuthDto externalAuth, GoogleJsonWebSignature.Payload payload)
        // {
        //     var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);
        //
        //     _user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        //     if (_user == null)
        //     {
        //         _user = await _userManager.FindByEmailAsync(payload.UserName);
        //
        //         if (_user == null)
        //         {
        //             _user = new User
        //             {
        //                 UserName = payload.UserName, UserName = payload.UserName, FirstName = payload.GivenName,
        //                 LastName = payload.FamilyName, EmailConfirmed = payload.EmailVerified
        //             };
        //             await _userManager.CreateAsync(_user);
        //
        //             if (!payload.EmailVerified)
        //             {
        //                 await GenerateEmailConfirmationToken(_user);
        //             }
        //
        //             await _userManager.AddToRoleAsync(_user, "User");
        //         }
        //
        //         await _userManager.AddLoginAsync(_user, info);
        //     }
        //
        //     return _user == null;
        //
        //     #region Comments
        //
        //     // In our database, we can have three different situations.
        //     // The user that tries to log in doesn’t exist at all,
        //     // the user exists but without external login information,
        //     // and the user exists with the external login information.
        //
        //     #endregion
        // }
        //
        // #endregion
    }
}