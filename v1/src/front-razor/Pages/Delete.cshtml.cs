using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using front_common.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace front_razor.Pages
{

    public class DeleteModel : PageModel
    {
        public  Guid TodoId { get; set; }
        public bool Deleted { get; set; } = false;
        public string ErrorMessage { get; set; } = null!;

        private readonly ILogger<PrivacyModel> _logger;
        private readonly ServiceSettings _settings;

        public DeleteModel(ILogger<PrivacyModel> logger,ServiceSettings settings)
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
            TodoId = id.Value;
            HttpClient hc = new HttpClient();
            try
            {

                //updating item in db
                var url = $"{_settings.TodoServiceUri}/api/Items/{id}";
                var res = await hc.DeleteAsync(url);
                if (!res.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Erreur http {res.StatusCode} : {res.ReasonPhrase}";
                    return;
                }
                ErrorMessage = null;
                Deleted = true;
            }
            catch(Exception ex)
            {
                ErrorMessage = $"Error while deleting item : {ex.Message}";
                _logger.LogError(ex, $"Exception: {ex.Message}");
                return;
            }
        }

    }
}