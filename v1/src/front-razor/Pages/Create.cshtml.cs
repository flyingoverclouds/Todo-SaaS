using front_common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Runtime;
using System.Text.Json;


namespace front_razor.Pages
{
    public class CreateModel : PageModel
    {
        public ServiceSettings _settings { get; private set; }

        public CreateModel(ServiceSettings settings)
        {
            this._settings = settings;
        }

        [BindProperty]
        public string Title { get; set; }


        [BindProperty]
        public string Content { get; set; }


        [BindProperty]
        public bool Done { get; set; }

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public async Task OnPost()
        {
            if (string.IsNullOrEmpty(this.Title))
            {
                this.ErrorMessage = "Le titre est obligatoire.";
                return;
            }

            try {
                TodoItem newItem = new TodoItem()
                {
                    id = Guid.NewGuid(),
                    tenant = "", // will be set by the api
                    title = this.Title,
                    content = this.Content,
                    done = this.Done
                };
                HttpClient hc = new HttpClient();
                var res = await hc.PutAsync($"{_settings.TodoServiceUri}/api/Items/{newItem.id}",
                    JsonContent.Create(JsonSerializer.Serialize<TodoItem>(newItem)));
                if (!res.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Erreur http {res.StatusCode} : {res.ReasonPhrase}";
                    return;
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "EXCEPTION : " + ex.Message;
            }
        }
    }
}
