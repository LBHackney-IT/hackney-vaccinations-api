using System;
using FluentAssertions;
using LbhNotificationsApi.V1.Domain;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Domain
{

    public class EntityTests
    {
        [Fact]
        public void EntitiesHaveAnId()
        {
            var entity = new Notification { TargetId = "yeyeyeyey" };
            entity.TargetId.Should().NotBeEmpty();//.BeGreaterOrEqualTo(0);
        }

        [Fact]
        public void EntitiesHaveACreatedAt()
        {
            var entity = new Notification();
            var date = new DateTime(2019, 02, 21);
            entity.CreatedAt = date;

            entity.CreatedAt.Should().BeSameDateAs(date);
        }
    }
}
