using KommunicationAPI.Data;
using KommunicationAPI.DTOs;
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
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetAllMessagesForUser(int userId)
        {
            
            var messages = await _dbContext.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .ToListAsync();
            
            return Ok(messages);
        }
        
        
        //Returns a specific selected message by messageId
        [HttpGet("messages/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _dbContext.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }
            
            return Ok(message);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDto dto)
        {
            var message = new Message
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                timestamp = DateTime.Now,
                IsRead = false
            };

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            return Ok(message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] UpdateMessageDto dto)
        {
            var message = await _dbContext.Messages.FindAsync(id);

            if (message == null)
                return NotFound();

            message.Content = dto.Content;

            await _dbContext.SaveChangesAsync();

            return Ok(message);
        }
        
        //TODO: Add method to change status from unread to read

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
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
