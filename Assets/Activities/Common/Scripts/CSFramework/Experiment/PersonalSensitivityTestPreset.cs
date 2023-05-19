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
        [SerializeField] public int lapCount;
        [SerializeField] public float lapDuration;
        [SerializeField] public float waitDuration;
    }
}