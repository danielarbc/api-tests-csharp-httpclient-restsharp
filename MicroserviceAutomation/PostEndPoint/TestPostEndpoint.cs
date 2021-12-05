using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MicroserviceAutomation.Helper.Request;
using MicroserviceAutomation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroserviceAutomation.Model.JsonModelGetUniqueUser;
using Newtonsoft.Json;
using FluentAssertions;
using MicroserviceAutomation.Helper;

namespace MicroserviceAutomation.PostEndPoint
{
    [TestClass]
    public class TestPostEndpoint
    {
        private string baseUrl = "https://gorest.co.in/public/v1/users/";
        private RestResponse restResponse;
        private string jsonMediaType = "application/json";
        private string authHeader = AuthToken.Auth;
        private Random random = new Random();

        [TestMethod]
        public void TestPostEndPoint_shouldBe201()
        {
            int randonEmailNumber = random.Next(999);
            var jsonData = new
            {
                name = "Catarina Nina Moraes",
                gender = "female",
                email = $"catarinaninamoraes{randonEmailNumber}@autvale.com",
                status = "active"
            };

            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", jsonMediaType);
                    httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);

                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(baseUrl);
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.UTF8, "application/json");

                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);

                    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                    restResponse.StatusCode.Should().Be(201);

                    JsonRootObjectGetUniqueUser JsonRootObjectGetUniqueUser = JsonConvert.DeserializeObject<JsonRootObjectGetUniqueUser>(restResponse.ResponseContent);
                    Console.WriteLine(JsonRootObjectGetUniqueUser.ToString());

                    int id = JsonRootObjectGetUniqueUser.data.id;
                    Console.WriteLine(id);

                    Helper.Data.UserId = JsonRootObjectGetUniqueUser.data.id;
                }
            }
        }
    }
}
