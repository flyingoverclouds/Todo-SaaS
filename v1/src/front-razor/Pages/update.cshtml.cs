using front_common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime;
using System.Text.Json;
using System.Xml;

namespace front_razor.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public string ErrorMessage { get; set; } = null;
        public TodoItem Todo { get; set; } = null;
        public ServiceSettings _settings { get; private set; }

        public UpdateModel(ILogger<PrivacyModel> logger, ServiceSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public async Task OnGet(Guid? id)
        {
            if (id == null)
            {
                ErrorMessage = "Missing id !";
                return;
            }

            HttpClient hc = new HttpClient();
            TodoItem todoToUpdate = null;
            try
            {
                // retrieveing TodoItems from Service
                _logger.LogInformation($"TodoServiceUri={_settings.TodoServiceUri}");
                todoToUpdate = await hc.GetFromJsonAsync<TodoItem>($"{_settings.TodoServiceUri}/api/Items/{id}");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error while getting item : {ex.Message}";
                _logger.LogError(ex, $"Exception: {ex.Message}");
                return;
            }
            this.Todo = todoToUpdate;
        }

        public async Task OnPost()
        {
            try
            {
                TodoItem newItem = new TodoItem()
                {
                    tenant = Request.Form["Todo.tenant"],
                    id = Guid.Parse(Request.Form["Todo.id"]),
                    timestamp = DateTime.UtcNow,
                    title = Request.Form["Todo.title"],
                    content = Request.Form["Todo.content"],
                    done = (Request.Form["Todo.done"] == "on") ? true : false
                };
                HttpClient hc = new HttpClient();
                var res = await hc.PostAsync($"{_settings.TodoServiceUri}/api/Items/", 
                    JsonContent.Create(JsonSerializer.Serialize<TodoItem>(newItem)));
                if (!res.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Erreur http {res.StatusCode} : {res.ReasonPhrase}";
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception Update.OnPost() {ex.Message}");
            }
            Response.Redirect("/");
        }


    }
}