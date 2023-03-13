using UnityEngine;

namespace CSFramework.Core
{
    public abstract class PresettableMonoBehaviour<TPreset>: MonoBehaviour, IPresettable<TPreset> 
        where TPreset : IPreset
    {
        [SerializeField] 
        private TPreset preset;
        public TPreset Preset => preset;
        
        public abstract PresettableCategory Category { get; }
    }
}