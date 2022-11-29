using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private Image storyHolder;
    [SerializeField] private VideoPlayer player;
    [SerializeField] private List<Sprite> stories;
    [SerializeField] private List<VideoClip> clips;
    [SerializeField] private ButtonController nextLevelButton;
    [SerializeField] private RawImage screen;

    void Start()
    {
        int currentCutscene = DataManager.Ui.CurrentCutscene;
        
        if (currentCutscene > 0)
        {
            storyHolder.sprite = stories[currentCutscene - 1];
            player.clip = clips[currentCutscene - 1];
            player.Play();
        }
        nextLevelButton.desiredScene = GetNextScene(currentCutscene);

        player.loopPointReached += EndVideo;
    }

    private SceneOrder GetNextScene(int desiredLevel)
    {
        switch(desiredLevel)
        {
            case 1:
                return SceneOrder.SecondLevel;
            case 2:
                return SceneOrder.MainMenu;
            case 3: 
                return SceneOrder.MainMenu;
            default:
                return SceneOrder.FirstLevel;
        }
    }

    private void EndVideo(UnityEngine.Video.VideoPlayer vp)
    {
        player.Stop();
        player.gameObject.SetActive(false);
        screen.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        
        player.loopPointReached -= EndVideo;
    }
}
