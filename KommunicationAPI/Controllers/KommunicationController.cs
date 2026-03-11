using KommunicationAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KommunicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KommunicationController : ControllerBase
    {
        private readonly KommunicationDbContext _dbContext;

        public KommunicationController(KommunicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/<KommunicationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<KommunicationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<KommunicationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<KommunicationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<KommunicationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
