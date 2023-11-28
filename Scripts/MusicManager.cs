using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource victoryMusic;
    [SerializeField] private AudioSource defeatMusic;
    
    private static MusicManager Instance;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(Instance.gameObject);
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDefeat += GameManagerOnDefeat;
            GameManager.Instance.OnVictory += GameManagerOnVictory;
        }
    }

    private void GameManagerOnDefeat(object sender, EventArgs e)
    {
        backgroundMusic.Stop();
        defeatMusic.Play();
    }
    
    private void GameManagerOnVictory(object sender, EventArgs e)
    {
        backgroundMusic.Stop();
        victoryMusic.Play();
    }
}
