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
        [SerializeField] public bool rotationalTest;
        [SerializeField] public bool linearTest;

        [SerializeField] public int lapsPerAxis;
        [SerializeField] public float lapDurationPerAxis;
        [SerializeField] public float waitDurationBtwEachTurn;
        [SerializeField] public float waitDurationBtw3AxisTurns;
        [SerializeField] public float linearDistance;
    }
}