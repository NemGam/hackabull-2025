using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct AudioCategory
{
    public string name;                 
    public List<AudioClip> audioClips;  
}


public enum gameStage
{
    Ambient = 0,
    Peaceful = 1,
    Chaotic = 2,
}
public class AudioSourceHandler : MonoBehaviour
{
    [SerializeField] List<AudioCategory> categorizedAudio; 
    
    [SerializeField] AudioSource ambientSource;
    [SerializeField] AudioSource peacefulSource;
    [SerializeField] AudioSource chaoticSource;

    [SerializeField] bool playPeacefulAudio;
    [SerializeField] bool playChaoticAudio;
    
    private int ambientIndex = 0;
    private int peacefulIndex = -1;
    private int chaoticIndex = -1;

    private bool staySilent = false;
    
    
    void Update()
    {
        if (!ambientSource.isPlaying && !staySilent)
        {
            AmbientPlayer();
        }

        if (!peacefulSource.isPlaying && playPeacefulAudio && !staySilent)
        {
            IncrementSelectIndex(gameStage.Peaceful);
            PeacefulPlayer();
        }
        
        if (!chaoticSource.isPlaying && playChaoticAudio && !staySilent)
        {
            IncrementSelectIndex(gameStage.Chaotic);
            ChaoticPlayer();
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchPeacefulToChaotic();
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchChaoticToPeaceful();
        }
    }

    public void SwitchPeacefulToChaotic()
    {
        playPeacefulAudio = false;
        StartCoroutine(FadeOutAudio(chaoticSource, 2f));
        StartCoroutine(FadeOutAudio(peacefulSource, 2f));
        StartCoroutine(FadeOutAudio(ambientSource, 2f));
        
        staySilent = true;
        ambientIndex = 1;
        
        StartCoroutine(ChaoticPlayerPlayInMoment(0.3f, 2.6f));
        playChaoticAudio = true;
    }
    
    public void SwitchChaoticToPeaceful()
    {
        playChaoticAudio = false;
        StartCoroutine(FadeOutAudio(chaoticSource, 2f));
        StartCoroutine(FadeOutAudio(peacefulSource, 2f));
        StartCoroutine(FadeOutAudio(ambientSource, 2f));
        
        staySilent = true;
        ambientIndex = 0;
        AmbientPlayer(1f);
        
        StartCoroutine(PeacefulPlayerPlayInMoment(0.7f, 2.6f));
        playPeacefulAudio = true;
    }
    
    public void AmbientPlayer(float volume = 0.7f)
    {
        ambientSource.PlayOneShot(categorizedAudio[(int)gameStage.Ambient].audioClips[ambientIndex], volume);
    }
    
    public void PeacefulPlayer(float volume = 0.7f)
    {
        peacefulSource.PlayOneShot(categorizedAudio[(int)gameStage.Peaceful].audioClips[peacefulIndex], volume);
    }
    
    public void ChaoticPlayer(float volume = 0.7f)
    {
        chaoticSource.PlayOneShot(categorizedAudio[(int)gameStage.Chaotic].audioClips[chaoticIndex], volume);
    }

    public void IncrementSelectIndex(gameStage stage)
    {
        string s = stage.ToString();
        switch (s)
        {
            case "Ambient":
                ambientIndex = (ambientIndex + 1) % categorizedAudio[(int)gameStage.Ambient].audioClips.Count;
                break;
            case "Peaceful":
                peacefulIndex = (peacefulIndex + 1) % categorizedAudio[(int)gameStage.Peaceful].audioClips.Count;
                break;
            case "Chaotic":
                chaoticIndex = (chaoticIndex + 1) % categorizedAudio[(int)gameStage.Chaotic].audioClips.Count;
                break;
        }
    }
    
    private IEnumerator ChaoticPlayerPlayInMoment(float volume, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (chaoticIndex < 0) IncrementSelectIndex(gameStage.Chaotic);
        AmbientPlayer(0.85f);
        ChaoticPlayer(volume);
        staySilent = false;
    }
    
    private IEnumerator PeacefulPlayerPlayInMoment(float volume, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (chaoticIndex < 0) IncrementSelectIndex(gameStage.Peaceful);
        PeacefulPlayer(volume);
        staySilent = false;
    }
    
    public IEnumerator FadeOutAudio(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        while (source.volume > 0f)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset for next play if needed
    }
}
