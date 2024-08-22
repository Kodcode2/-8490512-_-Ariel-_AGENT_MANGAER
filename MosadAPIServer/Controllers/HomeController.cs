using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MosadAPIServer.Services;

namespace MosadAPIServer.Controllers
{
    public class HomeController : Controller
    {

        private readonly TokenService _tokenService;

        public HomeController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // POST: api/T
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] string id)
        {
            try
            {
                var token = _tokenService.GenerateToken(id);
                return Ok(new { token=token });

            }catch(InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
