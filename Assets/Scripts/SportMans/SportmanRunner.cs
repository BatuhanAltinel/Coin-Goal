using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportmanRunner : SportmenBase
{
    void Start()
    {
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        OnTouchCoin(other);
    }
}
