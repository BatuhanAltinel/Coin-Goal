using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    LineRenderer lineRenderer;
    [SerializeField] List<Coin> coins;
    public Coin selectedCoin;

    void OnEnable()
    {
        EventManager.OnUnselectedCoins += DrawLineBetweenUnselectedCoins;
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
        foreach (var coin in coins)
        {
            if(selectedCoin != coin)
            {
                lineRenderer.SetPosition(indx,coin.transform.position);
                indx++;
            }   
        }
    }

    void OnDisable()
    {
        EventManager.OnUnselectedCoins -= DrawLineBetweenUnselectedCoins;
    }
}

