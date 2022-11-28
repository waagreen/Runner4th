using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CutsceneTrigger : MonoBehaviour
{
    private UiController uiController => DataManager.Ui;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == DataManager.playerTag)
        {
            int desiredCutscene = uiController.GetCurrentSceneIndex() - 2;
            uiController.SetCurrentCutscene(desiredCutscene);
        }
    }
}
