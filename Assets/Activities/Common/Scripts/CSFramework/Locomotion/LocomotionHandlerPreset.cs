using System;
using System.Collections.Generic;
using System.Linq;
using CSFramework.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Locomotion/LocomotionHandlerPreset", fileName = "new LocomotionHandlerPreset")]
    public class LocomotionHandlerPreset : Preset<LocomotionHandler2>
    {
        [SerializeField] private LocomotionProvider[] leftActiveLocomotionProviders;
        [SerializeField] private LocomotionProvider[] rightActiveLocomotionProviders;
        public List<LocomotionProvider> LeftActiveLocomotionProviders => leftActiveLocomotionProviders.ToList();
        public List<LocomotionProvider> RightActiveLocomotionProviders => rightActiveLocomotionProviders.ToList();

        public UnityEvent OnListChange = new UnityEvent();

        private void OnValidate()
        {
            OnListChange?.Invoke();
        }
    }
}
