using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeSprint.Controllers
{
    [Route("api/Generation")]
    [ApiController]
    public class GenerationController : ControllerBase
    {
        // GET: api/Generation
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/Generation/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/Generation
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GenerationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GenerationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
