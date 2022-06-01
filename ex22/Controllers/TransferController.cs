using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ex22;
using ex22.Data;
using ex2.Controllers;

namespace ex22.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {

        private readonly ex22Context _context;

        public TransferController(ex22Context context)
        {
            _context = context;
        }

        // POST: /api/Transfer/
        [HttpPost]
        public async Task<IActionResult> TransferAsync([Bind("From,To,Content")] Transfer transfer)
        {
            if (ex22.User.Id != transfer.To)
            {
                return NotFound();
            }
            var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == transfer.From);
            if (contact == null)
            {
                return NotFound();
            }
            var chat =  await _context.Chat.FirstOrDefaultAsync(m => m.Contact.Id == transfer.From);
            if (chat == null)
            {
                return NotFound();
            }
            int idm;
            if (_context.Message.Count() == 0)
            {
                idm = 0;
            }
            else
            {
                idm = _context.Message.Max(c => c.Id) + 1;
            }
            Message message = new();
            message.Id = idm;
            message.ChatId = chat.Id;
            message.Content = transfer.Content;
            message.Sent = false;
            message.Created = DateTime.UtcNow;


            _context.Add(message);
            contact.Last = message.Content;
            contact.LastDate = message.Created;
            _context.Update(contact);
            _context.SaveChanges();

            return Ok();
        }
    }
}
