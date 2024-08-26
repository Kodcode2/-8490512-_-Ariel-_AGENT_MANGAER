using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MosadAPIServer.DTO;
using MosadAPIServer.Exceptions;
using MosadAPIServer.Services;

namespace MosadAPIServer.Controllers
{
    public class HomeController : Controller
    {


        public HomeController(TokenService tokenService) { }

        // POST: api/T
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] IdForTokenDTO idDTO)
        {
            try
            {
                var token = TokenService.GenerateToken(idDTO.Id);
                return Ok(new { token });

            }catch(UnauthorizedIdException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
