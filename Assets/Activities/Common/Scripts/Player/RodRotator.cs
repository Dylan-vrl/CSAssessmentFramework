using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodRotator : MonoBehaviour
{
    private Vector3 _rotation;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }

    public void LeftRotate(bool rotate) {
        if(rotate) 
            _rotation = new Vector3(0, 0, 1);
        else
            _rotation = Vector3.zero;
    }

    public void RightRotate(bool rotate) {
        if(rotate) 
            _rotation = new Vector3(0, 0, -1);
        else
            _rotation = Vector3.zero;
    }

}
