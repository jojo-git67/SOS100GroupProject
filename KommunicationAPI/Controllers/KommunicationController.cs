using KommunicationAPI.Data;
using KommunicationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        
        //Get all messages
        [HttpGet("user/{senderId}")]
        public async Task<ActionResult<IEnumerable<Message>>> getMessages()
        {
            //Maybe add .Where()
            var messages = await _dbContext.Messages
                .ToListAsync();
            
            return Ok(messages);
        }

        //Returns a specific selected message
        [HttpGet("{id}")]
        public async Task<IActionResult> getMessageById(int id)
        {
            var message = await _dbContext.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }
            
            return Ok(message);
        }
        
        [HttpPost]
        public async Task<IActionResult> createMessage(Message message)
        {
            //Add more attributes?
            message.timestamp = DateTime.Now;
            message.IsRead = false;
            _dbContext.Messages.Add(message); 
            await _dbContext.SaveChangesAsync();
            
            return Ok(message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> editMessage(int id, string newMessage)
        {
            var message = await _dbContext.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }
            
            message.Content = newMessage;
            
            await _dbContext.SaveChangesAsync();
            
            return Ok(message);
        }
        
        //TODO: Add method to change status from unread to read

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteMessage(int id)
        {
            var message = await _dbContext.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }
            
            _dbContext.Messages.Remove(message);
            await  _dbContext.SaveChangesAsync();
            return Ok(message);
        }
    }
}
