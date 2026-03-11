using KommunicationAPI.Data;
using KommunicationAPI.Models;
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

        [HttpGet]
        public Message[] getMessages()
        {
            Message[] messages = _dbContext.Messages.ToArray();
            return messages;
        }

        [HttpPost]
        public void postMessage(Message message)
        {
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
        }
    }
}
