using NotificationsApi.Tests;


namespace LbhNotificationsApi.Tests.V1.E2ETests
{

    public class ConfirmationsTests : DynamoDbIntegrationTests<Startup>
    {


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
