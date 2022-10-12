using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(AudioSource))]
public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button bt;
    
    public SceneOrder desiredScene;

    private void Start() {
        bt.onClick.AddListener(LoadScene);
    }

    public void LoadScene() => DataManager.Ui.LoadLevel(desiredScene);

    private void OnDestroy() {
        bt.onClick.RemoveAllListeners();
    }
}
