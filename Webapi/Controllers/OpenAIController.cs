using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using OpenAI_API;
using EnrollmentSystem.API.ApiResponses;

namespace EnrollmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly string _openAiApiKey;

        public OpenAIController()
        {
            _openAiApiKey = "APIKEY";
        }

        [HttpPost]
        [Route("GetResult")]
        public IActionResult GetResult([FromBody] string prompt)
        {
            try
            {
                string answer = string.Empty;

                var openAiApi = new OpenAIAPI(_openAiApiKey);
                CompletionRequest completion = new CompletionRequest();

                completion.Prompt = prompt;
                completion.Model = "davinci-002";
                completion.MaxTokens = 100;

                var result = openAiApi.Completions.CreateCompletionAsync(completion);

                if (result != null)
                {
                    foreach (var item in result.Result.Completions)
                    {
                        answer = item.Text;
                    }
                    return Ok(new ApiResponse(true, answer, null));
                }
                return BadRequest(new ApiResponse(false, null, "Not Found"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new ApiResponse(false, null, "Internal server error"));
            }
        }
    }
}
