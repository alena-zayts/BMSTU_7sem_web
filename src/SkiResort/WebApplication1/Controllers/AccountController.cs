using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication1.Models; 
using WebApplication1.Options;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class accountController : Controller
    {

        private BL.Facade _facade;
        public accountController()
        {
            
        }

        // POST: account
        /// <summary>
        /// Log in
        /// </summary>
        /// <param name="userEmail">User's email</param>
        /// <param name="userPassword">User's password</param>
        /// <returns>Token</returns>
        /// <response code="200" cref="JsonResult">Authorization went successfully</response>
        /// <response code="404">User with such email wasn't found</response>
        /// <response code="401">Incorrect password</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LogInAsync([FromQuery] string userEmail, [FromQuery] string userPassword)
        {
            _facade = OtherOptions.createFacade();
            try
            {
                BL.Models.User foundUser = await _facade.LogInAsync(userEmail, userPassword);
                UserAccount userAccount = Converters.UserAccountConverter.ConvertUserToUserAccountDTO(foundUser);

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userAccount.UserEmail),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userAccount.Role)
                };

                ClaimsIdentity identity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);


                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                jwt.Payload["userID"] = foundUser.UserID;
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                };

                return Json(response);

            }
            catch (BL.Exceptions.UserExceptions.UserAuthorizationException ex)
            {
                string errorMessage = JsonSerializer.Serialize("Incorrect password", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
            catch (AccessToDB.Exceptions.UserExceptions.UserNotFoundException ex)
            {
                string errorMessage = JsonSerializer.Serialize("User with such email wasn't found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
        }

        // Put: account
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="userEmail">User's email</param>
        /// <param name="userPassword">User's password</param>
        /// <returns>Token</returns>
        /// <response code="200" cref="JsonResult">Registration went successfully</response>
        /// <response code="401">User with such email already exists</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Register([FromQuery] string userEmail, [FromQuery] string userPassword)
        {
            try
            {
                _facade = OtherOptions.createFacade();
                BL.Models.User newUser = await _facade.RegisterAsync(0, userEmail, userPassword);
                UserAccount userAccount = Converters.UserAccountConverter.ConvertUserToUserAccountDTO(newUser);

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userAccount.UserEmail),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userAccount.Role)     
                };

                ClaimsIdentity identity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);


                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                jwt.Payload["userID"] = newUser.UserID;
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                };

                return Json(response);
            }
            catch (BL.Exceptions.UserExceptions.UserRegistrationException ex)
            {
                string errorMessage = JsonSerializer.Serialize("User with such email already exists", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
        }

        // Get: account
        /// <summary>
        /// Continue as anauthorized (just get token)
        /// </summary>
        /// <returns>Token</returns>
        /// <response code="200" cref="JsonResult">Ok</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        public async Task<IActionResult> ContinueWithoutAccount()
        {
            _facade = OtherOptions.createFacade();
            BL.Models.User newUser = await _facade.LogInAsUnauthorizedAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "unathurozied")
            };

            ClaimsIdentity identity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);


            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            jwt.Payload["userID"] = newUser.UserID;
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
            };

            return Json(response);
        }

        // DELETE: account
        /// <summary>
        /// Log out (and get new token to be interpreted as an unauthorized user)
        /// </summary>
        /// <returns>New token for unauthorized user </returns>
        /// <response code="200" cref="JsonResult">User was successfully logged out</response>
        /// <response code="401">User was not logged in</response>
        [Authorize(Roles = "admin, authorized, ski_patrol")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete()
        {
            return await ContinueWithoutAccount();
        }
    }
}