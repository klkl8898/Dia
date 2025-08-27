using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dia
{
    public class DialogueAudioManager : MonoBehaviour
    {
        private static DialogueAudioManager instance;
        public static DialogueAudioManager Instance
        {
            get
            {
                return instance;
            }
        }
        private AudioSource Audio;
        private AudioClip clip;
        private void Awake()
        {
            instance = this;//类型转换 把this转换为T类型的对象
            Audio = GetComponent<AudioSource>();
        }
        public void Typing(AudioClip clip, float time, float Time)
        {
            this.clip = clip;
            StartCoroutine(typing(time, Time));
        }
        IEnumerator typing(float time, float Time)
        {
            while (Time > 0)
            {
                Audio.PlayOneShot(clip);
                Time -= time;
                yield return new WaitForSeconds(time);
            }
        }
        void OnDestory()
        {
            instance = null;
        }
    }

}
