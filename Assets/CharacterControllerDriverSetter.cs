using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterControllerDriver))]
public class CharacterControllerDriverSetter : MonoBehaviour
{
    [SerializeField] 
    private ContinuousMoveProviderEventChannelSO continuousMoveProviderEnableChannelSO;

    private CharacterControllerDriver _driver;
    
    private void Awake()
    {
        _driver = GetComponent<CharacterControllerDriver>();
    }

    private void OnEnable()
    {
        continuousMoveProviderEnableChannelSO.OnEventRaised += OnMoveProviderEnable;
    }

    private void OnDisable()
    {
        continuousMoveProviderEnableChannelSO.OnEventRaised -= OnMoveProviderEnable;
    }

    private void OnMoveProviderEnable(ContinuousMoveProviderBase moveProvider)
    {
        _driver.locomotionProvider = moveProvider;
    }
}
