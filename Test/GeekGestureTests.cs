using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolymorphicEnum;

namespace Test
{
    public class GameOutcome : PolymorphicEnum<GameOutcome>
    {
        public static GameOutcome WIN = Register();
        public static GameOutcome LOSE = Register();
        public static GameOutcome DRAW = Register();
    }

    /** PolymorphicEnum have behavior! **/
    public class GeekGesture : PolymorphicEnum<GeekGesture>
    {
        public static GeekGesture ROCK = Register();
        public static GeekGesture PAPER = Register();
        public static GeekGesture SCISSORS = Register();
        public static GeekGesture SPOCK = Register();
        public static GeekGesture LIZARD = Register();

        // we can implement with the integer representation and modulo tricks !
        public GameOutcome PlayAgainst(GeekGesture other)
        {
            return this == other
                ? GameOutcome.DRAW
                : (GameOutcome)(((other + 5 - this) % 5) % 2);
        }
    }

    [TestClass]
    public class GeekGestureTests
    {
        [TestMethod]
        public void gesture_against_self_is_a_draw()
        {
            Assert.AreEqual(GameOutcome.DRAW, GeekGesture.ROCK.PlayAgainst(GeekGesture.ROCK));
            Assert.AreEqual(GameOutcome.DRAW, GeekGesture.PAPER.PlayAgainst(GeekGesture.PAPER));
            Assert.AreEqual(GameOutcome.DRAW, GeekGesture.SCISSORS.PlayAgainst(GeekGesture.SCISSORS));
            Assert.AreEqual(GameOutcome.DRAW, GeekGesture.SPOCK.PlayAgainst(GeekGesture.SPOCK));
            Assert.AreEqual(GameOutcome.DRAW, GeekGesture.LIZARD.PlayAgainst(GeekGesture.LIZARD));
        }

        [TestMethod]
        public void gesture_against_1st_next_is_a_lose()
        {
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.ROCK.PlayAgainst(GeekGesture.PAPER));
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.PAPER.PlayAgainst(GeekGesture.SCISSORS));
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.SCISSORS.PlayAgainst(GeekGesture.SPOCK));
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.SPOCK.PlayAgainst(GeekGesture.LIZARD));
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.LIZARD.PlayAgainst(GeekGesture.ROCK));
        }

        [TestMethod]
        public void gesture_against_2nd_next_is_a_win()
        {
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.ROCK.PlayAgainst(GeekGesture.SCISSORS));
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.PAPER.PlayAgainst(GeekGesture.SPOCK));
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.SCISSORS.PlayAgainst(GeekGesture.LIZARD));
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.SPOCK.PlayAgainst(GeekGesture.ROCK));
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.LIZARD.PlayAgainst(GeekGesture.PAPER));
        }

        [TestMethod]
        public void gesture_against_3rd_next_is_a_lose()
        {
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.ROCK.PlayAgainst(GeekGesture.SPOCK));
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.PAPER.PlayAgainst(GeekGesture.LIZARD));
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.SCISSORS.PlayAgainst(GeekGesture.ROCK));
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.SPOCK.PlayAgainst(GeekGesture.PAPER));
            Assert.AreEqual(GameOutcome.LOSE, GeekGesture.LIZARD.PlayAgainst(GeekGesture.SCISSORS));
        }

        [TestMethod]
        public void gesture_against_4th_next_is_a_win()
        {
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.ROCK.PlayAgainst(GeekGesture.LIZARD));
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.PAPER.PlayAgainst(GeekGesture.ROCK));
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.SCISSORS.PlayAgainst(GeekGesture.PAPER));
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.SPOCK.PlayAgainst(GeekGesture.SCISSORS));
            Assert.AreEqual(GameOutcome.WIN, GeekGesture.LIZARD.PlayAgainst(GeekGesture.SPOCK));
        }
    }
}
