using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Cache;
using System.ComponentModel.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request){
            
            var response = await _authRepository.Register(
                new User{UserName = request.UserName}, 
                request.Password
            );

            if(!response.Success){
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login (UserLoginDto request){
            
            var response = await _authRepository.Login(
                request.UserName, 
                request.Password
            );

            if(!response.Success){
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}