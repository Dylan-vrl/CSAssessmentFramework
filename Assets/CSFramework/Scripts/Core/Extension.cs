using System;
using UnityEngine;

namespace CSFramework.Core
{
    public abstract class Extension<TExtended, TPreset> : MonoBehaviour, IExtension 
        where TPreset: IPreset
    {
        [SerializeField] 
        private TPreset preset;
        public TPreset Preset
        {
            get => preset;
            protected set => preset = value;
        }
        public abstract ExtensionCategory Category { get; }
        public Type ExtendedType => typeof(TExtended);
    }
}
