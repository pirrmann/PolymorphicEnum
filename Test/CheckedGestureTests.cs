using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolymorphicEnum;

namespace Test
{
    /** PolymorphicEnum have behavior! **/
    public class CheckedGesture : PolymorphicEnum<CheckedGesture>
    {
        public static CheckedGesture ROCK = Register<RockGesture>();
        public static CheckedGesture PAPER = Register();
        public static CheckedGesture SCISSORS = Register();

        // we can implement with the integer representation
        public bool Beats(CheckedGesture other)
        {
            return Checked(() => this.BeatsImpl(Checked(other)));
        }

        protected virtual bool BeatsImpl(CheckedGesture other)
        {
            return this.Ordinal - other.Ordinal == 1;
        }

        private class RockGesture : CheckedGesture
        {
            protected override bool BeatsImpl(CheckedGesture other)
            {
                return other == SCISSORS;
            }
        }
    }

    public class CagedGodzilla : CheckedGesture
    {
        public static CagedGodzilla Instance = new CagedGodzilla();

        protected override bool BeatsImpl(CheckedGesture other)
        {
            return true;
        }
    }

    [TestClass]
    public class CheckedGestureTests
    {
        [TestMethod]
        public void paper_beats_rock_checked()
        {
            Assert.IsTrue(CheckedGesture.PAPER.Beats(CheckedGesture.ROCK));
            Assert.IsFalse(CheckedGesture.ROCK.Beats(CheckedGesture.PAPER));
        }

        [TestMethod]
        public void scissors_beats_paper_checked()
        {
            Assert.IsTrue(CheckedGesture.SCISSORS.Beats(CheckedGesture.PAPER));
            Assert.IsFalse(CheckedGesture.PAPER.Beats(CheckedGesture.SCISSORS));
        }

        [TestMethod]
        public void rock_beats_scissors_checked()
        {
            Assert.IsTrue(CheckedGesture.ROCK.Beats(CheckedGesture.SCISSORS));
            Assert.IsFalse(CheckedGesture.SCISSORS.Beats(CheckedGesture.ROCK));
        }

        [TestMethod]
        public void godzilla_cant_beat_throws_exception()
        {
            UnregisteredEnumException catchedException = null;
            try
            {
                CagedGodzilla.Instance.Beats(CheckedGesture.ROCK);
            }
            catch (UnregisteredEnumException e)
            {
                catchedException = e;
            }

            Assert.IsNotNull(catchedException);
        }

        [TestMethod]
        public void cant_beat_godzilla_throws_exception()
        {
            UnregisteredEnumException catchedException = null;
            try
            {
                CheckedGesture.ROCK.Beats(CagedGodzilla.Instance);
            }
            catch (UnregisteredEnumException e)
            {
                catchedException = e;
            }
            Assert.IsNotNull(catchedException);
        }
    }
}
