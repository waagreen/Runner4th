using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public enum SceneOrder
{
    FirstLevel,
    SecondLevel,
    ThirdLevel,
    MainMenu,
    Lab,
    Cutscene
}

public class UiController : MonoBehaviour
{   
    [Header("UI Screens")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject pauseScreen;

    [Header("Trasition Stuff")]
    public Animator transition;
    public float transitionDuration;
    
    public int GetCurrentSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    private UnityEvent deathEvent;
    private UnityEvent<bool> pauseEvent;

    public int CurrentCutscene => currentCutscene;
    private static int currentCutscene;

    private void Start() 
    {   
        deathEvent = DataManager.Events.OnPlayerDeath;
        pauseEvent = DataManager.Events.OnPauseGame;

        deathEvent.AddListener(ShowDeathScreen);
        pauseEvent.AddListener(ShowPauseScreen);
    }

    public void SetCurrentCutscene(int sceneIndex)
    {
        currentCutscene = sceneIndex;
        StartCoroutine(loadCoroutine(SceneOrder.Cutscene));
    }

    public void LoadLevel(SceneOrder desiredScene) => StartCoroutine(loadCoroutine(desiredScene));
    private void ShowDeathScreen() => deathScreen.SetActive(true);
    private void ShowPauseScreen(bool isPaused) => pauseScreen.SetActive(isPaused); 

    
    IEnumerator loadCoroutine(SceneOrder desiredScene)
    {
        Time.timeScale = 1f;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(desiredScene.ToString());
    }
}
