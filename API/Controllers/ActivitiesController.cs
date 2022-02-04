using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _context;

        public ActivitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/<ActivitiesController>
        [HttpGet]

        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }


        // GET api/<ActivitiesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return await _context.Activities.FindAsync(id);
        }

        // POST api/<ActivitiesController>
        //[HttpPost]


        // PUT api/<ActivitiesController>/5
        //[HttpPut("{id}")]


        // DELETE api/<ActivitiesController>/5
        //[HttpDelete("{id}")]

    }
}
