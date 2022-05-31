using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ex22.Data;
using ex22;


namespace ex2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ex22Context _context;

        public ContactsController(ex22Context context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Json(await _context.Contact.ToListAsync());

        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Server")] Container1 container)
        {
            Contact contact = new Contact();
            contact.Id = container.Id;
            contact.Name = container.Name;
            contact.Server = container.Server;
            Chat chat = new Chat();
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
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return Json(contact);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Server,Last,LastDated")] Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return BadRequest();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return StatusCode(500);
        }

        private bool ContactExists(string id)
        {
            return (_context.Contact?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Contact == null)
            {
                return Problem("Entity set 'ex22Context.Contact'  is null.");
            }
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }
            var chat = await _context.Chat.FirstOrDefaultAsync(m => m.Contact.Id == id);
            if (chat != null)
            {
                _context.Chat.Remove(chat);
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("{id}/messages")]
        public async Task<IActionResult> getChat(string id)
        {
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            var chat = await _context.Chat.FirstOrDefaultAsync(m => m.Contact.Id == id);
            if (chat == null)
            {
                return NotFound();
            }
            var messages = from message in _context.Message
                           where message.ChatId == chat.Id 
                           select message;
            return Json(await messages.ToListAsync());
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> sendM(string id, [Bind("Content")] Container content)
        {
            Message message = new();
            if (id == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            var chat = await _context.Chat.FirstOrDefaultAsync(m => m.Contact.Id == id);
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
            message.Content = content.Content;
            message.Id = idm;
            message.Created = DateTime.UtcNow;
            message.Sent = false;
            message.ChatId = chat.Id;
            _context.Add(message);
            contact.Last = message.Content;
            contact.LastDate = message.Created;
            _context.Update(contact);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("{id1}/messages/{id2}")]
        public async Task<IActionResult> getM(string id1, int id2)
        {
            if (id1 == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id1);
            if (contact == null)
            {
                return NotFound();
            }
            var chat = await _context.Chat.FirstOrDefaultAsync(m => m.Contact.Id == id1);
            if (chat == null)
            {
                return NotFound();
            }
            var message = await _context.Message.FirstOrDefaultAsync(m => m.Id == id2);
            if (message == null)
            {
                return NotFound();
            }
            if(chat.Id != message.ChatId)
            {
                return NotFound();
            }

            return Json(message);
        }

        [HttpPut("{id1}/messages/{id2}")]
        public async Task<IActionResult> editM(string id1, int id2, [Bind("Content")] Container content)
        {
            
            if (id1 == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id1);
            if (contact == null)
            {
                return NotFound();
            }
            var chat = await _context.Chat.FirstOrDefaultAsync(m => m.Contact.Id == id1);
            if (chat == null)
            {
                return NotFound();
            }
            var message = await _context.Message.FirstOrDefaultAsync(m => m.Id == id2);
            if (message == null)
            {
                return NotFound();
            }
            if(message.ChatId != chat.Id)
            {
                return NotFound();
            }
            message.Content = content.Content;
            _context.Update(message);
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpDelete("{id1}/messages/{id2}")]
        public async Task<IActionResult> deleteM(string id1, int id2)
        {
            if (id1 == null || _context.Contact == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id1);
            if (contact == null)
            {
                return NotFound();
            }
            var chat = await _context.Chat.FirstOrDefaultAsync(m => m.Contact.Id == id1);
            if (chat == null)
            {
                return NotFound();
            }
            var message = await _context.Message.FirstOrDefaultAsync(m => m.Id == id2);
            if (message == null)
            {
                return NotFound();
            }
            if (chat.Id != message.ChatId)
            {
                return NotFound();
            }
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
