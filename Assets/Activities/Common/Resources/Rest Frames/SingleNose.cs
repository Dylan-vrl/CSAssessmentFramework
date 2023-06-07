using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//double nose -.19, -.16, .45
//scale .1

//single nose z: .4 - .8
//single nose 
// light skin color: E8BEAC, (232, 190, 172)

public class SingleNose : MonoBehaviour
{
    [SerializeField] private float noseWidth;
    [SerializeField] private float noseFlatness;

    [SerializeField] public float NoseWidth { get { return noseWidth; } set { noseWidth = value; } }
    [SerializeField] public float NoseFlatness { get { return noseFlatness; } set { noseFlatness = value; } }

    void Start()
    {
        float xScale = Mathf.Lerp(0.05f, .15f, noseWidth);
        float yScale = Mathf.Lerp(0.05f, .25f, 1 - noseFlatness);
        float zScale = Mathf.Lerp(.03f, .15f, .5f);

        transform.localScale = new Vector3(xScale, yScale, zScale);
    }

}
