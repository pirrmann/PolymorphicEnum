using System;
using System.Collections.Generic;

namespace Steps
{
    public static class Incrementer
    {
        private static Dictionary<Type, object> incrementers;

        static Incrementer()
        {
            incrementers = new Dictionary<Type, object>();
            incrementers.Add(typeof(sbyte), GetFunc<sbyte>(i => (sbyte)(i + 1)));
            incrementers.Add(typeof(byte), GetFunc<byte>(i => (byte)(i + 1)));
            incrementers.Add(typeof(short), GetFunc<short>(i => (short)(i + 1)));
            incrementers.Add(typeof(ushort), GetFunc<ushort>(i => (ushort)(i + 1)));
            incrementers.Add(typeof(int), GetFunc<int>(i => i + 1));
            incrementers.Add(typeof(uint), GetFunc<uint>(i => i + 1));
            incrementers.Add(typeof(long), GetFunc<long>(i => i + 1));
            incrementers.Add(typeof(ulong), GetFunc<ulong>(i => i + 1));
        }

        private static Func<T, T> GetFunc<T>(Func<T, T> f)
        {
            return f;
        }

        public static T PlusOne<T>(this T value)
            where T : struct
        {
            object incrementer;
            if (!incrementers.TryGetValue(typeof(T), out incrementer))
                throw new NotSupportedException("This type is not supported.");

            return ((Func<T, T>)incrementer).Invoke(value);
        }
    }

}
