using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private GameObject _meteor;
    [SerializeField] private int _currentWaveIndex = -1;
    [SerializeField] private List<Wave> _waves;
    
    [SerializeField] private GameObject _ant;
    [SerializeField] private GameObject _bee;
    [SerializeField] private GameObject _cockroach;
    [SerializeField] private GameObject _ladybug;
    [SerializeField] private GameObject _scorpio;
    [SerializeField] private GameObject _spider;
    
    private void Start()
    {
        _waves = GetComponents<Wave>().ToList();
        InvokeRepeating("CheckEnemiesOnMap", 5000, 1000);
    }

    private void CheckEnemiesOnMap()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        _currentWaveIndex++;
        
        Wave wave = _waves[_currentWaveIndex];

        for (int i = 0; i < wave.Ants; i++)
        {
            GameObject newMeteor = Instantiate(_meteor);
            newMeteor.GetComponent<Meteor>().SetEnemy(_ant);
        }        
    }
}
