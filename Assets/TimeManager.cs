using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField]Transform candle;
    [SerializeField]Animator anim;
    Vector3 finalPosition;
   [SerializeField] float currentTime, MaxTime;
    private void Start()
    {
        finalPosition = candle.position;
    }
    private void Update()
    {
        
    }
}
