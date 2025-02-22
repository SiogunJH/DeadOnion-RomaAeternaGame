using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class CameraManager : MonoBehaviourSingleton<CameraManager>
{
#if UNITY_EDITOR
    [Tab("Camera Manager")]
#endif
    [SerializeField]
    private Camera _cam;
    public bool CameraIsNull => (_cam == null);

    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Vector3 _position;
#if UNITY_EDITOR
    [EndTab]
#endif

    protected override void Awake()
    {
        base.Awake();
        if (CameraIsNull)
        {
            _cam = Camera.main;
        }
        PositionCamera(_position);
        _position = _cam.transform.position;
    }

    public void CameraLookAt(Transform target)
    {
        if (CameraIsNull)
        {
            Debug.LogWarning("Camera is not assigned");
            return;
        }
        if (target == null) return;

        _target = target;
        _cam.transform.LookAt(target);
    }

    public void PositionCamera(Vector3 position)
    {
        if(position == null) return;

        _cam.transform.position = position;
        _position = position;

        CameraLookAt(_target);
    }
}
