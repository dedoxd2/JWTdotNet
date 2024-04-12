using Microsoft.AspNetCore.Http;
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

                return Ok(new
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Token = result.Token,
                    ExpiresOn = result.ExpiresOn
                }); // Return Custom Data

            }
            return BadRequest(result.Message);

        }


        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody]TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.GetToken(model);
              
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            

            return Ok(result);
        }
    }
}
