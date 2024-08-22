using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.Services;
using MosadAPIServer.ModelsHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MosadAPIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class CruController<T,DTO>  : ControllerBase where DTO  :IDTOModel where T : IModel
    {
        private readonly MosadAPIServerContext _context;
        private readonly ICruService<T,DTO> _ModelService;

        public CruController(MosadAPIServerContext context, ICruService<T, DTO> service)
        {
            _context = context;
            _ModelService = service;
        }

        // GET: api/T
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            return await _ModelService.GetAllAsync();
        }




        // POST: api/T
        [HttpPost]
        public async Task<ActionResult> PostAgent([FromBody] DTO dtomodel)
        {
            int id = await _ModelService.CreateAsync(dtomodel);
            return Created(nameof(PostAgent), new { id = id });
        }

        // PUT: api/T/5/pin
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> PutAgent(int id, Location pinLocation)
        {
            if (id == null) return BadRequest("wrong id");
            try
            {
                await _ModelService.PinLocatinAsync(id, pinLocation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ModelService.IsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (NullReferenceException ex)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // PUT: api/T/5/move
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveAgent(int id, [FromBody] string direction)
        {
            try
            {
                await _ModelService.MoveAsync(id, direction);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ModelService.IsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (NullReferenceException ex)
            {
                return NotFound();
            }

            return NoContent();
        }


       
    }
}

