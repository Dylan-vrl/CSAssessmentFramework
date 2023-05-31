using CSFramework.Presettables;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public class ExperimentLengthInput : MonoBehaviour
{
    private TMPro.TMP_InputField input;

    private void Awake()
    {
        input = GetComponent<TMPro.TMP_InputField>();
    }

    private void OnEnable()
    {
        input.onValueChanged.AddListener(ModifyLength);
    }

    private void OnDisable()
    {
        input.onValueChanged.RemoveListener(ModifyLength);
    }

    private void ModifyLength(string length)
    {
        if (int.TryParse(length, out int value))
        {
            ExperimentController.Instance.ExperimentLength = value;
        }
    }
}
