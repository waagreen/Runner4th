using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button bt;
    
    public SceneOrder desiredScene;
    private TransitionController loader => DataManager.Loader;

    private void Awake() {
        bt.onClick.AddListener(LoadScene);
    }

    public void LoadScene() => loader.LoadLevel(desiredScene);

    private void OnDestroy() {
        bt.onClick.RemoveAllListeners();
    }
}
