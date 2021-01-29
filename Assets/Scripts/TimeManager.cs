using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField]Transform candle;
    [SerializeField]Animator anim;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Light lightTorch;
   [SerializeField] float MaxTime , StartTimeBeforeConsumption;
    float lowestPoint = 0.16f;
    Vector3 startPos = new Vector3();
    private void Start()
    {  
        startPos = candle.position;
        StopCoroutine(Melting());
        StartCoroutine(Melting());

    }
    float endY;
    IEnumerator Melting()
    {
        yield return new WaitForSeconds(StartTimeBeforeConsumption);

      
         endY =candle.position.y - lowestPoint;

        LeanTween.moveY(candle.gameObject, endY, MaxTime);
        
        yield return new WaitForSeconds(MaxTime);
        Death();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        RestartSetting();
    }
    void Death() {
        anim.enabled = false;
        sr.enabled = false;
        lightTorch.enabled = false;
    }
    public void RestartSetting() {
        LeanTween.reset();
        LeanTween.moveY(candle.gameObject, candle.position.y + lowestPoint, 1f);
        anim.enabled = true;
        sr.enabled = true;
        lightTorch.enabled = true;
        StopAllCoroutines();
        StartCoroutine(Melting());
    }
 
}
