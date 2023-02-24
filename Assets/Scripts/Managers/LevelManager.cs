using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;

    GameObject _currentLevel;
    int _levelIndex = 0;

    void OnEnable()
    {
        EventManager.OnNextLevel += NextLevel;
    }


    void Start()
    {
        _currentLevel = levels[_levelIndex];
        _currentLevel.SetActive(true);
    }

    void NextLevel()
    {
        _currentLevel.gameObject.SetActive(false);
        _levelIndex++;
         if(_levelIndex == levels.Length)
         {
            _levelIndex = 0;
         }
        _currentLevel = levels[_levelIndex];
        _currentLevel.SetActive(true);
    }

    void OnDisable()
    {
        EventManager.OnNextLevel -= NextLevel;
    }
}
