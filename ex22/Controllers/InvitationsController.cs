using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ex22;
using ex22.Data;

namespace ex22.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : Controller
    {
        private readonly ex22Context _context;


        public InvitationsController(ex22Context context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> InvitationAsync([Bind("From,To,Server")] Invitation invitation)
        { 
            if(ex22.User.Id != invitation.To)
            {
                return NotFound();
            }
            var c = await _context.Chat.FirstOrDefaultAsync(m => m.Contact.Id == invitation.From);
            if (c != null)
            {
                return NotFound();
            }
            Contact contact = new Contact();
            contact.Id = invitation.From;
            contact.Name = invitation.From;
            contact.Server = invitation.Server;
            contact.Last = "";
 
            Chat chat = new();

 
            int id;
            if (_context.Chat.Count() == 0)
            {
                id = 0;
            }
            else
            {
                id = _context.Chat.Max(c => c.Id) + 1;
            }
            chat.Id = id;
            chat.Contact = contact;
            _context.Add(chat);
            _context.Add(contact);
            await _context.SaveChangesAsync();
            return StatusCode(201);

            return Ok();
        }

    }
}
