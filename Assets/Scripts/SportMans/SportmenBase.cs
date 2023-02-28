using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportmenBase : MonoBehaviour
{

    public Transform _rightWayPoint;
    public Transform _leftWayPoint;
    public Transform targetWayPoint;
    public Transform _positionTransform;

    public Vector3 _startPosition;
    public Quaternion _startRotation;

    public float _moveSpeed;

    public Animator _anim;

    public Vector2 _randomShootVector;
    public float _maxShootPower = 1100f;

    void OnEnable()
    {
        EventManager.OnGoal += PlaySadAnim;
        EventManager.OnRestartLevel += PlayIdleAnim;
        EventManager.OnNextLevel += PlayIdleAnim;
        EventManager.OnRestartLevel += OnRestart;
        EventManager.OnNextLevel += OnRestart;
    }

    public void OnStart()
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("IsGoal",false);
        targetWayPoint = _rightWayPoint;
        _startPosition = _positionTransform.position;
        _startRotation = _positionTransform.rotation;
    }

    public void OnUpdate()
    {
        MoveToWayPoint();
    }

    public void MoveToWayPoint()
    {
        if(!GameManager.Instance.IsGoal)
        {
            
            transform.position = Vector3.MoveTowards(transform.position,targetWayPoint.position,_moveSpeed);

            if(Vector3.Distance(transform.position,targetWayPoint.position) < 0.05f )
            {
                ChangeWayPoint();
            }
        }
    }

    void ChangeWayPoint()
    {
        if(targetWayPoint == _rightWayPoint)
            targetWayPoint = _leftWayPoint;
        else
            targetWayPoint = _rightWayPoint;
    }

    public void TurnToTheCoin()
    {
        if(CoinManager.Instance.PreviousCoin != null && !GameManager.Instance.IsGoal)
        {
            transform.parent.LookAt(CoinManager.Instance.PreviousCoin.transform,Vector3.up);
        }
    }

    public void OnTouchCoin(Collider other)
    {
        if(other.gameObject.TryGetComponent<Coin>(out Coin coin) && !GameManager.Instance.IsGoal)
        {
            if(coin.coinState == CoinStates.powerUp)
            {
                EventManager.OnCoinStateChanged.Invoke(CoinStates.normal);
                OnCrushPowerUpCoin();
            }else
            {
                RandomShootVector();
                PlayShootAnim();

                coin.MoveTo(_randomShootVector,_maxShootPower);
                EventManager.OnKickBack.Invoke();
                Invoke("PlayIdleAnim",0.2f);
            }
        }
    }

    public virtual void OnCrushPowerUpCoin()
    {
        StartCoroutine(OnCrushPowerUpCoinRoutine());
    }

    IEnumerator OnCrushPowerUpCoinRoutine()
    {
        Vector3 targetPos = new Vector3(50,50,transform.position.z);
        Quaternion targetRot = Quaternion.Euler(-155,180,-128);

        while(Vector3.Distance(transform.position,targetPos) > 1f)
        {
            _positionTransform.position = Vector3.Lerp(_positionTransform.position,targetPos,0.1f);
            _positionTransform.rotation = Quaternion.Lerp(_positionTransform.rotation,targetRot,0.1f);

            yield return new WaitForEndOfFrame();
        }
        _positionTransform.position = targetPos;
        _positionTransform.rotation = targetRot;
        yield break;
    }  

    public void PlayIdleAnim()
    {
        _anim.SetBool("IsCoinCollision",false);
        _anim.SetBool("IsGoal",false);
    }

    public void PlayShootAnim()
    {
        _anim.SetBool("IsCoinCollision",true);
    }

    public void PlaySadAnim()
    {
        _anim.SetBool("IsGoal",true);
    }

    public void RandomShootVector()
    {
        float randomVectorY = Random.Range(1,0);
        float randomVectorX = Random.Range(-0.5f,0.5f);

        _randomShootVector = new Vector2(randomVectorX,randomVectorY);
    }

    public virtual void OnRestart()
    {
        _positionTransform.position = _startPosition;
        _positionTransform.rotation = _startRotation;

    }

    void OnDisable()
    {
        EventManager.OnGoal -= PlaySadAnim;
        EventManager.OnRestartLevel -= PlayIdleAnim;
        EventManager.OnNextLevel -= PlayIdleAnim;
        EventManager.OnRestartLevel -= OnRestart;
        EventManager.OnNextLevel -= OnRestart;
    }

}

