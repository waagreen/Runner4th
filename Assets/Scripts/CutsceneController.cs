using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private Image storyHolder;
    [SerializeField] private List<Sprite> stories;
    [SerializeField] private ButtonController nextLevelButton;
    private int currentCutscene = DataManager.Ui.CurrentCutscene;


    void Start()
    {
        storyHolder.sprite = stories[currentCutscene - 1];
        nextLevelButton.desiredScene = GetNextScene(currentCutscene);
    }

    private SceneOrder GetNextScene(int desiredLevel)
    {
        switch(desiredLevel)
        {
            case 1:
                return SceneOrder.SecondLevel;
            case 2:
                return SceneOrder.ThirdLevel;
            case 3: 
                return SceneOrder.MainMenu;
            default:
                return SceneOrder.FirstLevel;
        }
    }
}
