using Amazon.DynamoDBv2.Model;
using AutoMapper;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NotificationEntity, Notification>()
                .ReverseMap();

            CreateMap<QueryResponse, List<Notification>>()
                .ConvertUsing(qr => (qr == null) ? null : qr.ToNotification());

            CreateMap<Notification, NotificationResponseObject>();
        }
    }
}
