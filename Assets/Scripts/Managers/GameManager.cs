using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    LineRenderer lineRenderer;

    void OnEnable()
    {
        EventManager.OnPrepareToThrow += DrawArrowOnSelectedCoin;
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void DrawArrowOnSelectedCoin()
    {
        lineRenderer.material = lineRenderer.materials[0];
        
        Coin coin = CoinManager.Instance.selectedCoin;
        Vector3 targetPos = new Vector3(CoinManager.Instance.moveTargetPos.x,coin.transform.position.y,CoinManager.Instance.moveTargetPos.y);

        lineRenderer.SetPosition(0,CoinManager.Instance.selectedCoin.transform.position);
        lineRenderer.SetPosition(1,targetPos);
    }


    void OnDisable()
    {
        EventManager.OnPrepareToThrow -= DrawArrowOnSelectedCoin;
    }
}
