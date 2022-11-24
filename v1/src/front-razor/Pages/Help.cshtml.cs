using front_common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace front_razor.Pages
{
    public class HelpModel : PageModel
    {
        private readonly ILogger<HelpModel> _logger;
        private readonly IConfiguration _configuration;
        
        
        public string ApiEnvironmentVariables { get; set; } = "";
        public string FrontEnvironmentVariables { get; set; }

        public string TodoServiceUri { get; set; }
        public HelpModel(ILogger<HelpModel> logger, IConfiguration config,ServiceSettings service_Settings)
        {
            this._logger = logger;
            this._configuration = config;
            this.TodoServiceUri = service_Settings.TodoServiceUri;
        }

        public async Task OnGet()
        {
            try
            {
                _logger.LogInformation($"TodoServiceUri={TodoServiceUri}");
                HttpClient hc = new HttpClient();
                ApiEnvironmentVariables = await hc.GetStringAsync($"{TodoServiceUri}/api/Help/Env");
            }
            catch(Exception ex)
            {
                ApiEnvironmentVariables = $"Exception while calling {TodoServiceUri}/api/Help/Env : {ex.Message}";
                _logger.LogError(ex, ApiEnvironmentVariables);
            }

            StringBuilder sb = new StringBuilder();
            foreach (var evname in Environment.GetEnvironmentVariables().Keys)
            {
                var evValue = Environment.GetEnvironmentVariable(evname.ToString());
                if (evValue.EndsWith("==") || evname.ToString().ToLower().Contains("key"))
                    evValue = evValue.Substring(0, Math.Min(evValue.Length, 5)) + "**REDACTED**";
                sb.AppendLine($"{evname.ToString()}={evValue}");
            }
            FrontEnvironmentVariables = sb.ToString();
        }
    }
}