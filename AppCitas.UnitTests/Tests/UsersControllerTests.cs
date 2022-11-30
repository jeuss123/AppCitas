using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace AppCitas.UnitTests.Tests
{
    public class UsersControllerTests
    {
        private string apiRoute = "api/users";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUrl;
        private string registeredObject;
        private HttpContent httpContent;

        public UsersControllerTests()
        {

            _client = TestHelper.Instance.Client;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd")]
        public async Task GetUsersOK(string statusCode, string username, string password)
        {
            requestUrl = $"{apiRoute}";
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            
            // Act
            httpResponse = await _client.GetAsync(requestUrl);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", 1, 2)]
        public async Task GetUsersWithPaginationOK(string statusCode, string username, string password, int pageSize, int pageNumber)
        {
            requestUrl = $"{apiRoute}" + "?pageNumber=" + pageSize + "&pageSize" + pageNumber;
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
           
            // Act
            httpResponse = await _client.GetAsync(requestUrl);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd")]
        public async Task GetUserByUsernameOK(string statusCode, string username, string password)
        {
            requestUrl = $"{apiRoute}/" + username;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            
            // Act
            httpResponse = await _client.GetAsync(requestUrl);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        public async Task UpdateUser(string statusCode, string username, string password, string introduction, string lookingFor, string interests, string city, string country)
        {
            requestUrl = $"{apiRoute}";
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var memberUpdateDto = new MemberUpdateDto
            {
                Introduction = introduction,
                LookingFor = lookingFor,
                Interests = interests,
                City = city,
                Country = country
            };
            registeredObject = GetRegisterObject(memberUpdateDto);
            httpContent = GetHttpContent(registeredObject);

            // Act
            httpResponse = await _client.PutAsync(requestUrl, httpContent);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods
        private static string GetRegisterObject(MemberUpdateDto memberUpdateDto)
        {
            var entityObject = new JObject()
            {
                { nameof(memberUpdateDto.Introduction), memberUpdateDto.Introduction },
                { nameof(memberUpdateDto.LookingFor), memberUpdateDto.LookingFor },
                { nameof(memberUpdateDto.Interests), memberUpdateDto.Interests },
                { nameof(memberUpdateDto.City), memberUpdateDto.City },
                { nameof(memberUpdateDto.Country), memberUpdateDto.Country }
            };
            return entityObject.ToString();
        }
        private static string GetRegisterObject(string file)
        {
            var entityObject = new JObject()
            {
                { "File", file}
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