using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class RodFrameTest : MonoBehaviour
{
    private GameObject rod;
    private GameObject frame;
    
    private List<string> storedVals;
    private List<int> shuffledCards;
    private int index;
    private static int LAP_COUNT = 16;

    // Start is called before the first frame update
    void Start()
    {
        rod = GameObject.Find("Rod");
        frame = GameObject.Find("Frame");

        storedVals = new List<string>();

        int[] nums = {1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4};
        index = 0;
        var rng = new System.Random();
        shuffledCards = nums.OrderBy(a => rng.Next()).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        storedVals.Add("Case " + shuffledCards[index].ToString() + ": " + string.Format("{0:N1}", rod.transform.eulerAngles.z));
        if (index < LAP_COUNT) {
            switch (shuffledCards[index]) {   
                case 1:
                    // WriteString(string.Format("{0:N1}", rod.transform.eulerAngles.z) + ",case 1");
                    frame.transform.eulerAngles = new Vector3(0, 0, -18);
                    rod.transform.eulerAngles = new Vector3(0, 0, -27);
                    break;

                case 2:
                    //WriteString(string.Format("{0:N1}", rod.transform.eulerAngles.z) + ",case 2");
                    frame.transform.eulerAngles = new Vector3(0, 0, -18);
                    rod.transform.eulerAngles = new Vector3(0, 0, 27);
                    break;

                case 3:
                    //WriteString(string.Format("{0:N1}", rod.transform.eulerAngles.z) + ",case 3");
                    frame.transform.eulerAngles = new Vector3(0, 0, 18);
                    rod.transform.eulerAngles = new Vector3(0, 0, -27);
                    break;

                case 4:
                    //WriteString(string.Format("{0:N1}", rod.transform.eulerAngles.z) + ",case 4");
                    frame.transform.eulerAngles = new Vector3(0, 0, 18);
                    rod.transform.eulerAngles = new Vector3(0, 0, 27);
                    break;
            }
            index++;
        } else {
            Debug.Log(storedVals);
            Debug.Log("Session Ended.");
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
    }
}
