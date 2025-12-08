using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsMessagesMemoryController : ControllerBase
    {
        NieuwsberichtRepository repository;

        public NewsMessagesMemoryController(NieuwsberichtRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/<NewsMessagesMemoryController>
        [HttpGet]
        public IEnumerable<Nieuwsbericht> Get()
        {
            return repository.Messages;
        }

        // GET api/<NewsMessagesMemoryController>/5
        [HttpGet("{id}")]
        public ActionResult<Nieuwsbericht> Get(int id)
        {
            if (repository.IdExists(id))
            {
                return repository[id];
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<NewsMessagesMemoryController>
        [HttpPost]
        public ActionResult<Nieuwsbericht> Post([FromBody] Nieuwsbericht bericht)
        {
            Nieuwsbericht nieuwbericht = repository.Add(bericht);
            return CreatedAtAction("Get", new { id = nieuwbericht.Id }, nieuwbericht);
        }

        // PUT api/<NewsMessagesMemoryController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Nieuwsbericht nieuwsbericht)
        {
            if (nieuwsbericht.Id == id)
            {
                repository.Update(nieuwsbericht);
                return NoContent();
            }

            else
            {
                // Use methods of ControllerBase to return status to client
                return BadRequest();
            }
        }

        // DELETE api/<NewsMessagesMemoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.IdExists(id))
            {
                repository.Delete(id);
                // Use methods of ControllerBase to return status to client
                return NoContent();
            }
            else
            {
                // Use methods of ControllerBase to return status to client
                return NotFound();
            }

        }
    }
}
