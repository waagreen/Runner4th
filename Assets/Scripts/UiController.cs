using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UiController : MonoBehaviour
{   
    [SerializeField] private GameObject deathScreen;
    
    private UnityEvent deathEvent => DataManager.Events.OnPlayerDeath;
    public Animator transition;
    public float transitionDuration;

    private void Awake() {
        deathEvent.AddListener(ShowDeathScreen);
    }

    IEnumerator loadCoroutine(SceneOrder desiredScene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(desiredScene.ToString());
    }

    public void LoadLevel(SceneOrder desiredScene) => StartCoroutine( loadCoroutine(desiredScene));
    

    private void ShowDeathScreen() => deathScreen.SetActive(true);

    private void OnDestroy() {
        deathEvent.RemoveListener(ShowDeathScreen);
    }
}
