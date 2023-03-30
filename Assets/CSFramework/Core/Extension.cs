using System;

namespace CSFramework.Core
{
    public abstract class Extension<TExtended, TPreset> : PresettableMonoBehaviour<TPreset>, IExtension
        where TPreset: IPreset
    {
        public Type ExtendedType => typeof(TExtended);
    }
}
