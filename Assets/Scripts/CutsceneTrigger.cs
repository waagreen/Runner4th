using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CutsceneTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == DataManager.playerTag)
        {
            int desiredCutscene = DataManager.Ui.GetCurrentSceneIndex() - 2;
            Debug.Log(desiredCutscene);
            DataManager.Ui.SetCurrentCutscene(desiredCutscene);
        }
    }
}
