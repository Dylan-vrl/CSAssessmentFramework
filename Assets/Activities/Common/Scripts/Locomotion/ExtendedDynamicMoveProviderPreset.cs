using CSFramework.Core;
using CSFramework.Presettables;
using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider;
using static UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider.MovementDirection;

// ReSharper disable InconsistentNaming

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Locomotion/ExtendedDynamicMoveProviderPreset", fileName = "new ExtendedDynamicMoveProviderPreset")]
    public class ExtendedDynamicMoveProviderPreset : Preset<ExtendedDynamicMoveProvider>
    {
        [field: SerializeField] public float MoveSpeed { get; private set; } = 3f;

        [field: SerializeField] public bool EnableStrafe { get; private set; } = true;

        [field: SerializeField] public MovementDirection LeftHandMovementDirection { get; private set; } = HeadRelative;
        
        [field: SerializeField] public MovementDirection RightHandMovementDirection { get; private set; } = HeadRelative;
        
        [field: SerializeField, 
                Tooltip("Whether to fix stair effect when going downhill.")] 
        public bool FixDownHill { get; private set; } = true;
    }
}
