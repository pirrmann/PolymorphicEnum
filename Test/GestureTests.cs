using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolymorphicEnum;

namespace Test
{
    /** PolymorphicEnum have behavior! **/
    public class Gesture : PolymorphicEnum<Gesture>
    {
        public static Gesture ROCK = Register<RockGesture>();
        public static Gesture PAPER = Register();
        public static Gesture SCISSORS = Register();

        // we can implement with the integer representation
        public virtual bool Beats(Gesture other)
        {
            return this.Ordinal - other.Ordinal == 1;
        }

        private class RockGesture : Gesture
        {
            public override bool Beats(Gesture other)
            {
                return other == SCISSORS;
            }
        }
    }

    public class Godzilla : Gesture
    {
        public static Godzilla Instance = new Godzilla();

        public override bool Beats(Gesture other)
        {
            return true;
        }
    }

    [TestClass]
    public class GestureTests
    {
        [TestMethod]
        public void paper_beats_rock()
        {
            Assert.IsTrue(Gesture.PAPER.Beats(Gesture.ROCK));
            Assert.IsFalse(Gesture.ROCK.Beats(Gesture.PAPER));
        }

        [TestMethod]
        public void scissors_beats_paper()
        {
            Assert.IsTrue(Gesture.SCISSORS.Beats(Gesture.PAPER));
            Assert.IsFalse(Gesture.PAPER.Beats(Gesture.SCISSORS));
        }

        [TestMethod]
        public void rock_beats_scissors()
        {
            Assert.IsTrue(Gesture.ROCK.Beats(Gesture.SCISSORS));
            Assert.IsFalse(Gesture.SCISSORS.Beats(Gesture.ROCK));
        }

        [TestMethod]
        public void godzilla_beats_all()
        {
            Assert.IsTrue(Godzilla.Instance.Beats(Gesture.ROCK));
            Assert.IsTrue(Godzilla.Instance.Beats(Gesture.PAPER));
            Assert.IsTrue(Godzilla.Instance.Beats(Gesture.SCISSORS));
        }
    }
}
