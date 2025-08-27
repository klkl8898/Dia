using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dia
{
public class DialogueAudioManager : InstanceMono<DialogueAudioManager>
{
    private AudioSource Audio;
    private AudioClip clip;
    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }
    public void Typing(AudioClip clip, float time,float Time)
    {
        this.clip = clip;
        StartCoroutine(typing(time,Time));
    }
    IEnumerator typing(float time,float Time)
    {
        while (Time > 0)
        {
            Audio.PlayOneShot(clip);
                Time -= time;
            yield return new WaitForSeconds(time);
        }
    }
}
    
}
