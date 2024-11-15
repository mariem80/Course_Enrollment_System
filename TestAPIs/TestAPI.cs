namespace TestAPIs
{
    public class TestAPI
    {
        private readonly HttpClient _httpClient;

        public TestAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetApiResponse()
        {
            // Example GET request
            var response = await _httpClient.GetAsync("https://your-api-endpoint");

            // Handle the response as needed
            return await response.Content.ReadAsStringAsync();
        }
    }
}
