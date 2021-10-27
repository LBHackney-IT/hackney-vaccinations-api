using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace LbhNotificationsApi.V1.Infrastructure.Conventers
{
    /// <summary>
    /// Converter for DateTime objects because the default handling expects the data time string to always be in a very specific
    /// format and will throw an exception if not.
    /// </summary>
    public class DynamoDbDateTimeConverter : IPropertyConverter
    {
        private const string DATEFORMAT = "yyyy-MM-ddTHH\\:mm\\:ss.fffffffZ";

        public DynamoDBEntry ToEntry(object value)
        {
            if (null == value)
            {
                return new DynamoDBNull();
            }

            return new Primitive { Value = ((DateTime) value).ToUniversalTime().ToString(DATEFORMAT) };
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            var primitive = entry as Primitive;

            if (null == primitive)
            {
                return null;
            }

            var dtString = primitive.Value.ToString();

            return DateTime.Parse(dtString, null, System.Globalization.DateTimeStyles.RoundtripKind).ToLocalTime();
        }
    }
}
