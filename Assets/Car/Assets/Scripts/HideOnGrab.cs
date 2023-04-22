using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HideOnGrab : MonoBehaviour
{
    [SerializeField] private XRBaseInteractor _interactor;
    [SerializeField] private LayerMask _disableLayers;
    [SerializeField] private GameObject _GFX;

    private List<GameObject> _currentlyInside;

    public UnityEvent OnStartGrab;
    public UnityEvent OnStopGrab;

    private void Start()
    {
        _currentlyInside = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_interactor.hasSelection)
        {
            if(_disableLayers == (_disableLayers | (1 << other.gameObject.layer)))
            {
                _GFX?.SetActive(false);
                OnStartGrab.Invoke();
                _currentlyInside.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentlyInside.Contains(other.gameObject))
        {
            _currentlyInside.Remove(other.gameObject);
        }

        if(_currentlyInside.Count == 0)
        {
            OnStopGrab?.Invoke();
            _GFX.SetActive(true);
        }
    }
}
