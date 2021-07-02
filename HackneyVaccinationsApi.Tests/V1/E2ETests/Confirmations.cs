
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace HackneyVaccinationsApi.Tests.V1.E2ETests
{
    [TestFixture]
    public class ConfirmationsTests : IntegrationTests<Startup>
    {
        [SetUp]
        public void SetUp()
        {

        }

        // [TestCase(TestName = "Given valid email and mobile number a notification is sent to the details provided")]
        // public async Task PostEmailAndTextConfirmations()
        // {
        //     //arrange
        //     var postMessage = @"
        //         {
        //             ""Email"" : ""test@example.com"",
        //             ""MobileNumber"" : ""00000000000""
        //         }";
        //     HttpContent postContent = new StringContent(postMessage, Encoding.UTF8, "application/json");
        //     var requestUri = new Uri("api/v1/confirmations", UriKind.Relative);
        //
        //     //act
        //     var response = await Client.PostAsync(requestUri, postContent).ConfigureAwait(true);
        //     postContent.Dispose();
        //
        //     //assert
        //     response.StatusCode.Should().Be(201);
        // }
    }
}
