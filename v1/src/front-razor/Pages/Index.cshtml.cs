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

        public List<TodoItem> TodoItems { get; private set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            HttpClient hc = new HttpClient();
            var json = await hc.GetStringAsync("http://localhost:5006/api/Items"); // Todo-Service running in WSL
            TodoItems = JsonSerializer.Deserialize<List<TodoItem>>(json);
        }
    }
}