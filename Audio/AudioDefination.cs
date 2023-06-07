using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//播放音效  AudioManager监听
public class AudioDefination : MonoBehaviour
{
    public PlayAudioEvent_SO playAudioEvent;
    public AudioClip audioClip;
    public bool playOnAwake;
    private void OnEnable()
    {
        //启用时播放
        if(playOnAwake)
            PlayAduioClip();
    }

    public void PlayAduioClip()
    {
        playAudioEvent.RaiseEvent(audioClip);
    }
}
