using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneOrder
{
    MainMenu,
    FirstLevel,
    SecondLevel,
    ThirdLevel,
}
public class ButtonController : MonoBehaviour
{
    public SceneOrder desiredScene;

    public void LoadLevel() => SceneManager.LoadScene(desiredScene.ToString());
    public void ReloadGame()
    {   
        var currentScene = SceneManager.GetActiveScene();   
        SceneManager.LoadScene(currentScene.buildIndex);
	}
}
