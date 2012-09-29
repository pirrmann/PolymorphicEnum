using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace PolymorphicEnum
{
    public static class EnumSet
    {
        public static ISet<TEnum> Range<TEnum>(TEnum from, TEnum to)
            where TEnum : PolymorphicEnum<int, TEnum>, new()
        {
            return new HashSet<TEnum>(
                PolymorphicEnum<int, TEnum>
                    .GetValues()
                    .Where(e => e.Ordinal >= from.Ordinal && e.Ordinal <= to.Ordinal));
        }
    }
}