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
    public class LocomotionHandlerPreset : Preset<LocomotionHandler>
    {
        [SerializeField] private LocomotionProvider[] leftActiveLocomotionProviders;
        [SerializeField] private LocomotionProvider[] rightActiveLocomotionProviders;
        [SerializeField] private GameObject leftControllerPrefab;
        [SerializeField] private GameObject rightControllerPrefab;
        public List<LocomotionProvider> LeftActiveLocomotionProviders => leftActiveLocomotionProviders.ToList();
        public List<LocomotionProvider> RightActiveLocomotionProviders => rightActiveLocomotionProviders.ToList();
        public GameObject LeftControllerPrefab => leftControllerPrefab;
        public GameObject RightControllerPrefab => rightControllerPrefab;

        // Used for custom inspector
        [HideInInspector]
        public UnityEvent OnListChange = new();

        private void OnValidate()
        {
            OnListChange?.Invoke();
        }
    }
}
