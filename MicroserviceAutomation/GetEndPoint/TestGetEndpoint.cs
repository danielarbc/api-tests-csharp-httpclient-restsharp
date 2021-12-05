using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MicroserviceAutomation.Helper;
using MicroserviceAutomation.Model;
using MicroserviceAutomation.Model.JsonModelGetAll;
using MicroserviceAutomation.Model.JsonModelGetUniqueUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MicroserviceAutomation.GetEndPoint
{
    [TestClass]
	public class TestGetEndpoint
	{
		private string baseUrl = "https://gorest.co.in/public/v1/users/";
		private string getInvalidUserUrl = "https://gorest.co.in/public/v1/users/20063";
		private string jsonMediaType = "application/json";
		private string authHeader = AuthToken.Auth;
		private RestResponse restResponse;
		private Random random = new Random();
		private int userIdPost;

		[TestMethod]
		public void TestAssertGetAllUsers_shouldBe200()
		{
			using (HttpClient httpClient = new HttpClient())
			{
				using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
				{
					httpRequestMessage.RequestUri = new Uri(baseUrl);
					httpRequestMessage.Method = HttpMethod.Get;
					httpRequestMessage.Headers.Add("Accept", "application/json");

					Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

					using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
					{
						Console.WriteLine(httpResponseMessage.ToString());

						HttpStatusCode statusCode = httpResponseMessage.StatusCode;
						
						HttpContent responseContent = httpResponseMessage.Content;
						Task<string> responseData = responseContent.ReadAsStringAsync();
						string data = responseData.Result;

						RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);

						JsonRootObjectGetAll jsonRootObjectGetAll = JsonConvert.DeserializeObject<JsonRootObjectGetAll>(restResponse.ResponseContent);
						Console.WriteLine(jsonRootObjectGetAll.ToString());

						Assert.AreEqual(200, restResponse.StatusCode);

						Assert.IsNotNull(restResponse.ResponseContent);
					}
				}
			}
		}

		[TestMethod]
		public void TestAssertGetUniqueUser_shouldBe200()
		{
			AddRecord();
			using (HttpClient httpClient = new HttpClient())
			{
				using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
				{
					httpRequestMessage.RequestUri = new Uri(baseUrl + userIdPost);
					httpRequestMessage.Method = HttpMethod.Get;
					httpRequestMessage.Headers.Add("Accept", "application/json");

					Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

					using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
					{
						Console.WriteLine(httpResponseMessage.ToString());

						HttpStatusCode statusCode = httpResponseMessage.StatusCode;

						HttpContent responseContent = httpResponseMessage.Content;
						Task<string> responseData = responseContent.ReadAsStringAsync();
						string data = responseData.Result;

						RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);

						JsonRootObjectGetUniqueUser JsonRootObjectGetUniqueUser = JsonConvert.DeserializeObject<JsonRootObjectGetUniqueUser>(restResponse.ResponseContent);
						Console.WriteLine(JsonRootObjectGetUniqueUser.ToString());

						Assert.AreEqual(200, restResponse.StatusCode);

						Assert.IsNotNull(restResponse.ResponseContent);

						Assert.AreEqual(userIdPost, JsonRootObjectGetUniqueUser.data.id);
						Assert.AreEqual("Catarina Nina Moraes", JsonRootObjectGetUniqueUser.data.name);
						Assert.AreEqual("female", JsonRootObjectGetUniqueUser.data.gender);
						Assert.AreEqual("active", JsonRootObjectGetUniqueUser.data.status);
					}
				}
			}
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

					userIdPost = JsonRootObjectGetUniqueUser.data.id;
				}
			}
		}

		[TestMethod]
		public void TestAssertGetInvalidUser_shouldBe404()
		{

			using (HttpClient httpClient = new HttpClient())
			{
				using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
				{
					httpRequestMessage.RequestUri = new Uri(getInvalidUserUrl);
					httpRequestMessage.Method = HttpMethod.Get;
					httpRequestMessage.Headers.Add("Accept", "application/json");

					Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

					using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
					{
						Console.WriteLine(httpResponseMessage.ToString());

						HttpStatusCode statusCode = httpResponseMessage.StatusCode;

						HttpContent responseContent = httpResponseMessage.Content;
						Task<string> responseData = responseContent.ReadAsStringAsync();
						string data = responseData.Result;

						RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);

						JsonRootObjectGetUniqueUser JsonRootObjectGetUniqueUser = JsonConvert.DeserializeObject<JsonRootObjectGetUniqueUser>(restResponse.ResponseContent);
						Console.WriteLine(JsonRootObjectGetUniqueUser.ToString());

						Assert.AreEqual(404, restResponse.StatusCode);

						Assert.AreEqual("Resource not found", JsonRootObjectGetUniqueUser.data.message);
					}
				}
			}
		}
	}
}
