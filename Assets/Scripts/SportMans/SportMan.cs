using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportMan : SportmenBase
{
    void Start()
    {
        OnStart();
    }

    void Update()
    {
        TurnToTheCoin();    
    }

    void OnTriggerEnter(Collider other)
    {
        OnTouchCoin(other);
    }
}
