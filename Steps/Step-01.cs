using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Step_01
{
    public abstract class PolymorphicEnum<T>
        where T : struct, IComparable<T>, IConvertible
    {
        public T Ordinal { get; private set; }
        public string Name { get; private set; }

        protected PolymorphicEnum()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public abstract class PolymorphicEnum : PolymorphicEnum<int>
    {
    }

    public class SomeEnum : PolymorphicEnum
    {
        public static SomeEnum FirstValue;
        public static SomeEnum SecondValue;
    }
}