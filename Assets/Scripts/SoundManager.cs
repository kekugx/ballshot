using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip shotSound;
    public AudioClip glassSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        FindObjectOfType<GameManager>().SectionClearedEvent += OnSectionClearedEvent;
    }

    private void Start()
    {
        var rayController = FindObjectOfType<RayController>();
        rayController.ShootBallEvent += RayControllerOnShootBallEvent;
    }

    private void RayControllerOnShootBallEvent(Vector3[] obj)
    {
        _audioSource.PlayOneShot(shotSound);
    }

    private void OnSectionClearedEvent()
    {
        _audioSource.PlayOneShot(glassSound);
    }
}
