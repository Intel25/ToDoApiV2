using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApiV2.Models;

namespace ToDoApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private DataContext _dataContext;
        public TasksController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int count)
        {
            if (page < 1 || count < 1)
            {
                return BadRequest();
            }

            var tasksToReturn = await _dataContext.Tasks
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync();

            return Ok(tasksToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _dataContext.Tasks.FirstOrDefaultAsync(elem => elem.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskModel taskToCreate)
        {
            await _dataContext.Tasks.AddAsync(taskToCreate);
            await _dataContext.SaveChangesAsync();
            return CreatedAtAction("GetById", new { taskToCreate.Id }, taskToCreate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _dataContext.Tasks.FirstOrDefaultAsync(elem => elem.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            _dataContext.Tasks.Remove(item);
            await _dataContext.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskModel taskUpdate)
        {
            var item = await _dataContext.Tasks.FirstOrDefaultAsync(elem => elem.Id == id);

            if (item == null)
            {
                return NotFound();
            }
            item.Description = taskUpdate.Description;
            item.Name = taskUpdate.Name;
            item.DeadLine = taskUpdate.DeadLine;
            await _dataContext.SaveChangesAsync();

            return Ok(item);
        }

    }
}

