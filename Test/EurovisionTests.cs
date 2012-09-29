using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolymorphicEnum;

namespace Test
{
    /**
     * The policy on how to notify the user of any
     * Eurovision song contest event
     */
    public class EurovisionNotification
        : PolymorphicEnum<EurovisionNotification>
    {
        /** Default behavior : I don't want to know ! */
        public virtual bool MustNotify(String eventCity,
                                       String userCity)
        {
            return false;
        }

        /** I love Eurovision, don't want to miss it, never! */
        private class EurovisionNotificationAlways
            : EurovisionNotification
        {
            public override bool MustNotify(string eventCity,
                                            string userCity)
            {
 	            return true;
            }
        }

        /**
         * I only want to know about Eurovision if it takes place
         * in my city, so that I can take holidays elsewhere at
         * the same time
         */
        private class EurovisionNotificationOnlyCity
            : EurovisionNotification
        {
            public override bool MustNotify(string eventCity,
                                            string userCity)
            {
 	            return eventCity.Equals(
                    userCity,
                    StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public static EurovisionNotification ALWAYS =
            Register<EurovisionNotificationAlways>();
        public static EurovisionNotification ONLY_IN_MY_CITY =
            Register<EurovisionNotificationOnlyCity>();
        public static EurovisionNotification NEVER =
            Register();
    }

    [TestClass]
    public class EurovisionTests
    {
        [TestMethod]
        public void notify_users_anywhere()
        {
            Assert.IsTrue(EurovisionNotification.ALWAYS.MustNotify("Baku", "Baku"));
            Assert.IsTrue(EurovisionNotification.ALWAYS.MustNotify("Baku", "BAKU"));
            Assert.IsTrue(EurovisionNotification.ALWAYS.MustNotify("Baku", "Paris"));
        }

        [TestMethod]
        public void notify_users_in_Baku_only()
        {
            Assert.IsTrue(EurovisionNotification.ONLY_IN_MY_CITY.MustNotify("Baku", "Baku"));
            Assert.IsTrue(EurovisionNotification.ONLY_IN_MY_CITY.MustNotify("Baku", "BAKU"));
            Assert.IsFalse(EurovisionNotification.ONLY_IN_MY_CITY.MustNotify("Baku", "Paris"));
        }

        [TestMethod]
        public void notify_users_never()
        {
            Assert.IsFalse(EurovisionNotification.NEVER.MustNotify("Baku", "Baku"));
            Assert.IsFalse(EurovisionNotification.NEVER.MustNotify("Baku", "BAKU"));
            Assert.IsFalse(EurovisionNotification.NEVER.MustNotify("Baku", "Paris"));
        }
    }
}
