using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyAppT.Models;
using Xunit;

namespace IntegrationTestingProject
{
    public class RegisterControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        public RegisterControllerIntegrationTests(TestingWebAppFactory<Program> factory)
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

        [Fact]
        public async Task Update_GET_Action()
        {
            // Arrange
            int testId = 1;

            // Act
            var response = await _client.GetAsync($"/Register/Update/{testId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("1", responseString);
            Assert.Contains("Test One", responseString);
            Assert.Contains("40", responseString);
        }

        [Fact]
        public async Task Update_POST_Action_InvalidModel()
        {
            // Arrange
            int testId = 1;

            var initialRes = await _client.GetAsync($"/Register/Update/{testId}");
            var antiForgeryVal = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initialRes);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/Register/Update");

            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.Cookie, antiForgeryVal.cookie).ToString());

            var formModel = new Dictionary<string, string>
                                {
                                    { AntiForgeryTokenExtractor.Field, antiForgeryVal.field },
                                    { "Id", $"{testId}" },
                                    { "Name", "Test One" },
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
        public async Task Update_POST_Action_ValidModel()
        {
            // Arrange
            int testId = 2;
            var initialRes = await _client.GetAsync($"/Register/Update/{testId}");
            var antiForgeryVal = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initialRes);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Register/Update");

            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.Cookie, antiForgeryVal.cookie).ToString());

            var formModel = new Dictionary<string, string>
                                {
                                    { AntiForgeryTokenExtractor.Field, antiForgeryVal.field },
                                    { "Id", $"{testId}" },
                                    { "Name", "New Person" },
                                    { "Age", "45" }
                                };
            postRequest.Content = new FormUrlEncodedContent(formModel);

            // Act
            var response = await _client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("2", responseString);
            Assert.Contains("New Person", responseString);
            Assert.Contains("45", responseString);
        }

        [Fact]
        public async Task Delete_POST_Action()
        {
            // Arrange
            int testId = 3;
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/Register/Delete/{testId}");

            // Act
            var response = await _client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.DoesNotContain("Test Three", responseString);
            Assert.DoesNotContain("60", responseString);
        }
    }
}
