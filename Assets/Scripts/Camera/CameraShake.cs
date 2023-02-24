using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float _cameraMoveSpeed = 0.5f;
    [SerializeField] float _cameraKickMoveSpeed = 0.1f;
    [SerializeField] Vector3 _targetPos;
    [SerializeField] Vector3 _shootTargetPos;
    Vector3 _currentPosition;
    void OnEnable()
    {
        EventManager.OnKickBack += CameraShootBounce;
        EventManager.OnPrepareToThrow += CameraBounce;    
        EventManager.OnAfterThrow += CameraToNormalPosition;
    }

    void Start()
    {
        _currentPosition = transform.position;
        _targetPos = new Vector3(0,49,-8.1f);
        _shootTargetPos = new Vector3(0,48.5f,-8.1f);
    }

    void CameraBounce()
    {
        transform.DOMove(_targetPos,_cameraMoveSpeed).SetEase(Ease.Linear);
    }

    void CameraToNormalPosition()
    {
        transform.DOMove(_currentPosition,_cameraMoveSpeed).SetEase(Ease.Linear);
    }

    void CameraShootBounce()
    {
        transform.DOMove(_shootTargetPos,_cameraKickMoveSpeed).SetEase(Ease.InOutBounce).
        OnComplete(() => transform.position = _currentPosition);
    }

    void OnDisable()
    {
        EventManager.OnKickBack -= CameraShootBounce;
        EventManager.OnPrepareToThrow -= CameraBounce;
        EventManager.OnAfterThrow -= CameraToNormalPosition;
    }

}
