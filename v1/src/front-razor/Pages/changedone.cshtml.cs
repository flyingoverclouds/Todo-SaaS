using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using front_common.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace front_razor.Pages
{

    public class ChangeDoneModel : PageModel
    {
        public TodoItem Todo { get; set; } = null!;
        public bool? PreviousDone { get; set; } = null;
        public string ErrorMessage { get; set; } = null!;

        private readonly ILogger<PrivacyModel> _logger;
        private readonly ServiceSettings _settings;

        public ChangeDoneModel(ILogger<PrivacyModel> logger,ServiceSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }


        public async Task OnGet(Guid? id)
        {
            if (id==null) {
                ErrorMessage = "Missing id !";
                return;
            }

            HttpClient hc = new HttpClient();
            TodoItem todoToChange = null;
            try
            {
                // retrieveing TodoItems from Service
                _logger.LogInformation($"TodoServiceUri={_settings.TodoServiceUri}");
                
                var json = await hc.GetStringAsync($"{_settings.TodoServiceUri}/api/Items/{id}");
                todoToChange = JsonSerializer.Deserialize<TodoItem>(json)!;
            }
            catch(Exception ex) {
                ErrorMessage = $"Error while get item : {ex.Message}";
                _logger.LogError(ex, $"Exception: {ex.Message}");
                return;
            }
            if (todoToChange == null) {
                ErrorMessage = "Item not found";
                return;
            }
            try
            {
                // inversing done status
                this.PreviousDone = todoToChange.done;
                todoToChange.done = !todoToChange.done;

                //updating item in db
                var json = JsonSerializer.Serialize<TodoItem>(todoToChange);
                var url = $"{_settings.TodoServiceUri}/api/Items/";
                var res = await hc.PostAsync(url,JsonContent.Create(json));
                if (!res.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Erreur http {res.StatusCode} : {res.ReasonPhrase}";
                    return;
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = $"Error while updating item : {ex.Message}";
                _logger.LogError(ex, $"Exception: {ex.Message}");
                return;
            }
            this.Todo = todoToChange;

        }

    }
}