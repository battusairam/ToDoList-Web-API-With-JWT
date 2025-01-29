using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ToDoListAPIJWT.Data;
using ToDoListAPIJWT.Models;

namespace ToDoListAPIJWT.Controllers
{
    [Route("api/todo")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)=> _context = context;
        

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> Get()
        {
            return Ok(_context.TodoItems.ToList());
        }

        [HttpPost]
        public IActionResult Post([FromBody] TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TodoItem todoItem)
        {
            var existingItem = _context.TodoItems.Find(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Title = todoItem.Title;
            existingItem.IsCompleted = todoItem.IsCompleted;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todoItem = _context.TodoItems.Find(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
