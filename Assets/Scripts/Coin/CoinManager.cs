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
    public Vector2 maxPowerVector;
    

    void OnEnable()
    {
        EventManager.OnUnselectedCoins += DrawLineBetweenUnselectedCoins;
        EventManager.onCoinSelect += ShowLineRenderer;
        EventManager.OnThrow += DisappearLineRenderer;
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
            selectedCoin.MoveTo(moveTargetPos.normalized);
    }

    public void CalculateTheThrowVector(Vector2 first,Vector2 last)
    {
        moveTargetPos = first - last;
    }
    void ShowLineRenderer()
    {
        lineRenderer.enabled =true;
    }
    void DisappearLineRenderer()
    {
        lineRenderer.enabled = false;
    }

   
    void OnDisable()
    {
        EventManager.OnUnselectedCoins -= DrawLineBetweenUnselectedCoins;
        EventManager.onCoinSelect -= ShowLineRenderer;
        EventManager.OnThrow -= ThrowTheSelectedCoin;
        EventManager.OnThrow -= DisappearLineRenderer;
    }
}

