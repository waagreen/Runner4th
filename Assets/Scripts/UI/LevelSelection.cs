using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] ButtonController playButton;

    [Header("Level Thumbs")]
    [SerializeField] Image spriteHolder;
    [SerializeField] List<Sprite> levelSprites;
    
    private int currentIndex = 0;

    void Start()
    {
        spriteHolder.sprite = levelSprites[currentIndex];
        playButton.desiredScene = SceneOrder.FirstLevel;
        
        leftButton.onClick.AddListener(() => ScrollImages(-1));
        rightButton.onClick.AddListener(() => ScrollImages(1));
    }

    private void ScrollImages(int direction)
    {
        int newIndex = currentIndex + direction;
      
        if (newIndex > 2) newIndex = 0;
        else if (newIndex < 0) newIndex = 2; 
        
        currentIndex = newIndex;
        spriteHolder.sprite = levelSprites[currentIndex];
        playButton.desiredScene = GetNextScene(newIndex);
    }
    
    private SceneOrder GetNextScene(int desiredLevel)
    {
        switch(desiredLevel)
        {
            case 0:
                return SceneOrder.FirstLevel;
            case 1:
                return SceneOrder.SecondLevel;
            case 2: 
                return SceneOrder.ThirdLevel;
            default:
                return SceneOrder.FirstLevel;
        }
    }

    private void OnDestroy()
    { 
        leftButton.onClick.RemoveListener(() => ScrollImages(-1));
        rightButton.onClick.RemoveListener(() => ScrollImages(1));
    }
}
