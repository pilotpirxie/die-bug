using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [SerializeField] private GameObject _secondPlayer;
    [SerializeField] private GameObject _secondPlayerUI;

    private bool _playerAdded;
    private void Start()
    {
        _waves = GetComponents<Wave>().ToList();
        InvokeRepeating("CheckEnemiesOnMap", 5, 1);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) && !_playerAdded)
        {
            _secondPlayer.SetActive(true);
            _secondPlayerUI.SetActive(true);
            _playerAdded = true;
        }
    }

    private void CheckEnemiesOnMap()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] meteors = GameObject.FindGameObjectsWithTag("Meteor");

        if (enemies.Length + meteors.Length == 0)
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        _currentWaveIndex++;

        if (_currentWaveIndex >= _waves.Count) return; 
        
        Wave wave = _waves[_currentWaveIndex - 1];
        
        SpawnEnemies(wave.Ants, _ant);
        SpawnEnemies(wave.Bees, _bee);
        SpawnEnemies(wave.Cockroaches, _cockroach);
        SpawnEnemies(wave.Ladybugs, _ladybug);
        SpawnEnemies(wave.Scorpios, _scorpio);
        SpawnEnemies(wave.Spiders, _spider);
    }

    private void SpawnEnemies(int numberOfEnemies, GameObject enemyObj)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(318, 422), Random.Range(270, 370), Random.Range(255, 380));
            GameObject newMeteor = Instantiate(_meteor, spawnPos, Quaternion.identity);
            newMeteor.GetComponent<Meteor>().SetEnemy(enemyObj);
        }  
    }
}
