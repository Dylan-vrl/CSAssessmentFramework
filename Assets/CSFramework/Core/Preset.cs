using UnityEngine;

namespace CSFramework.Core
{
    public abstract class Preset<TPresettable> : ScriptableObject, IPreset
        where TPresettable : IPresettable
    {
        
    }
}
