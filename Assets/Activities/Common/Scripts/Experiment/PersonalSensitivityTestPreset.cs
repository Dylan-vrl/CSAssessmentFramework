using CSFramework.Core;
using CSFramework.Extensions;
using CSFramework.Presettables;
using UnityEngine;

namespace CSFramework.Presets
{
    [CreateAssetMenu(menuName = "CSFramework/Preset Instances/Experiment/PersonalSensitivityTestPreset", fileName = "new PersonalSensitivityTestPreset")]
    public class PersonalSensitivityTestPreset: Preset<PersonalSensitivityTest>
    {
        // TODO replace with your own fields following this format
        //[field: SerializeField]
        //public int Field { get; private set; }
        [SerializeField] public bool insideGameScene;
        [SerializeField] public int turnPerAxis;
        [SerializeField] public float turnDurationPerAxis;
        [SerializeField] public float waitDurationBtwEachTurn;
        [SerializeField] public float waitDurationBtw3AxisTurns;
        [SerializeField] public Vector3 linearDistance;
    }
}