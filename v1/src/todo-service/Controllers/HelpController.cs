using Microsoft.AspNetCore.Mvc;
using front_common.Models;
using todo_service.Services;
using Newtonsoft.Json;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace todo_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : ControllerBase
    {
        
        private readonly IConfiguration _config;

        public HelpController(IConfiguration _config)
        {
            
            this._config = _config;
        }

        // GET: api/Items
        /// <summary>
        /// Return all items 
        /// </summary>
        /// <returns></returns>
        [HttpGet("env/")]
        public async Task<string> GetEnv()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var evname in Environment.GetEnvironmentVariables().Keys)
            {
                var evValue = Environment.GetEnvironmentVariable(evname.ToString());
                if (evValue.EndsWith("==") || evname.ToString().ToLower().Contains("key"))
                    evValue = evValue.Substring(0, Math.Min(evValue.Length, 5)) + "**REDACTED**";
                sb.AppendLine($"{evname.ToString()}={evValue}");
            }
            return sb.ToString();
        }

    }
}
