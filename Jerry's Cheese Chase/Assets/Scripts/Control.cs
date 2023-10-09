using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void home()
    {
        SceneManager.LoadScene("Level Selector");
    }

    public void first()
    {
        ResetGame("Level 1");
    }
    public void second()
    {
        ResetGame("Level 2");
    }
    public void ResetGame(string level)
    {
        SceneManager.LoadScene(level);
        Time.timeScale = 1;
        ScoreScript.cheese = 0;
        Lives.heart = 3;
    }
}
