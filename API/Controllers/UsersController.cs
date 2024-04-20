using Microsoft.AspNetCore.Mvc;
using TestsService.AppCore.Respositories.Interfaces;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TestsService.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRespository _userRespository;
        private readonly ILogger<UsersController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsersController(ILogger<UsersController> logger, 
            IUserRespository userRespository,
            IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _userRespository = userRespository;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _userRespository.GetUsers());
        }
    }
}