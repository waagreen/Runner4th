using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{   
    public Animator transition;
    public float transitionDuration;

    IEnumerator loadCoroutine(SceneOrder desiredScene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(desiredScene.ToString());
    }

    public void LoadLevel(SceneOrder desiredScene) => StartCoroutine( loadCoroutine(desiredScene));
}
