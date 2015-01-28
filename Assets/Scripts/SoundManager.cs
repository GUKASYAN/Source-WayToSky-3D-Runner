/// <summary>
/// Sound manager.
/// This script use for manager all sound(bgm,sfx) in game
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    public AudioSource ASource;
    public List<AudioClip> SoundsList; 

    private void Start()
    {
        soundManager = this; 
    }    
    public void MusicMute()
    {   
        ASource.mute = true;
    }    
    public void PlayDeadSound()
    {
        this.audio.Stop();         
        AudioSource.PlayClipAtPoint(SoundsList[0], Camera.main.transform.position);
    }
}
