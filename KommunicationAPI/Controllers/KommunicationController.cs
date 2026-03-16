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
        [HttpGet("messages/{messageId}")]
        public async Task<IActionResult> GetMessageById(int messageId)
        {
            var message = await _dbContext.Messages.FindAsync(messageId);

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
        
        //Updates message.IsRead from false to true
        
        [HttpPatch("{messageId}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int messageId)
        {
            var message = await _dbContext.Messages.FindAsync(messageId);

            if (message == null)
                return NotFound();

            if (!message.IsRead)
            {
                message.IsRead = true;
                await _dbContext.SaveChangesAsync();
            }

            return NoContent();
        }

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
