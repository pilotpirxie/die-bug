using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManger;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Play()
    {
    SceneManger.LoadScene(SceneManger.GetActiveScene().buildIndex + 1);

    }


}
