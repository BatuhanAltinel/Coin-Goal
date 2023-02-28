using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    Animator anim;

    [SerializeField] GameObject _goalKeeper;
    [SerializeField] Transform _rightWayPoint;
    [SerializeField] Transform _leftWayPoint;
    Transform targetWayPoint;
    Vector3 _startPosition;
    Quaternion _startRotation;
    [SerializeField] float _moveSpeed;
    float elapsedTime = 0;


    void OnEnable()
    {
        EventManager.OnGoal += PlaySadAnim;    
        EventManager.OnRestartLevel += PlaySideWalkAnim;
        EventManager.OnNextLevel += PlaySideWalkAnim;
        EventManager.OnRestartLevel += OnRestart;
        EventManager.OnNextLevel += OnRestart;
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("IsGoal",false);
        targetWayPoint = _rightWayPoint;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    void ChangeWayPoint()
    {
        if(targetWayPoint == _rightWayPoint)
            targetWayPoint = _leftWayPoint;
        else
            targetWayPoint = _rightWayPoint;
    }

    void Update()
    {
        if(!GameManager.Instance.IsGoal)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _moveSpeed;
            
            _goalKeeper.transform.position = Vector3.MoveTowards(_goalKeeper.transform.position,targetWayPoint.position,_moveSpeed);

            if(Vector3.Distance(_goalKeeper.transform.position,targetWayPoint.position) < 0.05f )
            {
                ChangeWayPoint();
                elapsedTime = 0;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Coin>(out Coin coin) && !GameManager.Instance.IsGoal)
        {
            if(coin.coinState == CoinStates.powerUp)
            {
                EventManager.OnCoinStateChanged.Invoke(CoinStates.normal);
                OnCrushPowerUpCoin();
            }else
            {
                float randomVectorY = Random.Range(1,0);
                float randomVectorX = Random.Range(-1,1);

                float randomPower = Random.Range(600,750);
                coin.MoveTo(new Vector2(randomVectorX,randomVectorY),randomPower);
                EventManager.OnKickBack.Invoke();
            }
            
        }
    }

    void OnRestart()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
    }

    void OnCrushPowerUpCoin()
    {
        StartCoroutine(OnCrushPowerUpCoinRoutine());
    }

    IEnumerator OnCrushPowerUpCoinRoutine()
    {
        Vector3 targetPos = new Vector3(50,50,transform.position.z);
        Quaternion targetRot = Quaternion.Euler(-155,180,-128);

        while(Vector3.Distance(transform.position,targetPos) > 1f)
        {
            transform.position = Vector3.Lerp(transform.position,targetPos,0.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRot,0.5f);

            yield return new WaitForEndOfFrame();
        }
        transform.position = targetPos;
        transform.rotation = targetRot;
        yield break;
    }

    void PlaySadAnim()
    {
        anim.SetBool("IsGoal",true);
    }
    void PlaySideWalkAnim()
    {
        anim.SetBool("IsGoal",false);
    }

    void OnDisable()
    {
        EventManager.OnGoal -= PlaySadAnim;    
        EventManager.OnRestartLevel -= PlaySideWalkAnim;
        EventManager.OnNextLevel -= PlaySideWalkAnim;
        EventManager.OnRestartLevel -= OnRestart;
        EventManager.OnNextLevel -= OnRestart;
    }
}
