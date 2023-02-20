using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    [SerializeField] GameObject _goalKeeper;

    [SerializeField] Transform _rightWayPoint;
    [SerializeField] Transform _leftWayPoint;
    Transform targetWayPoint;

    [SerializeField] float _moveSpeed;
    float elapsedTime = 0;

    void Start()
    {
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
            
            _goalKeeper.transform.position = Vector3.Lerp(_goalKeeper.transform.position,targetWayPoint.position,_moveSpeed);

            if(Vector3.Distance(_goalKeeper.transform.position,targetWayPoint.position) < 0.05f )
            {
                ChangeWayPoint();
                elapsedTime = 0;
            }
        }
    }
}
