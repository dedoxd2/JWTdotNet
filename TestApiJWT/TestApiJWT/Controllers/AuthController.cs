﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestApiJWT.Models;
using TestApiJWT.Services;

namespace TestApiJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

     
        
        
        
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            var result = await _authService.RegisterAsync(model);



            if (result.IsAuthenticated)
            {
                //   return Ok(result);
                SetRefreshTokenInCookie(result.Token, result.RefreshTokenExpiration);

                return Ok(new
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Token = result.Token,
               //     ExpiresOn = result.ExpiresOn
                }); // Return Custom Data

            }
            return BadRequest(result.Message);

        }


        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody]TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.GetTokenAsync(model);
              
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.Token,result.RefreshTokenExpiration);
            

            return Ok(result);
        }



        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);


            return Ok(model);
        }


        private void SetRefreshTokenInCookie(string refreshToken , DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken" , refreshToken , cookieOptions);
        }



        [HttpGet("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }


        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)    
        {

            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required");

            var result =   await  _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is Invalid");


            return Ok();

        }

    }
}
