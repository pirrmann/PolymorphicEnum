using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Steps;

namespace Step_03
{
    public abstract class PolymorphicEnum<T, TEnum>
        where T : struct, IComparable<T>, IConvertible
        where TEnum : PolymorphicEnum<T, TEnum>, new()
    {
        public T Ordinal { get; private set; }
        public string Name { get; private set; }

        private static Dictionary<T, TEnum> registeredInstances = new Dictionary<T, TEnum>();

        protected PolymorphicEnum()
        {
        }

        protected static TEnum Register(Nullable<T> ordinal = null)
        {
            return Register<TEnum>(ordinal);
        }

        protected static TEnum Register<TEnumInstance>(
                Nullable<T> ordinal = null)
            where TEnumInstance : TEnum, new()
        {
            if (!ordinal.HasValue)
            {
                ordinal = registeredInstances.Any()
                    ? registeredInstances.Keys.Max().PlusOne()
                    : default(T);
            }

            TEnum instance = new TEnumInstance();
            instance.Ordinal = ordinal.Value;

            registeredInstances.Add(ordinal.Value, instance);

            return instance;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public abstract class PolymorphicEnum<TEnum> : PolymorphicEnum<int, TEnum>
        where TEnum : PolymorphicEnum<int, TEnum>, new()
    {
    }

    public class SomeEnum : PolymorphicEnum<SomeEnum>
    {
        public static SomeEnum FirstValue = Register();
        public static SomeEnum SecondValue = Register();
    }
}