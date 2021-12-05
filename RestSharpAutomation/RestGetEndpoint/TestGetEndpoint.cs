using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceAutomation.Model.JsonModelGetAll;
using MicroserviceAutomation.Model.JsonModelGetUniqueUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;

namespace RestSharpAutomation.RestGetEndpoint
{
    [TestClass]
    public class TestGetEndpoint
    {
        
        private string getAllUsersUrl = "https://gorest.co.in/public/v1/users/";
        private string getUniqueUserUrl = "https://gorest.co.in/public/v1/users/" ;
        private string getInvalidUserUrl = "https://gorest.co.in/public/v1/users/20063";
        private string postUniqueUserUrl = "https://gorest.co.in/public/v1/users/";
        private int userId;
        private Random random = new Random();
        private Data GetDataObject()
        {
            Data data = new Data();
            data.name = "Catarina Nina Moraes";
            data.gender = "female";
            data.email = "catarinaninamoraes" + random.Next(1000) + "@autvale.com";
            data.status = "active";

            return data;
        }

        [TestMethod]
        public void TestAssertGetUniqueUserWithHelper_shouldBe200()
        {
            AddRecord();
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept", "application/json" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getUniqueUserUrl + userId, headers);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "Content is Null/Empty");

            IRestResponse<JsonRootObjectGetUniqueUser> restResponse1 = restClientHelper.PerformGetRequest<JsonRootObjectGetUniqueUser>(getUniqueUserUrl + userId, headers);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Assert.IsNotNull(restResponse1.Data, "Content is Null/Empty");

            Assert.AreEqual(userId, restResponse1.Data.data.id);
            Assert.AreEqual("Catarina Nina Moraes", restResponse1.Data.data.name);
            Assert.AreEqual("female", restResponse1.Data.data.gender);
            Assert.AreEqual("active", restResponse1.Data.data.status);

        }
        [TestMethod]
        public void TestAssertGetAllUsersWithHelper_shouldBe200()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept", "application/json" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getAllUsersUrl, headers);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "Content is Null/Empty");

            IRestResponse<JsonRootObjectGetAll> restResponse1 = restClientHelper.PerformGetRequest<JsonRootObjectGetAll>(getAllUsersUrl, headers);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse1.Data, "Content is Null/Empty");

        }

        [TestMethod]
        public void TestAssertGetUniqueUserWithHelper_shouldBe404()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Accept", "application/json" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(getInvalidUserUrl, headers);
            Assert.AreEqual(404, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "Content is Null/Empty");

            IRestResponse<JsonRootObjectGetUniqueUser> restResponse1 = restClientHelper.PerformGetRequest<JsonRootObjectGetUniqueUser>(getInvalidUserUrl, headers);
            Assert.AreEqual(404, (int)restResponse1.StatusCode);
            Assert.IsNotNull(restResponse1.Data, "Content is Null/Empty");
            Assert.AreEqual("Resource not found", restResponse1.Data.data.message);
        }

        public void AddRecord()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json"},
                { "Accept", "application/json"},
                { "Authorization", "Bearer 7ebca025948b14ab1772bfeb77a26c05e4eefcb471f3f81e47f176038af659db"}
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<JsonRootObjectGetUniqueUser> restResponse = restClientHelper.PerformPostRequest<JsonRootObjectGetUniqueUser>(postUniqueUserUrl, headers, GetDataObject(), DataFormat.Json);

            Assert.AreEqual(201, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Data, "Response Content is Null");

            Assert.AreEqual("Catarina Nina Moraes", restResponse.Data.data.name);
            Assert.AreEqual("female", restResponse.Data.data.gender);
            Assert.AreEqual("active", restResponse.Data.data.status);
            Console.WriteLine(restResponse.Data.data.id);

            userId = restResponse.Data.data.id;

        }
    }
}
