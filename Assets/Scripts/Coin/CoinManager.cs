using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    LineRenderer lineRenderer;
    [SerializeField] List<Coin> coins;
    public Coin selectedCoin;
    public Vector2 moveTargetPos;

    void OnEnable()
    {
        EventManager.OnUnselectedCoins += DrawLineBetweenUnselectedCoins;
        EventManager.OnThrow += ThrowTheSelectedCoin;
    }
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = coins.Count-1;
    }

    public void SetTheCoinSelected(Coin coin)
    {
        selectedCoin = coin;
    }

    void DrawLineBetweenUnselectedCoins()
    {
        int indx = 0;
        lineRenderer.material = lineRenderer.materials[0];
        foreach (var coin in coins)
        {
            if(selectedCoin != coin)
            {
                lineRenderer.SetPosition(indx,coin.transform.position);
                indx++;
            }   
        }
    }

    void ThrowTheSelectedCoin()
    {
        if(selectedCoin != null)
            selectedCoin.MoveTo(moveTargetPos);
    }

    
    void OnDisable()
    {
        EventManager.OnUnselectedCoins -= DrawLineBetweenUnselectedCoins;
        EventManager.OnThrow += ThrowTheSelectedCoin;
    }
}

