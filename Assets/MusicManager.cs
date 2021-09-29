using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public static AudioClip lobbyMusic;
    public static AudioClip gameMusic;
    private static AudioSource audioSource;


    public AudioClip _lobbyMusic;
    public AudioClip _gameMusic;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        lobbyMusic = _lobbyMusic;
        gameMusic = _gameMusic;

        audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(this.gameObject);
    }

    public static void StartMusic(bool inLobby)
    {
        audioSource.clip = inLobby ? lobbyMusic : gameMusic;
        audioSource.Play();
    }
}
