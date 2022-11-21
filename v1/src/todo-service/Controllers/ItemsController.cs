using Microsoft.AspNetCore.Mvc;
using front_common.Models;
using todo_service.Services;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace todo_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        const string FixedTenant = "T1";

        
        IItemDataService dataservice = null;

        public ItemsController(Microsoft.Azure.Cosmos.Container dbContainer)
        {
            dataservice = new TodoItemService(dbContainer,FixedTenant);
        }

        // GET: api/Items
        /// <summary>
        /// Return all items 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<TodoItem>> Get()
        {
            return await dataservice.GetItemsAsync();
        }

        // GET api/Items/5
        /// <summary>
        /// Return a specific todoItem by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<TodoItem> Get(Guid id)
        {
            return await dataservice.GetItemAsync(id);
        }

        // POST api/<ItemApi>
        /// <summary>
        /// ajoute un nouveau todo item dans la db
        /// </summary>
        /// <param name="value">TodoItem en Json</param>
        [HttpPost]
        public async void Post([FromBody] string todoJson)
        {
            await dataservice.CreateItemAsync(JsonConvert.DeserializeObject<TodoItem>(todoJson));
        }


        // Todo : add update checked status api 
        //// PUT api/<ItemApi>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ItemApi>/5
        /// <summary>
        /// DElete the todo item with a specific ID
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async void Delete(Guid id)
        {
            await dataservice.DeleteItemAsync(id);
        }
    }
}
