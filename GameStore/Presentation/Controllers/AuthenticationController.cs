using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTransferObjects.ForAuth;

namespace Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;


        #region Ctor

        public AuthenticationController(IServiceManager service)
        {
            _service = service;
        }

        #endregion

        #region Registration

        [HttpPost("registration")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _service.AuthenticationService.RegisterUser(userForRegistration);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
            
                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            return StatusCode(201);
        }

        #endregion

        #region Login
        
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _service.AuthenticationService.ValidateUser(user))
                return Unauthorized();
        
            // if (!await _service.AuthenticationService.IsEmailConfirmed(user))
            //     return Unauthorized(new AuthResponseDto { ErrorMessage = "UserName is not confirmed" });
        
            // if (await _service.AuthenticationService.IsUserLockOut(user.UserName))
            // {
            //     return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
            // }
        
            #region Check two factor validation
        
            // if (await _service.AuthenticationService.GetTwoFactorEnabledAsync(user))
            // {
            //     if (!await _service.AuthenticationService.IsEmailInTwoFactorProvidersAsync(user))
            //     {
            //         return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid 2-Step Verification Provider." });
            //     }
            //
            //     await _service.AuthenticationService.GenerateOTPFor2StepVerification(user);
            //     return Ok(new AuthResponseDto { Is2StepVerificationRequired = true, Provider = "UserName" });
            // }
        
            #endregion
        
            var token = await _service
                .AuthenticationService.CreateToken();
            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }
        
        #endregion

        #region Privacy

        [HttpGet("privacy")]
        // [Authorize]
        [Authorize(Roles = "Manager")]
        public IActionResult Privacy()
        {
            var claims = User.Claims
                .Select(c => new { c.Type, c.Value })
                .ToList();
            return Ok(claims);
        }

        #endregion

        #region Get Auth User
        
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetAuthUser()
        {
            var userName = GetIdFromToken();
            var result = await _service.AuthenticationService.GetAuthUser(userName);
        
            return Ok(result);
        }
        
        private string GetIdFromToken()
        {
            var identity = User.Identity as ClaimsIdentity;
            var userName = "";
            if (identity != null)
            {
                var claims = identity.Claims;
               // id = claims.FirstOrDefault(p => p.Type == "id")?.Value;
               userName = claims.FirstOrDefault(p => p.Type.Contains("name"))?.Value;
               
            }
        
            return userName;
        }
        
        #endregion
        
        
         #region CheckEmail
        
         [HttpPost("checkEmail")]
         public async Task<IActionResult> CheckEmail([FromBody] CheckEmail email)
         {
             var result = await _service.AuthenticationService.ValidateEmail(email.Email);
        
             return Ok(result);
         }
        
         #endregion
        
        // #region Forgot Password
        //
        // [HttpPost("forgotPassword")]
        // [ServiceFilter(typeof(ValidationFilterAttribute))]
        // public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        // {
        //     var response = await _service.AuthenticationService.SendRestoreLinkToEmail(forgotPasswordDto);
        //     if (!response)
        //         return BadRequest("Invalid Request.");
        //
        //
        //     return Ok();
        // }
        //
        // #endregion
        //
        // #region Reset Password
        //
        // [HttpPost("ResetPassword")]
        // [ServiceFilter(typeof(ValidationFilterAttribute))]
        // public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        // {
        //     var resetPassResult = await _service.AuthenticationService.ResetPassword(resetPasswordDto);
        //
        //     if (!resetPassResult.Succeeded)
        //     {
        //         var errors = resetPassResult.Errors.Select(e => e.Description);
        //
        //         return BadRequest(new { Errors = errors });
        //     }
        //
        //     await _service.AuthenticationService.SetLockoutEndDateAsync(resetPasswordDto.UserName);
        //
        //     return Ok();
        // }
        //
        // #endregion
        //
        // #region UserName Confirmation
        //
        // [HttpGet("EmailConfirmation")]
        // public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        // {
        //     if (!await _service.AuthenticationService.ValidateEmail(email))
        //         return BadRequest("Invalid UserName Confirmation Request");
        //
        //     var confirmResult = await _service.AuthenticationService.EmailConfirmation(email, token);
        //     if (!confirmResult.Succeeded)
        //         return BadRequest("Invalid UserName Confirmation Request");
        //
        //     return Ok();
        // }
        //
        // #endregion
        //
        // #region Two Step Verification
        //
        // [HttpPost("TwoStepVerification")]
        // [ServiceFilter(typeof(ValidationFilterAttribute))]
        // public async Task<IActionResult> TwoStepVerification([FromBody]TwoFactorDto twoFactorDto)
        // {
        //     var response=  await _service.AuthenticationService.ValidateEmail(twoFactorDto.UserName);
        //     
        //     if (!response)
        //         return BadRequest("Invalid Request");
        //
        //     if (!await _service.AuthenticationService.VerifyTwoFactorToken(twoFactorDto))
        //         return BadRequest("Invalid Token Verification");
        //
        //     var token = await _service.AuthenticationService.CreateToken();
        //     return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        // }
        //
        // #endregion
        //
        //
        // #region External Login
        //
        // [HttpPost("ExternalLogin")]
        // [ServiceFilter(typeof(ValidationFilterAttribute))]
        // public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
        // {
        //     var payload = await _service.AuthenticationService.VerifyGoogleToken(externalAuth);
        //     if(payload == null)
        //         return BadRequest("Invalid External Authentication. Payload is null.");
        //
        //
        //     if (await _service.AuthenticationService.ExternalLogin(externalAuth,payload))
        //         return BadRequest("Invalid External Authentication.");
        //
        //     //check for the Locked out account
        //     if (await _service.AuthenticationService.IsUserLockOut(payload.UserName))
        //     {
        //         return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
        //     }
        //
        //     var token = await _service
        //         .AuthenticationService.CreateToken();
        //     return Ok(new AuthResponseDto { Token = token, IsAuthSuccessful = true });
        // }
        //
        //
        // #endregion
    }
}