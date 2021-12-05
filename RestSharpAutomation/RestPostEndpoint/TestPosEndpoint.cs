using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceAutomation.Model.JsonModelGetUniqueUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.HelperClass.Request;
using RestSharpAutomation.Model.JsonModelPostUniqueUserDuplicated;

namespace RestSharpAutomation.RestPostEndpoint
{
    [TestClass]
    public class TestPosEndpoint
    {
        private string postUniqueUserUrl = "https://gorest.co.in/public/v1/users";
        private string getUrl = "https://gorest.co.in/public/v1/users/2106";
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
        //[TestMethodWithReport]
        public void TestPostUniqueUserWithHelper_shouldBe201()
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

        }

        [TestMethod]
        //[TestMethodWithReport]
        public void TestPostUniqueUserWithHelper_shouldBe422()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json"},
                { "Accept", "application/json"},
                { "Authorization", "Bearer 7ebca025948b14ab1772bfeb77a26c05e4eefcb471f3f81e47f176038af659db"}
            };

            object sameObject = GetDataObject();

            RestClientHelper restClientHelper = new RestClientHelper();
            //1st send
            
            IRestResponse<JsonRootObjectGetUniqueUser> restResponse = restClientHelper.PerformPostRequest<JsonRootObjectGetUniqueUser>(postUniqueUserUrl, headers, sameObject, DataFormat.Json);
            Assert.AreEqual(201, (int)restResponse.StatusCode);
            Console.WriteLine(restResponse.Data.data.id);

            //2nd send
            IRestResponse<JsonRootObjectPostUniqueUserDuplicated> restResponse1 = restClientHelper.PerformPostRequest<JsonRootObjectPostUniqueUserDuplicated>(postUniqueUserUrl, headers, sameObject, DataFormat.Json);

            Assert.AreEqual(422, (int)restResponse1.StatusCode);
            Assert.AreEqual("email", restResponse1.Data.data[0].field);
            Assert.AreEqual("has already been taken", restResponse1.Data.data[0].message);
        }
    }
}
