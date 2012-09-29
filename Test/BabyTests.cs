using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolymorphicEnum;

namespace Test
{
    public class BabyState : PolymorphicEnum<BabyState>
    {
        public static BabyState POOP = Register();

        public static BabyState SLEEP =
            Register(data: new { Next = POOP });

        public static BabyState EAT =
            Register(data: new { Next = SLEEP });

        public static BabyState CRY =
            Register(data: new { Next = EAT });

        public BabyState Next(bool discomfort)
        {
            if (discomfort)
                return CRY;

            return this.Data == null
                ? EAT
                : (BabyState)((dynamic)this.Data).Next;
        }
    }

    [TestClass]
    public class BabyTests
    {
        private const bool DISCOMFORT = true;
        private const bool NO_DISCOMFORT = false;

        [TestMethod]
        public void eat_then_sleep_then_poop_and_repeat()
        {
            Assert.AreEqual(BabyState.SLEEP, BabyState.EAT.Next(NO_DISCOMFORT));
            Assert.AreEqual(BabyState.POOP, BabyState.SLEEP.Next(NO_DISCOMFORT));
            Assert.AreEqual(BabyState.EAT, BabyState.POOP.Next(NO_DISCOMFORT));
        }

        [TestMethod]
        public void if_discomfort_then_cry_then_eat()
        {
            Assert.AreEqual(BabyState.CRY, BabyState.SLEEP.Next(DISCOMFORT));
            Assert.AreEqual(BabyState.EAT, BabyState.CRY.Next(NO_DISCOMFORT));
        }
    }
}
