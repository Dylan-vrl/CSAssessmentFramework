using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodRotator : MonoBehaviour
{
    public float rotationSpeed = 1.5f;
    private Vector3 _rotation;
    private bool leftRotate;
    private bool rightRotate;

    void Start() 
    {
        leftRotate = false;
        rightRotate = false;

    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }

    public void LeftRotate() {
        leftRotate = !leftRotate;
        if(leftRotate) 
            _rotation = new Vector3(0, 0, 1 * rotationSpeed);
        else
            _rotation = Vector3.zero;
    }

    public void RightRotate() {
        rightRotate = ! rightRotate;
        if(rightRotate) 
            _rotation = new Vector3(0, 0, -1 * rotationSpeed);
        else
            _rotation = Vector3.zero;
    }

}
