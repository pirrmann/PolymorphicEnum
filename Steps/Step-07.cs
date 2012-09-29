using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using PolymorphicEnum;
using Steps;

namespace Step_07
{
    public abstract class PolymorphicEnum<T, TEnum>
        where T : struct, IComparable<T>, IConvertible
        where TEnum : PolymorphicEnum<T, TEnum>, new()
    {
        private static bool namesInitialized = false;
        private static Dictionary<T, TEnum> registeredInstances = new Dictionary<T, TEnum>();

        public T Ordinal { get; private set; }
        protected object Data { get; private set; }

        private string name = null;
        private string Name
        {
            get
            {
                EnsureNamesInitialized();
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        protected PolymorphicEnum()
        {
        }

        protected void EnsureNamesInitialized()
        {
            if (!namesInitialized)
            {
                MemberInfo[] enumMembers = typeof(TEnum).GetMembers(
                        BindingFlags.Public
                        | BindingFlags.Static
                        | BindingFlags.GetField);

                foreach (FieldInfo enumMember in enumMembers)
                {
                    TEnum enumValue =
                        enumMember.GetValue(null) as TEnum;

                    if(enumValue != null)
                        enumValue.Name = enumMember.Name;
                }
                namesInitialized = true;
            }
        }

        protected static TEnum Register(
            Nullable<T> ordinal = null,
            object data = null)
        {
            return Register<TEnum>(ordinal, data);
        }

        protected static TEnum Register<TEnumInstance>(
            Nullable<T> ordinal = null,
            object data = null)
        where TEnumInstance : TEnum, new()
        {
            StackFrame frame = new StackFrame(1);
            if (frame.GetMethod().Name == "Register")
                frame = new StackFrame(2);

            MethodBase enumConstructor = frame.GetMethod();
            if (enumConstructor.DeclaringType != typeof(TEnum))
                throw new EnumInitializationException(
                    "Enum members cannot be registered from other enums.");

            if (!ordinal.HasValue)
            {
                ordinal = registeredInstances.Any()
                    ? registeredInstances.Keys.Max().PlusOne()
                    : default(T);
            }

            TEnum instance = new TEnumInstance();
            instance.Ordinal = ordinal.Value;
            instance.Data = data;

            registeredInstances.Add(ordinal.Value, instance);

            namesInitialized = false;

            return instance;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public static implicit operator T(PolymorphicEnum<T, TEnum> x)
        {
            return x.Ordinal;
        }

        public static explicit operator PolymorphicEnum<T, TEnum>(T x)
        {
            TEnum enumInstance;
            if (!registeredInstances.TryGetValue(x, out enumInstance))
                throw new ArgumentException(
                    string.Format("PolymorphicEnum value {0} not found", x, "x"));

            return enumInstance;
        }

        public static bool TryParse(string value, out TEnum result)
        {
            return TryParse(value, false, out result);
        }

        public static bool TryParse(string value,
                                    bool ignoreCase,
                                    out TEnum result)
        {
            TEnum[] instances = registeredInstances
                .Values
                .Where(
                    e => e.Name.Equals(
                        value,
                        ignoreCase
                            ? StringComparison.InvariantCultureIgnoreCase
                            : StringComparison.InvariantCulture))
                .ToArray();

            if (instances.Length == 1)
            {
                result = instances[0];
                return true;
            }
            else
            {
                result = default(TEnum);
                return false;
            }
        }

        public static TEnum Parse(string value)
        {
            return Parse(value, false);
        }

        public static TEnum Parse(string value, bool ignoreCase)
        {
            TEnum result;

            if (!TryParse(value, ignoreCase, out result))
                throw new ArgumentException(string.Format("PolymorphicEnum value {0} not found", value, "value"));

            return result;
        }

        #region Consistency enforcement

        private bool IsRegistered
        {
            get { return registeredInstances.Values.Contains(this); }
        }

        protected TEnum Checked(TEnum value)
        {
            if (!value.IsRegistered)
                throw new UnregisteredEnumException(
                    "This enum is not registered");

            return value;
        }

        protected void Checked(Action a)
        {
            if (!IsRegistered)
                throw new UnregisteredEnumException(
                    "This enum is not registered");

            a.Invoke();
        }

        protected TReturn Checked<TReturn>(Func<TReturn> f)
        {
            if (!IsRegistered)
                throw new UnregisteredEnumException(
                    "This enum is not registered");

            return f.Invoke();
        }

        #endregion
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