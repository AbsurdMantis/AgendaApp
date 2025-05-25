using AgendaApp.Server.DTOs;
using AgendaApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // dps configurar com jwt
    public class ContatosController : ControllerBase
    {
        private readonly IContatoService _service;

        public ContatosController(IContatoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContatoDTO>>> GetContatos()
        {
            var contatos = await _service.GetContatosAsync();

            return Ok(contatos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContatoDTO>> GetContato(int id)
        {
            var contato = await _service.GetContatoAsync(id);
            if (contato == null)
            {
                return NotFound();
            }

            return Ok(contato);
        }

        [HttpPost]
        public async Task<ActionResult<ContatoDTO>> CreateContato(ContatoDTO newContato)
        {
            var contato = await _service.AddContatoAsync(newContato);

            return CreatedAtAction(nameof(GetContato), new { id = contato.Id }, contato);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ContatoDTO>> UpdateContato(int id, ContatoDTO editContato)
        {
            var contato = await _service.UpdateContatoAsync(id, editContato);
            if (contato == null)
            {
                return NotFound();
            }

            return Ok(contato);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            var remove = await _service.RemoveContatoAsync(id);

            if (!remove)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
