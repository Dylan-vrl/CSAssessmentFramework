using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public bool IsRotating { get; set; } = false;

    private void Update()
    {
        if(IsRotating)
            transform.Rotate(Vector3.up * (360 * Time.deltaTime), Space.Self);
    }
}
