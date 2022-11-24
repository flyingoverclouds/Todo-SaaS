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
        private readonly ServiceSettings _settings;

        public List<TodoItem> TodoItems { get; private set; } = null!;

        public IndexModel(ILogger<IndexModel> logger, ServiceSettings serviceSetting)
        {
            this._logger = logger;
            this._settings= serviceSetting;
        }

        public async Task OnGet()
        {
            _logger.LogInformation($"TodoServiceUri={_settings.TodoServiceUri}");
            HttpClient hc = new HttpClient();
            var json = await hc.GetStringAsync($"{_settings.TodoServiceUri}/api/Items"); // Todo-Service running in WSL
            TodoItems = JsonSerializer.Deserialize<List<TodoItem>>(json)!;
        }
    }
}