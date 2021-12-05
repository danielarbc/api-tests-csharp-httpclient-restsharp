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
using MicroserviceAutomation.Helper;

namespace MicroserviceAutomation.PutEndpoint
{
    [TestClass]
    public class TestPutEndpoint
    {
        private string baseUrl = "https://gorest.co.in/public/v1/users/";
        private RestResponse restResponse;
        private string jsonMediaType = "application/json";
        private string authHeader = AuthToken.Auth;
        private Random random = new Random();
        private int userId;

        [TestMethod]
        public void TestAseertPutEndPoint_shouldBe200()
        {
            AddRecord();

            int randonEmailNumber = random.Next(999);
            var jsonData = new
            {
                email = $"catarinaninamoraes{randonEmailNumber}@autvale.com",
                status = "inactive"
            };

            //RestResponse restResponse = HttpClientHelper.PerformGetRequest(postUrl, httpHeader);

            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {

                    httpClient.DefaultRequestHeaders.Add("Accept", jsonMediaType);
                    httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);

                    httpRequestMessage.Method = HttpMethod.Put;
                    httpRequestMessage.RequestUri = new Uri(baseUrl + userId);
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.UTF8, "application/json");

                    Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);

                    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                    JsonRootObjectGetUniqueUser JsonRootObjectGetUniqueUser = JsonConvert.DeserializeObject <JsonRootObjectGetUniqueUser>(restResponse.ResponseContent);
                    Console.WriteLine(JsonRootObjectGetUniqueUser.ToString());

                    Assert.AreEqual(200, restResponse.StatusCode);

                    Assert.AreEqual("Catarina Nina Moraes", JsonRootObjectGetUniqueUser.data.name);
                    Assert.AreEqual("female", JsonRootObjectGetUniqueUser.data.gender);
                    Assert.AreEqual("inactive", JsonRootObjectGetUniqueUser.data.status);
                    Assert.AreEqual(userId, JsonRootObjectGetUniqueUser.data.id);
                }
            }
        }
        public void AddRecord()
        {
            //Generate a Randon number, because we can't register the same email
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