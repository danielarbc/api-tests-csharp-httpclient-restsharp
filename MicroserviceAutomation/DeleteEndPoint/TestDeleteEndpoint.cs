using MicroserviceAutomation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MicroserviceAutomation.Model.JsonModelGetUniqueUser;
using Newtonsoft.Json;
using MicroserviceAutomation.Helper;
using MicroserviceAutomation.Helper.Request;

namespace MicroserviceAutomation.DeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndpoint
    {
        private string baseUrl = "https://gorest.co.in/public/v1/users/";
        private string jsonMediaType = "application/json";
        private string authHeader = AuthToken.Auth;
        private Random random = new Random();
        private int userId;
        private RestResponse restResponse;

        [TestMethod]
        public void TestDelete_shouldBe200()
        {
            AddRecord();
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Authorization", authHeader }
            };

            restResponse = HttpClientHelper.PerformDeleteRequest(baseUrl + userId, headers);
            Assert.AreEqual(204, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformDeleteRequest(baseUrl + userId, headers);
            Assert.AreEqual(404, restResponse.StatusCode);
        }
        public void AddRecord()
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

                    JsonRootObjectGetUniqueUser JsonRootObjectGetUniqueUser = JsonConvert.DeserializeObject<JsonRootObjectGetUniqueUser>(restResponse.ResponseContent);
                    Assert.AreEqual(201, restResponse.StatusCode);

                    userId = JsonRootObjectGetUniqueUser.data.id;
                }
            }
        }
    }

   
}
