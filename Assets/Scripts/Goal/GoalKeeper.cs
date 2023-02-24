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

    [SerializeField] float _moveSpeed;
    float elapsedTime = 0;


    void OnEnable()
    {
        EventManager.OnGoal += PlaySadAnim;    
        EventManager.OnRestartLevel += PlaySideWalkAnim;
        EventManager.OnNextLevel += PlaySideWalkAnim;
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("IsGoal",false);
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
            float randomVectorY = Random.Range(1,0);
            float randomVectorX = Random.Range(-1,1);

            float randomPower = Random.Range(600,750);
            coin.MoveTo(new Vector2(randomVectorX,randomVectorY),randomPower);
            EventManager.OnKickBack.Invoke();
        }
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
    }
}
