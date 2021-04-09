using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;

    private AudioClip[] audioArray;
    private Dictionary<string, AudioClip> audioDic;

	void Awake () {
        Instance = this;
        FindAndInit();
	}
	
	private void FindAndInit()
    {
        audioArray = Resources.LoadAll<AudioClip>("Audio/All");
        audioDic = new Dictionary<string, AudioClip>();
        for (int i = 0; i < audioArray.Length; i++)
        {
            audioDic.Add(audioArray[i].name, audioArray[i]);
        }
    }

    public AudioClip GetAudioClipByEnum(ClipName name)
    {
        AudioClip temp;
        audioDic.TryGetValue(name.ToString(), out temp);
        return temp;
    }

    public void PlayAudioClipByEnum(ClipName name, Vector3 pos, float volume = 1)
    {
        AudioSource.PlayClipAtPoint(GetAudioClipByEnum(name), pos, volume);
    }

    public AudioSource AddAudioSourceComponent(ClipName name, GameObject go, bool playOnAwake = true, bool loop = true, float volume = 1)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClipByEnum(name);
        audioSource.loop = loop;
        audioSource.playOnAwake = playOnAwake;
        audioSource.volume = volume;
        if (playOnAwake)
            audioSource.Play();
        return audioSource;
    }
}
