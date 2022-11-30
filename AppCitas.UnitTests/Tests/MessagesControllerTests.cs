using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppCitas.UnitTests.Tests
{
    public class MessagesControllerTests
    {
        private string apiRoute = "api/messages";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUrl;
        private string registeredObject;
        private HttpContent httpContent;
        public MessagesControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }
        [Theory]
        [InlineData("OK", "hewitt", "Pa$$w0rd", "louise", "WInter is coming")]
        public async Task CreateMessageOK(string statusCode, string username, string password, string recipientUsername, string content)
        {
            requestUrl = $"{apiRoute}";
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var messageDto = new MessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };
            registeredObject = GetRegisterObject(messageDto);
            httpContent = GetHttpContent(registeredObject);
            requestUrl = $"{apiRoute}";

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        [Theory]
        [InlineData("BadRequest", "lisa", "Pa$$w0rd", "lisa", "Hello world")]
        public async Task CreateMessageToYourself(string statusCode, string username, string password, string recipientUsername, string content)
        {
            requestUrl = $"{apiRoute}";
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var messageDto = new MessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };
            registeredObject = GetRegisterObject(messageDto);
            httpContent = GetHttpContent(registeredObject);
            

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        [Theory]
        [InlineData("NotFound", "lisa", "Pa$$w0rd", "brand", "Hola mundo")]
        public async Task CreateMessageNotFound(string statusCode, string username, string password, string recipientUsername, string content)
        {
            requestUrl = $"{apiRoute}";
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var messageDto = new MessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };
            registeredObject = GetRegisterObject(messageDto);
            httpContent = GetHttpContent(registeredObject);

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd")]
        public async Task GetMessagesForUserOK(string statusCode, string username, string password)
        {
            requestUrl = $"{apiRoute}";
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            
            // Act
            httpResponse = await _client.GetAsync(requestUrl);
            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "todd", "Pa$$w0rd", "lisa")]
        public async Task GetMessagesThreadOK(string statusCode, string username, string password, string username2)
        {
            requestUrl = $"{apiRoute}/thread/" + username2;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            
            // Act
            httpResponse = await _client.GetAsync(requestUrl);
            _client.DefaultRequestHeaders.Authorization = null;
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        


        #region Privated methods
        private static string GetRegisterObject(MessageDto message)
        {
            var entityObject = new JObject()
            {
                { nameof(message.RecipientUsername), message.RecipientUsername },
                { nameof(message.Content), message.Content }
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
