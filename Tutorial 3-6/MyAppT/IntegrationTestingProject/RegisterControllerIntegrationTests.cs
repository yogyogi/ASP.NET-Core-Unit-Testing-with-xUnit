using Microsoft.Net.Http.Headers;
using MyAppT;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTestingProject
{
    public class RegisterControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        public RegisterControllerIntegrationTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Read_GET_Action()
        {
            // Act
            var response = await _client.GetAsync("/Register/Read");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("<h1 class=\"bg-info text-white\">Records</h1>", responseString);
        }

        [Fact]
        public async Task Create_GET_Action()
        {
            // Act
            var response = await _client.GetAsync("/Register/Create");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Create Record", responseString);
        }

        [Fact]
        public async Task Create_POST_Action_InvalidModel()
        {
            // Arrange
            var initialRes = await _client.GetAsync("/Register/Create");
            var antiForgeryVal = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initialRes);
            
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Register/Create");
            
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.Cookie, antiForgeryVal.cookie).ToString());
            
            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.Field, antiForgeryVal.field },
                { "Name", "New Person" },
                { "Age", "25" }
            };
            postRequest.Content = new FormUrlEncodedContent(formModel);

            // Act
            var response = await _client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("The field Age must be between 40 and 60", responseString);
        }

        [Fact]
        public async Task Create_POST_Action_ValidModel()
        {
            // Arrange
            var initialRes = await _client.GetAsync("/Register/Create");
            var antiForgeryVal = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initialRes);
            
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Register/Create");
            
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.Cookie, antiForgeryVal.cookie).ToString());
            
            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.Field, antiForgeryVal.field },
                { "Name", "New Person" },
                { "Age", "45" }
            };
            postRequest.Content = new FormUrlEncodedContent(formModel);

            // Act
            var response = await _client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("New Person", responseString);
            Assert.Contains("45", responseString);
        }
    }
}
