using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolymorphicEnum;

namespace Test
{
    public class AuthorityLevel : PolymorphicEnum<AuthorityLevel>
    {
        /** make decision as the manager */
        public static AuthorityLevel TELL = Register();

        /** convince people about decision */
        public static AuthorityLevel SELL = Register();

        /** get input from team before decision */
        public static AuthorityLevel CONSULT = Register();

        /** make decision together with team */
        public static AuthorityLevel AGREE = Register();

        /** influence decision made by the team */
        public static AuthorityLevel ADVISE = Register();

        /** ask feedback after decision by team */
        public static AuthorityLevel INQUIRE = Register();

        /** no influence, let team work it out */
        public static AuthorityLevel DELEGATE = Register();

        // It's ok to use the internal ordinal integer for the implementation
        public int NumberOfPoints()
        {
            return this.Ordinal + 1;
        }

        // It's ok to use the internal ordinal integer for the implementation
        public bool IsControlOriented()
        {
            return this.Ordinal < AGREE.Ordinal;
        }

        // EnumSet is a Set implementation that benefits from the integer-like
        // nature of the enums
        public static ISet<AuthorityLevel> DELEGATION_LEVELS = EnumSet.Range(ADVISE, DELEGATE);

        // enums are comparable hence the usual benefits
        public static AuthorityLevel Highest(IEnumerable<AuthorityLevel> levels)
        {
            return levels.Max();
        }
    }

    [TestClass]
    public class PokerTests
    {
        [TestMethod]
        public void votes_with_a_clear_majority()
        {
            var a = AuthorityLevel.DELEGATION_LEVELS;

            //// Using an EnumMap to represent the votes by authority level
            //Dictionary<AuthorityLevel, int> votes = new EnumMap(AuthorityLevel.class);
            //votes.put(SELL, 1);
            //votes.put(ADVISE, 3);
            //votes.put(INQUIRE, 2);
            //assertThat(votes.get(ADVISE)).isEqualTo(3);
        }
    }
}
