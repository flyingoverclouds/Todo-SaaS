using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace todo_service
{
    [Route("api/[controller]")]
    [ApiController]
    public class Items : ControllerBase
    {
        // GET: api/Items
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return new [] { new TodoItem(), new TodoItem() };
        }

        // GET api/Items/5
        [HttpGet("{id}")]
        public TodoItem Get(int id)
        {
            return new TodoItem();
        }

        // POST api/<ItemApi>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ItemApi>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemApi>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
