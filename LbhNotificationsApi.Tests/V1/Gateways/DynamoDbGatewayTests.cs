using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.Tests.V1.Helper;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways;
using LbhNotificationsApi.V1.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Gateways
{

    public sealed class DynamoDbGatewayTests : IDisposable
    {

        private readonly Fixture _fixture = new Fixture();
        private string _pk = "lbhNoification";

        private readonly Mock<IDynamoDBContext> _dynamoDb;
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDB;
        private readonly DynamoDbGateway _gateway;
        private readonly List<Action> _cleanup;

        public DynamoDbGatewayTests()
        {
            _cleanup = new List<Action>();
            _dynamoDb = new Mock<IDynamoDBContext>();
            _amazonDynamoDB = new Mock<IAmazonDynamoDB>();
            _gateway = new DynamoDbGateway(_dynamoDb.Object, _amazonDynamoDB.Object);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var action in _cleanup)
                    action();

                _disposed = true;
            }
        }
        [Fact]
        public async Task GetAllNotifications()
        {
            var query = new NotificationSearchQuery();

            var databaseEntities = MockQueryResponse(1);
            var entities = databaseEntities.ToNotification();

            _amazonDynamoDB.Setup(x => x.QueryAsync(It.IsAny<QueryRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(databaseEntities);

            var response = await _gateway.GetAllAsync(query).ConfigureAwait(false);

            response.Should().BeEquivalentTo(entities);
        }
        [Fact]
        public async Task GetEntityByIdReturnsNullIfEntityDoesntExist()
        {
            var guid = Guid.NewGuid();
            _dynamoDb.Setup(x => x.LoadAsync<NotificationEntity>(_pk, guid, default))
                  .ReturnsAsync((NotificationEntity) null);
            var response = await _gateway.GetEntityByIdAsync(guid).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.LoadAsync<NotificationEntity>(_pk, guid, default), Times.Once);
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetEntityByIdReturnsTheEntityIfItExists()
        {
            var entity = _fixture.Create<Notification>();
            var dbEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

            _dynamoDb.Setup(x => x.LoadAsync<NotificationEntity>(_pk, entity.Id, default))
                     .ReturnsAsync(dbEntity);

            var response = await _gateway.GetEntityByIdAsync(entity.Id).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.LoadAsync<NotificationEntity>(_pk, entity.Id, default), Times.Once);

            entity.Id.Should().Be(response.Id);
            entity.TargetId.Should().Be(response.TargetId);
            entity.CreatedAt.Should().BeSameDateAs(response.CreatedAt);
        }


        [Fact]
        public async Task PostNewNotificationSuccessfulSaves()
        {
            // Arrange
            var entityRequest = _fixture.Build<Notification>()
                                 .Create();
            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<NotificationEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
            // Act
            await _gateway.AddAsync(entityRequest).ConfigureAwait(false);

            // Assert
            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<NotificationEntity>(), default), Times.Once);

        }


        [Fact]
        public async Task UpdateExistingNotificationSuccessfulSaves()
        {
            // Arrange
            var entityRequest = _fixture.Build<Notification>()
                                   .With(x => x.PerformedActionType, ActionType.Initiated.ToString())
                                 .Create();
            var dbEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entityRequest);
            _dynamoDb.Setup(x => x.LoadAsync<NotificationEntity>(_pk, entityRequest.Id, default))
                     .ReturnsAsync(dbEntity);

            var approvalRequest = new UpdateRequest { ActionNote = "Approval Note", ActionType = ActionType.Approved };

            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<NotificationEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
            // Act
            var response = await _gateway.UpdateAsync(entityRequest.Id, approvalRequest).ConfigureAwait(false);

            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<NotificationEntity>(), default), Times.Once);
            // var load = await _gateway.GetEntityByIdAsync(entityRequest.Id).ConfigureAwait(false);
            response.ActionNote.Should().BeEquivalentTo(approvalRequest.ActionNote);
            response.PerformedActionType.Should().Be(approvalRequest.ActionType.ToString());
            response.PerformedActionDate?.Date.Should().BeSameDateAs(DateTime.UtcNow.Date);

        }

        [Fact]
        public async Task UpdateExistingNotificationFailedSaves()
        {
            // Arrange
            var guid = Guid.NewGuid();

            var approvalRequest = new UpdateRequest { ActionNote = "Approval Note", ActionType = ActionType.Approved };

            _dynamoDb.Setup(x => x.SaveAsync(It.IsAny<NotificationEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
            // Act
            await _gateway.UpdateAsync(guid, approvalRequest).ConfigureAwait(false);

            // Assert

            _dynamoDb.Verify(x => x.SaveAsync(It.IsAny<NotificationEntity>(), default), Times.Never);
            var load = await _gateway.GetEntityByIdAsync(guid).ConfigureAwait(false);
            load.Should().BeNull();

        }

        private QueryResponse MockQueryResponse(int cnt = 1)
        {
            var response = new QueryResponse();
            for (int i = 0; i < cnt; i++)
            {
                response.Items.Add(
                    new Dictionary<string, AttributeValue>()
                    {
                        {"pk", new AttributeValue {S = _pk}},
                        {"id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                        {"target_id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                        {"target_type", new AttributeValue {S = _fixture.Create<TargetType>().ToString()}},
                        {"notification_type", new AttributeValue {S = _fixture.Create<NotificationType>().ToString()}},
                        {
                            "created_at",
                            new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}
                        },
                        {"action_note", new AttributeValue {S = _fixture.Create<string>()}},
                        {
                            "performed_action_date",
                            new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}
                        },
                        {"performed_action_done_by", new AttributeValue {S = _fixture.Create<string>()}},
                        {"performed_action_type", new AttributeValue {S = _fixture.Create<ActionType>().ToString()}},
                        {"require_action", new AttributeValue {S = _fixture.Create<bool>().ToString()}},
                        {"require_sms_notification", new AttributeValue {S = _fixture.Create<bool>().ToString()}},
                        {"require_letter", new AttributeValue {S = _fixture.Create<bool>().ToString()}},
                        {"require_email_notification", new AttributeValue {S = _fixture.Create<bool>().ToString()}},
                        {"is_read_status", new AttributeValue {S = _fixture.Create<bool>().ToString()}},
                        {"Email", new AttributeValue {S = _fixture.Create<string>()}},
                        {"message", new AttributeValue {S = _fixture.Create<string>().ToString()}},

                        {
                            "is_message_sent",
                            new AttributeValue {SS = _fixture.Create<List<string>>()}
                        },
                        {
                            "mobile_number",
                            new AttributeValue {S = _fixture.Create<string>()}
                        },
                        {"user", new AttributeValue {S = _fixture.Create<string>()}},
                       
                        //{
                        //    "person",
                        //    new AttributeValue
                        //    {
                        //        M = new Dictionary<string, AttributeValue>
                        //        {
                        //            {"id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                        //            {"fullName", new AttributeValue {S = _fixture.Create<string>()}}
                        //        }
                        //    }
                        //},
                        //{
                        //    "suspense_resolution_info",
                        //    new AttributeValue
                        //    {
                        //        M = new Dictionary<string, AttributeValue>
                        //        {
                        //            {
                        //                "isConfirmed",
                        //                new AttributeValue {BOOL = _fixture.Create<bool>()}
                        //            },
                        //            {
                        //                "isApproved",
                        //                new AttributeValue {BOOL = _fixture.Create<bool>()}
                        //            },
                        //            {"note", new AttributeValue {S = _fixture.Create<string>()}},
                        //            {
                        //                "resolutionDate",
                        //                new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}
                        //            }
                        //        }
                        //    }
                        //}
                    });
            }
            return response;
        }
    }
}
