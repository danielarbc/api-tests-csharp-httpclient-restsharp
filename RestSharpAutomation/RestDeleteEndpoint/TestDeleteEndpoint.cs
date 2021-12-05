using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceAutomation.Model.JsonModelGetUniqueUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;

namespace RestSharpAutomation.RestDeleteEndpoint
{
    [TestClass]
    public class TestDeleteEndpoint
    {
        private string postUniqueUserUrl = "https://gorest.co.in/public/v1/users";
        private string deleteUniqueUserUrl = "https://gorest.co.in/public/v1/users/";
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

        [TestMethod]
        public void TestPutUniqueUserWithHelper_shouldBe200()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json"},
                { "Accept", "application/json"},
                { "Authorization", "Bearer 7ebca025948b14ab1772bfeb77a26c05e4eefcb471f3f81e47f176038af659db"}
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<JsonRootObjectGetUniqueUser> restResponse = restClientHelper.PerformPostRequest<JsonRootObjectGetUniqueUser>(postUniqueUserUrl, headers, GetDataObjectforPost(), DataFormat.Json);

            Assert.AreEqual(201, (int)restResponse.StatusCode);
            int userId = restResponse.Data.data.id;
            Console.WriteLine(userId);

            IRestResponse restResponse1 = restClientHelper.PerformDeleteRequest(deleteUniqueUserUrl + userId, headers);
            Assert.AreEqual(204, (int)restResponse1.StatusCode);

            restResponse1 = restClientHelper.PerformDeleteRequest(deleteUniqueUserUrl + userId, headers);
            Assert.AreEqual(404, (int)restResponse1.StatusCode);

        }



    }
}
