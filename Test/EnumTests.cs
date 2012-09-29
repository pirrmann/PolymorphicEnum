using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolymorphicEnum;

namespace Test
{
    public class SomeEnum : PolymorphicEnum<SomeEnum>
    {
        public static SomeEnum FirstValue = Register();
        public static SomeEnum SecondValue = Register();
    }

    public class OtherEnum : PolymorphicEnum<SomeEnum>
    {
        public static SomeEnum ThirdValueAttempt = Register();
    }

    public class OtherChildEnum : SomeEnum
    {
        public static SomeEnum FourthValueAttempt = Register();
    }

    [TestClass]
    public class EnumTests
    {
        [TestMethod]
        public void to_string()
        {
            Assert.AreEqual("FirstValue", SomeEnum.FirstValue.ToString());
            Assert.AreEqual("SecondValue", SomeEnum.SecondValue.ToString());
        }

        [TestMethod]
        public void compares_to()
        {
            Assert.AreEqual(0, SomeEnum.FirstValue.CompareTo(SomeEnum.FirstValue));
            Assert.AreEqual(-1, SomeEnum.FirstValue.CompareTo(SomeEnum.SecondValue));

            Assert.AreEqual(1, SomeEnum.SecondValue.CompareTo(SomeEnum.FirstValue));
            Assert.AreEqual(0, SomeEnum.SecondValue.CompareTo(SomeEnum.SecondValue));
        }

        [TestMethod]
        public void implicit_conversion_operators()
        {
            Assert.IsTrue(SomeEnum.FirstValue == 0);
            Assert.IsTrue(SomeEnum.SecondValue == 1);
        }

        [TestMethod]
        public void explicit_conversion_operators()
        {
            Assert.IsTrue(SomeEnum.FirstValue == (SomeEnum)0);
            Assert.IsTrue(SomeEnum.SecondValue == (SomeEnum)1);
        }

        [TestMethod]
        public void cannot_add_new_enum_members()
        {
            TypeInitializationException catchedException = null;
            try
            {
                SomeEnum thirdValueAttempt = OtherEnum.ThirdValueAttempt;
            }
            catch (TypeInitializationException e)
            {
                catchedException = e;
            }

            Assert.IsNotNull(catchedException);
            Assert.IsInstanceOfType(catchedException.InnerException, typeof(EnumInitializationException));
        }

        [TestMethod]
        public void cannot_add_new_enum_members_from_child()
        {
            TypeInitializationException catchedException = null;
            try
            {
                SomeEnum fourthValueAttempt = OtherChildEnum.FourthValueAttempt;
            }
            catch (TypeInitializationException e)
            {
                catchedException = e;
            }

            Assert.IsNotNull(catchedException);
            Assert.IsInstanceOfType(catchedException.InnerException, typeof(EnumInitializationException));
        }


        #region From string

        [TestMethod]
        public void from_string_try_found_nocase()
        {
            SomeEnum result;
            bool found = SomeEnum.TryParse("SecondValue", out result);

            Assert.IsTrue(found);
            Assert.AreEqual(SomeEnum.SecondValue, result);
        }

        [TestMethod]
        public void from_string_try_found_cs()
        {
            SomeEnum result;
            bool found = SomeEnum.TryParse("SecondValue", false, out result);

            Assert.IsTrue(found);
            Assert.AreEqual(SomeEnum.SecondValue, result);
        }

        [TestMethod]
        public void from_string_try_found_ci()
        {
            SomeEnum result;
            bool found = SomeEnum.TryParse("SecondValue", true, out result);

            Assert.IsTrue(found);
            Assert.AreEqual(SomeEnum.SecondValue, result);
        }

        [TestMethod]
        public void from_string_try_notfound_nocase()
        {
            SomeEnum result;
            bool found = SomeEnum.TryParse("SecondValuE", out result);

            Assert.IsFalse(found);
            Assert.AreEqual(default(SomeEnum), result);
        }

        [TestMethod]
        public void from_string_try_notfound_cs()
        {
            SomeEnum result;
            bool found = SomeEnum.TryParse("SecondValuE", false, out result);

            Assert.IsFalse(found);
            Assert.AreEqual(default(SomeEnum), result);
        }

        [TestMethod]
        public void from_string_try_notfound_ci()
        {
            SomeEnum result;
            bool found = SomeEnum.TryParse("SecondValu", true, out result);

            Assert.IsFalse(found);
            Assert.AreEqual(default(SomeEnum), result);
        }

        [TestMethod]
        public void from_string_notry_found_nocase()
        {
            SomeEnum result = SomeEnum.Parse("SecondValue");

            Assert.AreEqual(SomeEnum.SecondValue, result);
        }

        [TestMethod]
        public void from_string_notry_found_cs()
        {
            SomeEnum result = SomeEnum.Parse("SecondValue", false);

            Assert.AreEqual(SomeEnum.SecondValue, result);
        }

        [TestMethod]
        public void from_string_notry_found_ci()
        {
            SomeEnum result = SomeEnum.Parse("SecondValue", true);

            Assert.AreEqual(SomeEnum.SecondValue, result);
        }

        [TestMethod]
        public void from_string_notry_notfound_nocase()
        {
            Exception caughtException = null;
            SomeEnum result = SomeEnum.FirstValue;

            try
            {
                result = SomeEnum.Parse("SecondValuE");
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            Assert.IsNotNull(caughtException);
            Assert.AreEqual(SomeEnum.FirstValue, result);
        }

        [TestMethod]
        public void from_string_notry_notfound_cs()
        {
            Exception caughtException = null;
            SomeEnum result = SomeEnum.FirstValue;

            try
            {
                result = SomeEnum.Parse("SecondValuE", false);
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            Assert.IsNotNull(caughtException);
            Assert.AreEqual(SomeEnum.FirstValue, result);
        }

        [TestMethod]
        public void from_string_notry_notfound_ci()
        {
            Exception caughtException = null;
            SomeEnum result = SomeEnum.FirstValue;

            try
            {
                result = SomeEnum.Parse("SecondValu", true);
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            Assert.IsNotNull(caughtException);
            Assert.AreEqual(SomeEnum.FirstValue, result);
        }

        #endregion
    }
}
