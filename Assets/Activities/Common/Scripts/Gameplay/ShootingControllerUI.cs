using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class ShootingControllerUI : MonoBehaviour
{
    [SerializeField] private FollowPath followPath;
    [SerializeField] private TMP_InputField speedInput;
    [SerializeField] private TextMeshProUGUI speedText;

    private void OnEnable()
    {
        speedInput.onSubmit.AddListener(SpeedInputChange);
    }

    private void OnDisable()
    {
        speedInput.onSubmit.AddListener(SpeedInputChange);
    }

    private void Start()
    {
        speedText.text = followPath.Speed.ToString(CultureInfo.InvariantCulture);
    }

    private void SpeedInputChange(string input)
    {
        if (int.TryParse(input, out var i))
        {
            followPath.Speed = i;
            speedText.text = followPath.Speed.ToString(CultureInfo.InvariantCulture);
        }
    }
}
