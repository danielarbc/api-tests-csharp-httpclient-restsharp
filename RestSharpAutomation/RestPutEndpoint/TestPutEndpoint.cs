using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceAutomation.Model.JsonModelGetUniqueUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using Newtonsoft.Json;

namespace RestSharpAutomation.RestPutEndpoint
{
    [TestClass]
    public class TestPutEndpoint
    {
        private string postUniqueUserUrl = "https://gorest.co.in/public/v1/users";
        private string putUniqueUserUrl = "https://gorest.co.in/public/v1/users/";
        private string getUrl = "https://gorest.co.in/public/v1/users/2106";
        private Random random = new Random();
        private Data GetDataObjectforPost()
        {
            Data data = new Data();
            data.name = "Catarina Nina Moraes";
            data.gender = "female";
            data.email = "catarinaninamoraes" + random.Next(1000) + "@autvale.com";
            data.status = "active";

            return data;
        }
        private object GetDataObjectForPut()
        {
            return new
            {
                email = $"catarinaninamoraes{random.Next(1000)}@autvale.com",
                status = "inactive"

            };

        }

        [TestMethod]
        public void TestPutUniqueUserWithHelper_shouldBe200()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Accept", "application/json"},
                { "Authorization", "Bearer 7ebca025948b14ab1772bfeb77a26c05e4eefcb471f3f81e47f176038af659db"}
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<JsonRootObjectGetUniqueUser> restResponse = restClientHelper.PerformPostRequest<JsonRootObjectGetUniqueUser>(postUniqueUserUrl, headers, GetDataObjectforPost(), DataFormat.Json);

            Assert.AreEqual(201, (int)restResponse.StatusCode);
            int userId = restResponse.Data.data.id;
            Console.WriteLine(userId);

            IRestResponse<JsonRootObjectGetUniqueUser> restResponse1 = restClientHelper.PerformPutRequest<JsonRootObjectGetUniqueUser>(putUniqueUserUrl + userId, headers, GetDataObjectForPut(), DataFormat.Json);

            Console.WriteLine(restResponse1.StatusCode);
            Console.WriteLine(restResponse1.ResponseUri);
            Console.WriteLine(restResponse1.Content);

            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Assert.AreEqual("Catarina Nina Moraes", restResponse1.Data.data.name);
            Assert.AreEqual("female", restResponse1.Data.data.gender);
            Assert.AreEqual("inactive", restResponse1.Data.data.status);
            Assert.AreEqual(userId, restResponse1.Data.data.id);
            
        }
    }
}
