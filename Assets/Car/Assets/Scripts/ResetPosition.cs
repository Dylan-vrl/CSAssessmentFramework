using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    [SerializeField] private Transform _resetTransform;
    [SerializeField] private GameObject _player;
    [SerializeField] private Camera _playerHead;

    void Start()
    {
        ResetPose();
    }

    public void ResetPose()
    {
        var rotationAngleY = _resetTransform.rotation.eulerAngles.y - _playerHead.transform.rotation.eulerAngles.y;
        _player.transform.Rotate(0, rotationAngleY, 0);

        var distanceDiff = _resetTransform.position - _playerHead.transform.position;
        _player.transform.position += distanceDiff;
    }
}
