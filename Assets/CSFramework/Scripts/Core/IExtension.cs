using System;

namespace CSFramework.Core
{
    public interface IExtension
    {
        public ExtensionCategory Category { get; }
        public Type ExtendedType { get; }
    }
}
