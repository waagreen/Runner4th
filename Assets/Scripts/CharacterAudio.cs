using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    collision,
    running,
    jump,
    slide
}

[RequireComponent(typeof(AudioSource))]
public class CharacterAudio : MonoBehaviour
{
    public List<AudioClip> collisionSounds = new List<AudioClip>();
    public List<AudioClip> runningSounds = new List<AudioClip>();
    public List<AudioClip> jumpSounds = new List<AudioClip>();
    public List<AudioClip> slideSounds = new List<AudioClip>();

    private AudioSource audioPlayer;

    private void Start() {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType sType)
    {
        if(audioPlayer.isPlaying) audioPlayer.Stop();

        List<AudioClip> desiredSoundList = GetSoundList(sType);

        int randomIndex = Random.Range(0, desiredSoundList.Count);
        
        audioPlayer.clip = desiredSoundList[randomIndex];
        audioPlayer.Play();
    }

    private List<AudioClip> GetSoundList(SoundType sType)
    {
        switch(sType)
        {
            case SoundType.collision : return collisionSounds;
            case SoundType.running : return runningSounds;
            case SoundType.jump : return jumpSounds;
            case SoundType.slide : return slideSounds;
            default : return null;
        }
    }
}
