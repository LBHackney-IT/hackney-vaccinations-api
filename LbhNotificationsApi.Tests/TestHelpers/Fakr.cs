using AutoFixture;
using AutoFixture.Dsl;
using Bogus;
using System.Collections.Generic;
using System.Linq;
namespace LbhNotificationsApi.Tests.TestHelpers
{
    public static class Fakr
    {
        private static Faker _faker = new Faker();                 // Good for single values
        private static Fixture _fixture = new Fixture();           // Good for complex objects
        static Fakr()                                           // Gets called automatically by common language runtime (CLR)
        {
            CustomizeCircularReferenceBehaviour();
        }
        public static int Id(int minimum = 0, int maximum = 10000)
        {
            return _faker.Random.Int(minimum, maximum);
        }
        public static string Text()
        {
            return string.Join(" ", _faker.Random.Words(5));
        }
        public static T Create<T>()
        {
            return _fixture.Create<T>();
        }
        public static IEnumerable<T> CreateMany<T>(int quantity = 3)
        {
            return _fixture.CreateMany<T>(quantity);
        }

        public static ICustomizationComposer<T> Build<T>()
        {
            return _fixture.Build<T>();
        }
        #region Options
        public static void CustomizeCircularReferenceBehaviour()
        {
            _fixture.Customize(new PreventCircularReferencesCustomisation());
        }
        #endregion
    }
    #region Autofixture Customization
    public class PreventCircularReferencesCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
    #endregion
}
