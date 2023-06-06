using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    
    private void Update()
    {
        transform.Rotate(Vector3.up,speed * Time.deltaTime);
    }
}
