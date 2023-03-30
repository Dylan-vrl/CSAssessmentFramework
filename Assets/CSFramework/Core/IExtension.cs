using System;

namespace CSFramework.Core
{
    public interface IExtension: IPresettable
    {
        public Type ExtendedType { get; } 
    }
}
