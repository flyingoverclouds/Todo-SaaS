using front_common.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;

namespace front_razor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _TodoServiceUri;

        public List<TodoItem> TodoItems { get; private set; }
        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            this._logger = logger;
            this._configuration = config;
            this._TodoServiceUri = _configuration.GetValue<string>("TodoServiceUri");
        }

        public async Task OnGet()
        {
            
            HttpClient hc = new HttpClient();
            var json = await hc.GetStringAsync($"{_TodoServiceUri}/api/Items"); // Todo-Service running in WSL
            TodoItems = JsonSerializer.Deserialize<List<TodoItem>>(json);
        }
    }
}