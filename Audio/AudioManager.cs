using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    [Header("事件监听")]
    public PlayAudioEvent_SO FXEvent;
    public PlayAudioEvent_SO BGMEvent;
    public FloatEvent_SO VolumeEvent;
    public VoidEvent_SO pauseGameEvent;
    [Header("广播")]
    public FloatEvent_SO syncVolumeEvent;
    [Header("组件")]
    public AudioSource  BGMSource;
    public AudioSource FXSource;
    public AudioMixer mixer;


    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        VolumeEvent.OnEventRaised += OnVolumeEvent;
        pauseGameEvent.OnEventRaised +=OnPauseGameEvent;
    }
    private void OnDisable() 
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        VolumeEvent.OnEventRaised -= OnVolumeEvent;
        pauseGameEvent.OnEventRaised -=OnPauseGameEvent;
    }
    private void OnPauseGameEvent()
    {
        float amount;
        //传递
        mixer.GetFloat("MasterVolume",out amount);
        //UIManager中Slider监听
        syncVolumeEvent.OnEventRaised(amount);
    }
    private void OnVolumeEvent(float amount)
    {
        //  名称 数值
        //需要对数值进行处理以适应Volume de -80 ->20
        mixer.SetFloat("MasterVolume",(amount*100 -80));
    }
    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }
}
