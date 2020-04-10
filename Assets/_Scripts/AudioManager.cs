using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip musicBg;
    
    [SerializeField] private AudioClip blockDrop;
    [SerializeField] private AudioClip lineWhipe;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = musicBg;
        _audioSource.volume = 0.75f;
        _audioSource.loop = true;
        _audioSource.Play();
        
        EventBus.OnBlockDrop += OnBlockDrop;
        EventBus.OnAddPoints += OnAddPoints;
    }

    private void OnDestroy()
    {
        EventBus.OnBlockDrop -= OnBlockDrop;
        EventBus.OnAddPoints -= OnAddPoints;
    }

    private void OnBlockDrop(object sender, EventArgs e)
    {
        _audioSource.PlayOneShot(blockDrop, 1f);
    }

    private void OnAddPoints(object sender, EventArgs e)
    {
        _audioSource.PlayOneShot(lineWhipe, 1f);
    }
}
