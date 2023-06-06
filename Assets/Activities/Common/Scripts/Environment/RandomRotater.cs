using UnityEngine;

public class RandomRotater : MonoBehaviour
{
	public Vector3 rotationAxis;
	public static float maxSpeed = 0.2f;

	void Start()
	{
        rotationAxis = Random.insideUnitSphere;
	}

    void Update()
	{
        transform.Rotate(rotationAxis * maxSpeed);
	}
    
}
