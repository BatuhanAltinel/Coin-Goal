using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportmanRunner : MonoBehaviour
{
    [SerializeField] Transform _rightWayPoint;
    [SerializeField] Transform _leftWayPoint;
    Transform targetWayPoint;

     [SerializeField] float _moveSpeed;
    float elapsedTime = 0;

    Animator _anim;
    Vector2 _randomShootVector;
    float _maxShootPower = 1100f;

     void OnEnable()
    {
        EventManager.OnGoal += PlaySadAnim;
        EventManager.OnRestartLevel += PlayIdleAnim;
        EventManager.OnNextLevel += PlayIdleAnim;
    }
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("IsGoal",false);
        targetWayPoint = _rightWayPoint;
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
            
            transform.position = Vector3.MoveTowards(transform.position,targetWayPoint.position,_moveSpeed);

            if(Vector3.Distance(transform.position,targetWayPoint.position) < 0.05f )
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
            RandomShootVector();
            PlayShootAnim();

            coin.MoveTo(_randomShootVector,_maxShootPower);
            EventManager.OnKickBack.Invoke();
            Invoke("PlayIdleAnim",0.2f);
        }
    }

    void PlayIdleAnim()
    {
        _anim.SetBool("IsCoinCollision",false);
        _anim.SetBool("IsGoal",false);
    }

    void PlayShootAnim()
    {
        _anim.SetBool("IsCoinCollision",true);
    }

    void PlaySadAnim()
    {
        _anim.SetBool("IsGoal",true);
    }

    void RandomShootVector()
    {
        float randomVectorY = Random.Range(1,0);
        float randomVectorX = Random.Range(-0.5f,0.5f);

        _randomShootVector = new Vector2(randomVectorX,randomVectorY);
    }

    void OnDisable()
    {
        EventManager.OnGoal -= PlaySadAnim;
        EventManager.OnRestartLevel -= PlayIdleAnim;
        EventManager.OnNextLevel -= PlayIdleAnim;
    }
}
