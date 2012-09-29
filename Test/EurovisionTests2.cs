using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolymorphicEnum;

namespace Test
{
    using Policy = System.Func<string, string, bool>;

    public class EurovisionNotificationLambda
        : PolymorphicEnum<EurovisionNotificationLambda>
    {
        private static Policy Policy(Policy f) { return f; }

        public static EurovisionNotificationLambda ALWAYS =
            Register(data: Policy((eventCity, userCity) => true));

        public static EurovisionNotificationLambda ONLY_IN_MY_CITY =
            Register(data: Policy((eventCity, userCity) =>
                eventCity.Equals(
                    userCity,
                    StringComparison.InvariantCultureIgnoreCase)));

        public static EurovisionNotificationLambda NEVER =
            Register(data: Policy((eventCity, userCity) => false));

        public bool MustNotify(String eventCity, String userCity)
        {
            return ((Policy)this.Data).Invoke(eventCity, userCity);
        }
    }

    [TestClass]
    public class EurovisionTests2
    {
        [TestMethod]
        public void notify_users_anywhere()
        {
            Assert.IsTrue(EurovisionNotificationLambda.ALWAYS.MustNotify("Baku", "Baku"));
            Assert.IsTrue(EurovisionNotificationLambda.ALWAYS.MustNotify("Baku", "BAKU"));
            Assert.IsTrue(EurovisionNotificationLambda.ALWAYS.MustNotify("Baku", "Paris"));
        }

        [TestMethod]
        public void notify_users_in_Baku_only()
        {
            Assert.IsTrue(EurovisionNotificationLambda.ONLY_IN_MY_CITY.MustNotify("Baku", "Baku"));
            Assert.IsTrue(EurovisionNotificationLambda.ONLY_IN_MY_CITY.MustNotify("Baku", "BAKU"));
            Assert.IsFalse(EurovisionNotificationLambda.ONLY_IN_MY_CITY.MustNotify("Baku", "Paris"));
        }

        [TestMethod]
        public void notify_users_never()
        {
            Assert.IsFalse(EurovisionNotificationLambda.NEVER.MustNotify("Baku", "Baku"));
            Assert.IsFalse(EurovisionNotificationLambda.NEVER.MustNotify("Baku", "BAKU"));
            Assert.IsFalse(EurovisionNotificationLambda.NEVER.MustNotify("Baku", "Paris"));
        }
    }
}
