using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _storyUI;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _menuSelectSfx;


    public void ShowStory()
    {
        _menuUI.SetActive(false);
        _storyUI.SetActive(true);
        _audioSource.PlayOneShot(_menuSelectSfx);
    }

    public void Back()
    {
        _menuUI.SetActive(true);
        _storyUI.SetActive(false);
        _audioSource.PlayOneShot(_menuSelectSfx);
    }
    
    public void PlayGame()
    {        
        // _audioSource.PlayOneShot(_menuSelectSfx);
        SceneManager.LoadScene("Scenes/New Level1");
    }
}
