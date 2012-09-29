using System;

namespace PolymorphicEnum
{
    public class UnregisteredEnumException : Exception
    {
        public UnregisteredEnumException(string message)
            : base(message)
        {
        }
    }
}
