using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.Services
{
    public class OpenAIService
    {
        private readonly OpenAIAPI _openAiApi;
        public OpenAIService(string apiKey)
        {
            _openAiApi = new OpenAIAPI(apiKey);
        }

  
    }
}
