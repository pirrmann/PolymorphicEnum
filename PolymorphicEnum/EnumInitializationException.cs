using System;

namespace PolymorphicEnum
{
    public class EnumInitializationException : Exception
    {
        public EnumInitializationException(string message)
            : base(message)
        {
        }
    }
}
