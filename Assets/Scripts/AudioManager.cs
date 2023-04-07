using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] sfx;
    public AudioSource[] bgm;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX(int soundToPlay)
    {
        sfx[soundToPlay].Play();
    }

    public void PlayBGM(int musicToPlay)
    {
        if (bgm[musicToPlay].isPlaying)
            return;

        StopMusic();
        bgm[musicToPlay].Play();
    }

    public void StopMusic()
    {
        foreach (var music in bgm)
        {
            music.Stop();
        }
    }
}
