using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapAudio : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private int audioIndex=0;
    [SerializeField] private float myVolume = 0.5f;
    [SerializeField] private bool StopAfterLastAudio= false;
    
    AudioSource audioSource;
    private bool doNotPlay = false;
    
    public bool autoPlay = false;
    public bool loopCurrentClip=false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioIndex += -1;
        if (!(audioSource == null || audioClips.Count == 0))
            audioSource.clip = audioClips[0];
    }
    
    void Update()
    {
        if (!audioSource.isPlaying && autoPlay && !loopCurrentClip && !doNotPlay)
        {
            PlayNextAudio();
        }

        if (!audioSource.isPlaying && loopCurrentClip && !doNotPlay)
        {
            audioSource.PlayOneShot(audioClips[audioIndex], myVolume);
        }
    }

    public void InterputAudio(AudioClip newAudio = null, float volume = 0.5f, float duration = 0f)
    {
        myVolume = volume;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (newAudio != null)
        {
            StartCoroutine(PlayClipCoroutine(newAudio, volume, duration));
        }
    }
    // Update is called once per frame
    public void PlayNextAudio(float volume=0.5f)
    {
        if (audioClips == null || audioClips.Count == 0) return;

        myVolume = volume;
        // If looping do not increment the Index otherwise it will loop the next clip not current
        if (!loopCurrentClip) audioIndex = (audioIndex+1) % audioClips.Count;
        audioSource.PlayOneShot(audioClips[audioIndex],volume);
        
        if (audioIndex == 0 && StopAfterLastAudio) doNotPlay = true;
    }
 
    public void SetdoNotPlay(bool newVal)
    {
        doNotPlay = newVal;
    }

    public void PlayAudioIndexed(int index, float volume=0.5f)
    {
        if (audioClips == null || audioClips.Count == 0) return;
        if (index < 0 || index >= audioClips.Count) return; 
        
        Debug.Log("Playing audio at index" + index);
        myVolume = volume;
        audioSource.PlayOneShot(audioClips[index], volume);
        
    }

    private IEnumerator PlayClipCoroutine(AudioClip clip, float volume, float duration)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;

        yield return new WaitForSeconds(duration);
        audioSource.Play();
    }
}
