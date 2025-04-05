using Cinemachine;
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
    public bool CameraIsNull => (_defaultCamera == null);

    [Header("Default")]
    [SerializeField]
    private Transform _defaultTarget;
    [SerializeField]
    private Vector3 _defaultPosition;

    [Header("Animation")]
    [SerializeField]
    private Vector3 _animationPosition;
    [SerializeField]
    private Quaternion _animationTilt;


    [Header("Cameras")]
    [SerializeField]
    private CinemachineVirtualCamera _defaultCamera;
    [SerializeField]
    private CinemachineVirtualCamera _animationCamera;

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
        PositionCamera(_defaultPosition);
        _defaultPosition = _defaultCamera.transform.position;

        _animationCamera.gameObject.transform.position = _animationPosition;
        _animationCamera.gameObject.transform.rotation = _animationTilt;
    }
    private void Start()
    {
        SwitchMainCamera();
    }

    public void CameraLookAt(Transform target)
    {
        if (CameraIsNull)
        {
            Debug.LogWarning("Camera is not assigned");
            return;
        }
        if (target == null) return;

        _defaultTarget = target;
        _defaultCamera.transform.LookAt(target);
    }

    public void PositionCamera(Vector3 position)
    {
        if (CameraIsNull)
        {
            Debug.LogWarning("Camera is not assigned");
            return;
        }
        if(position == null) return;

        _defaultCamera.transform.position = position;
        _defaultPosition = position;

        CameraLookAt(_defaultTarget);
    }


    
    
    public void SwitchMainCamera()
    {
        ChangeCamera(_defaultCamera);
    }
    public void SwitchAnimationCamera()
    {
        ChangeCamera(_animationCamera);
    }
    private void ChangeCamera(CinemachineVirtualCamera cam)
    {
        _defaultCamera.Priority = 0;
        _animationCamera.Priority = 0;

        cam.Priority = 1;
    }
    public bool AnimationCameraInPosition()
    {
        return _cam.transform.position == _animationCamera.transform.position;
    }
}
