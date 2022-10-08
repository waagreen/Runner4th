using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class TransitionController : MonoBehaviour
{   
    [SerializeField] private GameObject deathScreen;

    public UnityEvent OnPlayerDeath = new UnityEvent();

    public Animator transition;
    public float transitionDuration;

    private void Awake() {
        OnPlayerDeath.AddListener(ShowDeathScreen);
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
        OnPlayerDeath.RemoveListener(ShowDeathScreen);
    }
}
