﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardManager : MonoBehaviour
{
    public static BoardManager _instance;
    AudioSource snapSound;
    [HideInInspector]
    public List<Spawner> spawners = new List<Spawner>();
    public virtual void Awake() {
        if (isActiveAndEnabled) {
            if (_instance == null) {
                _instance = this;
            }
            else if (_instance != this) {
                Destroy(gameObject);
            }
        }
        snapSound = GetComponent<AudioSource>();
    }

    public bool CheckDiscard( Vector2 pos) {
        bool foundDiscard = false;
        foreach (Spawner spawner in spawners) {
            foundDiscard |= spawner.CheckDiscard(pos);
        }
        return foundDiscard;
    }
    public void PlaySnapSound() 
        => snapSound.Play();
    
}

