using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private InputActionReference reference;

    private void Update()
    {
        Debug.Log(reference.action.enabled);
    }
}
