using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    [SerializeField]
    private float smooth = 5F;
    [SerializeField]
    private float lookAhead = 0F;
 
    Quaternion _lastRotation;
    Quaternion _goalRotation;
    private Transform _lookAt;
    private void Awake ()
    {
        _lastRotation = transform.rotation;
        _goalRotation = _lastRotation;
        _lookAt = Camera.main.transform;
    }
    
    private void FixedUpdate ()
    {
        Debug.DrawLine(_lookAt.position, transform.position, Color.gray);
 
        Vector3 difference = _lookAt.TransformPoint(new Vector3(lookAhead, 0F, 0F)) - transform.position;
        Vector3 upVector = _lookAt.position - _lookAt.TransformPoint(Vector3.down);
        _goalRotation = Quaternion.LookRotation(difference, upVector);
    }

    private void LateUpdate ()
    {
        _lastRotation = Quaternion.Slerp (_lastRotation, _goalRotation, smooth * Time.deltaTime);
        transform.rotation = _lastRotation;
    }
}