using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

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

    //[SerializeField] private Wave _finalWave;

    [SerializeField] private GameObject _secondPlayer;
    [SerializeField] private GameObject _secondPlayerUI;

    [SerializeField] private AudioMixerSnapshot _paused;
    [SerializeField] private AudioMixerSnapshot _unPaused;
    
    [SerializeField] private GameObject _deathPanel;
    
    [SerializeField] private TextMeshProUGUI _scoreValue;
    [SerializeField] private TextMeshProUGUI _scoreValueFinal;
    [SerializeField] private TextMeshProUGUI _congratulationsText;

    [HideInInspector]
    public float playerScore;
    [HideInInspector]
    //public bool _deathPause = false;

    private bool _playerAdded;
    private void Start()
    {
        _unPaused.TransitionTo(.1f);

        Time.timeScale = 1;
        _waves = GetComponents<Wave>().ToList();
        InvokeRepeating("CheckEnemiesOnMap", 5, 1);
    }

    private void Update()
    {
        if (playerScore < 0)
            playerScore = 0;
        
        _scoreValue.SetText(playerScore.ToString());
        // if (Input.GetKeyUp(KeyCode.Return) && !_playerAdded)
        // {
        //     _secondPlayer.SetActive(true);
        //     _secondPlayerUI.SetActive(true);
        //     _playerAdded = true;
        // }
    }

    public void DeathPause(bool _deathPause)
    {
        if (_deathPause)
        {
            _paused.TransitionTo(.2f);
            _deathPanel.SetActive(true);
            _scoreValue.gameObject.SetActive(false);
            _scoreValueFinal.SetText(playerScore.ToString());
            Time.timeScale = 0.5f;
        }
        else if (!_deathPause)
        {
            _unPaused.TransitionTo(.2f);
            _deathPanel.SetActive(false);
            _scoreValue.SetText(playerScore.ToString());
            Time.timeScale = 1;
        }
    }
    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
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

        if (_currentWaveIndex >= _waves.Count)
        {
            DeathPause(true);
            _congratulationsText.SetText("CONGRATULATIONS!");
            _scoreValue.SetText(playerScore.ToString());
        }
        else
        {
            _currentWaveIndex++;
            
            Wave wave = _waves[_currentWaveIndex - 1];

            SpawnEnemies(wave.Ants, _ant);
            SpawnEnemies(wave.Bees, _bee);
            SpawnEnemies(wave.Cockroaches, _cockroach);
            SpawnEnemies(wave.Ladybugs, _ladybug);
            SpawnEnemies(wave.Scorpios, _scorpio);
            SpawnEnemies(wave.Spiders, _spider);
        }
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
