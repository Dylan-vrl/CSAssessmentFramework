using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PressableButton : MonoBehaviour
{
    [SerializeField] private GameObject _buttonGO;
    [SerializeField] private float _heightOffset = 0.5f;
    public UnityEvent OnPress;
    public UnityEvent OnRelease;

    private List<GameObject> _currentPressers;

    private void Start()
    {
        _currentPressers = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(_currentPressers.Count == 0)
        {
            _buttonGO.transform.localPosition -= Vector3.up * _heightOffset;
        }
        _currentPressers.Add(other.gameObject);

        OnPress?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentPressers.Contains(other.gameObject))
        {
            _currentPressers.Remove(other.gameObject);
        }

        if(_currentPressers.Count == 0)
        {
            _buttonGO.transform.localPosition += Vector3.up * _heightOffset;
            OnRelease?.Invoke();
        }
    }

}
