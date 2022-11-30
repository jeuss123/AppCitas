using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace AppCitas.UnitTests.Tests
{
    public class LikesControllerTests
    {
        private string apiRoute = "api/likes";
        private readonly HttpClient _client;
        private HttpResponseMessage? httpResponse;
        private string requestUrl = String.Empty;
        private string registerObject = String.Empty;
        private string loginObjetct = String.Empty;
        private HttpContent? httpContent;
        public LikesControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", "caroline")]
        public async Task AddLikeOk(string statusCode, string username, string password, string userLiked)
        {
            requestUrl = $"{apiRoute}/" + userLiked;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        [Theory]
        [InlineData("NotFound", "lisa", "Pa$$w0rd", "john")]
        public async Task AddLikeNotFound(string statusCode, string username, string password, string userLiked)
        {
            requestUrl = $"{apiRoute}/" + userLiked;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("BadRequest", "todd", "Pa$$w0rd", "todd")]
        public async Task AddLikeBadRequestToYourself(string statusCode, string username, string password, string userLiked)
        {
            requestUrl = $"{apiRoute}/" + userLiked;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);    

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        [Theory]
        [InlineData("BadRequest", "lisa", "Pa$$w0rd", "louise")]
        public async Task AddLikeSamePerson(string statusCode, string username, string password, string userLiked)
        {
            requestUrl = $"{apiRoute}/" + userLiked;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods
        private static string GetRegisterObject(LoginDto loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
            return entityObject.ToString();
        }
        private StringContent GetHttpContent(string objectToEncode)
        {
            return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
        }
        #endregion
    }
}