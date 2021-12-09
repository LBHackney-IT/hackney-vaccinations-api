using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.E2ETests
{

    public class NotificationE2EDynamoDbTest : DynamoDbIntegrationTests<Startup>
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly string _pk = "lbhNoification";

        /// <summary>
        /// Method to construct a test entity that can be used in a test
        /// </summary>
        /// <returns></returns>
        private Notification ConstructTestEntity()
        {
            var entity = _fixture.Create<Notification>();
            entity.CreatedAt = DateTime.UtcNow;
            return entity;
        }
        private NotificationObjectRequest GivenANewNotificationObjectRequestForScreen(TargetType targetType = TargetType.FailedDirectDebits, string message = "Direct Debit failed")
        {
            var entity = _fixture.Build<NotificationObjectRequest>()
                .With(_ => _.TargetType, targetType)
                 .With(_ => _.Message, message)
                .With(_ => _.NotificationType, NotificationType.Screen)
                .With(_ => _.RequireAction, false)
                .With(_ => _.RequireEmailNotification, false)
                .With(_ => _.RequireSmsNotification, false)
                .With(_ => _.RequireLetter, false)
                .Without(_ => _.ServiceKey)
                .Without(_ => _.TemplateId)
                .Create();
            return entity;
        }
        private NotificationObjectRequest GivenANewNotificationRequestWithValidationErrors()
        {
            var entity = _fixture.Build<NotificationObjectRequest>()
                           .Without(_ => _.TargetId)
                           .Create();
            return entity;
        }

        private NotificationObjectRequest GivenANewScreenNotificationRequestWithValidationErrors()
        {
            var entity = _fixture.Build<NotificationObjectRequest>()
                .With(_ => _.NotificationType, NotificationType.Screen)
                .Without(_ => _.TargetId)
                .Without(_ => _.TargetType)
                .Without(_ => _.Message)
                .Create();
            return entity;
        }
        private NotificationObjectRequest GivenANewEmailNotificationRequestWithValidationErrors()
        {
            var entity = _fixture.Build<NotificationObjectRequest>()
                .With(_ => _.NotificationType, NotificationType.Email)
                .Without(_ => _.ServiceKey)
                .Without(_ => _.TemplateId)
                .Without(_ => _.Email)
                .Create();
            return entity;
        }

        private NotificationObjectRequest GivenANewTextNotificationRequestWithValidationErrors()
        {
            var entity = _fixture.Build<NotificationObjectRequest>()
                .With(_ => _.NotificationType, NotificationType.Email)
                .Without(_ => _.ServiceKey)
                .Without(_ => _.TemplateId)
                .Without(_ => _.MobileNumber)
                .Create();
            return entity;
        }

        /// <summary>
        /// Method to add an entity instance to the database so that it can be used in a test.
        /// Also adds the corresponding action to remove the upserted data from the database when the test is done.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task SetupTestData(Notification entity)
        {
            var dbEntity = entity.ToDatabase();
            dbEntity.Pk = _pk;
            await DynamoDbContext.SaveAsync(dbEntity).ConfigureAwait(false);

            async void Item() =>
                await DynamoDbContext.DeleteAsync<NotificationEntity>(_pk, entity.Id)
                    .ConfigureAwait(false);

            CleanupActions.Add(Item);
        }

        [Fact]
        public async Task GetEntityByIdNotFoundReturns404()
        {
            var id = Guid.NewGuid();
            var uri = new Uri($"api/v2/notifications/{id}", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetNotificationByIdFoundReturnsResponse()
        {
            var entity = ConstructTestEntity();
            await SetupTestData(entity).ConfigureAwait(false);
            var uri = new Uri($"api/v2/notifications/{entity.Id}", UriKind.Relative);
            var response = await Client.GetAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonSerializer.Deserialize<NotificationResponseObject>(responseContent, CreateJsonOptions());

            apiEntity.Should().BeEquivalentTo(entity, (x) => x
                .Excluding(y => y.CreatedAt)
                .Excluding(y => y.IsMessageSent)
                .Excluding(y => y.ServiceKey)
                .Excluding(y => y.TemplateId)
                .Excluding(y => y.IsReadStatus)
                .Excluding(y => y.PerformedActionDate)
                .Excluding(y => y.User));
            apiEntity.CreatedDate.Date.Should().BeCloseTo(DateTime.UtcNow.Date);
        }

        [Fact]
        public async Task PostScreenNotificationReturnsCreated()
        {
            var requestObject = GivenANewNotificationObjectRequestForScreen();

            var uri = new Uri($"api/v2/notifications", UriKind.Relative);
            var body = JsonSerializer.Serialize(requestObject, CreateJsonOptions());

            using var stringContent = new StringContent(body);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync(uri, stringContent).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            responseContent.Should().NotBeEmpty();
            var apiNotification = responseContent.Replace("\\", string.Empty).Replace("\"", string.Empty);
            var id = Guid.Parse(apiNotification);
            var dbRecord = await DynamoDbContext.LoadAsync<NotificationEntity>(_pk, id).ConfigureAwait(false);
            id.Should().Be(dbRecord.Id);
            requestObject.TargetType.ToString().Should().Be(dbRecord.TargetType);
            requestObject.Message.Should().BeEquivalentTo(dbRecord.Message);
        }

        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        [Fact]
        public async Task PostNotificationReturnsUnProcessableEntityWithValidationErrors()
        {
            var requestObject = GivenANewNotificationRequestWithValidationErrors();

            var uri = new Uri($"api/v2/notifications", UriKind.Relative);
            var body = JsonSerializer.Serialize(requestObject, CreateJsonOptions());

            using var content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync(uri, content).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var jo = JObject.Parse(responseContent);
            var errors = jo["errors"].Children();

            ShouldHaveErrorFor(errors, "NotificationType");

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        }


        [Fact]
        public async Task PostScreenNotificationReturnsUnProcessableEntityWithValidationErrors()
        {
            var requestObject = GivenANewScreenNotificationRequestWithValidationErrors();

            var uri = new Uri($"api/v2/notifications", UriKind.Relative);
            var body = JsonSerializer.Serialize(requestObject, CreateJsonOptions());

            using var content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync(uri, content).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var jo = JObject.Parse(responseContent);
            var errors = jo["errors"].Children();

            ShouldHaveErrorFor(errors, "Message");
            ShouldHaveErrorFor(errors, "TargetType");
            ShouldHaveErrorFor(errors, "TargetId");

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        }

        [Fact]
        public async Task PostEmailNotificationReturnsUnProcessableEntityWithValidationErrors()
        {
            var requestObject = GivenANewEmailNotificationRequestWithValidationErrors();

            var uri = new Uri($"api/v2/notifications", UriKind.Relative);
            var body = JsonSerializer.Serialize(requestObject, CreateJsonOptions());

            using var content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync(uri, content).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var jo = JObject.Parse(responseContent);
            var errors = jo["errors"].Children();

            ShouldHaveErrorFor(errors, "ServiceKey");
            ShouldHaveErrorFor(errors, "TemplateId");
            ShouldHaveErrorFor(errors, "Email");

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        }

        [Fact]
        public async Task PostTextNotificationReturnsUnProcessableEntityWithValidationErrors()
        {
            var requestObject = GivenANewTextNotificationRequestWithValidationErrors();

            var uri = new Uri($"api/v2/notifications", UriKind.Relative);
            var body = JsonSerializer.Serialize(requestObject, CreateJsonOptions());

            using var content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync(uri, content).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var jo = JObject.Parse(responseContent);
            var errors = jo["errors"].Children();

            ShouldHaveErrorFor(errors, "ServiceKey");
            ShouldHaveErrorFor(errors, "TemplateId");
            ShouldHaveErrorFor(errors, "MobileNumber");

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        }
        //[Fact]
        public async Task DeleteNotificationByIdReturnSuccessResponse()
        {
            var entity = ConstructTestEntity();
            entity.RequireAction = false;
            entity.PerformedActionType = string.Empty;
            await SetupTestData(entity).ConfigureAwait(false);
            var uri = new Uri($"api/v2/notifications/{entity.Id}", UriKind.Relative);
            var response = await Client.DeleteAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonSerializer.Deserialize<ActionResponse>(responseContent, CreateJsonOptions());
            apiEntity.Status.Should().BeTrue();
            apiEntity.Message.Should().BeEquivalentTo("successfully removed");
        }

        [Fact]
        public async Task DeleteNotificationByIdReturnsFailedResponse()
        {
            var id = Guid.NewGuid();
            var uri = new Uri($"api/v2/notifications/{id}", UriKind.Relative);
            var response = await Client.DeleteAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonSerializer.Deserialize<ActionResponse>(responseContent, CreateJsonOptions());
            apiEntity.Status.Should().BeFalse();
            apiEntity.Message.Should().BeEquivalentTo($"Record with Id: {id} not found");
        }


        [Fact]
        public async Task DeleteNotificationByIdWithRequireActionSetToTrueAndPerformedActionEmptyReturnBadRequestResponse()
        {
            var entity = ConstructTestEntity();
            entity.RequireAction = true;
            entity.PerformedActionType = string.Empty;
            await SetupTestData(entity).ConfigureAwait(false);
            var uri = new Uri($"api/v2/notifications/{entity.Id}", UriKind.Relative);
            var response = await Client.DeleteAsync(uri).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var apiEntity = JsonSerializer.Deserialize<ActionResponse>(responseContent, CreateJsonOptions());
            apiEntity.Status.Should().BeFalse();
            apiEntity.Message.Should().BeEquivalentTo("You are not allow to remove/delete this record");
        }
        private static void ShouldHaveErrorFor(JEnumerable<JToken> errors, string propertyName, string errorCode = null)
        {
            var error = errors.FirstOrDefault(x => (x.Path.Split('.').Last().Trim('\'', ']')) == propertyName) as JProperty;
            error.Should().NotBeNull();
            if (!string.IsNullOrEmpty(errorCode))
                error?.Value.ToString().Should().Contain(errorCode);
        }
    }
}
