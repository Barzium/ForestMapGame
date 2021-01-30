using UnityEngine.UI;
using System.Collections;

using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager _instance;
    [SerializeField] Transform candle;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Light lightTorch;
    [SerializeField] float MaxTime, StartTimeBeforeConsumption;
    [SerializeField] Image deathPanel;
    float lowestPoint = 0.16f;
    Vector3 startPos = new Vector3();
    public static bool isDead = false;
    private void Awake()
    {
        _instance = this;
    }

    float endY;
    IEnumerator Melting()
    {
        yield return new WaitForSeconds(StartTimeBeforeConsumption);

        endY = candle.position.y - lowestPoint;

        LeanTween.moveY(candle.gameObject, endY, MaxTime);

        yield return new WaitForSeconds(MaxTime);
        Death();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    RestartSetting();

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Death();
        //}
    }
    void Death()
    {
        anim.enabled = false;
        sr.enabled = false;
        lightTorch.enabled = false;
        RedEyesManager._instance.DeathLocation();
        //StopCoroutine(DeathPanel(true));
        //StartCoroutine(DeathPanel(true));
        isDead = true;
    }
    bool firstTIme = true;
    public void RestartSetting()
    {
        _instance = this;
        startPos = candle.position;
        if (!firstTIme)
        {
          
        LeanTween.moveY(candle.gameObject, candle.position.y + lowestPoint, 1f);
        } 
        firstTIme = false;
         LeanTween.reset();
        anim.enabled = true;
        sr.enabled = true;
        lightTorch.enabled = true;
        StopAllCoroutines(); 
       
       // StartCoroutine(DeathPanel(false));
        StartCoroutine(Melting());
    }


    IEnumerator DeathPanel(bool toAdd)
    {
        yield return new WaitForSeconds(2f);
        Color color = deathPanel.color;
        float addAlpha = deathPanel.color.a;
        while (true)
        {
            if (toAdd)
            {
                addAlpha += 0.02f;
            }
            else
            {
                addAlpha -= 0.02f;
            }
            addAlpha = Mathf.Clamp(addAlpha, 0, 1f);

            yield return null;
            deathPanel.color = new Color(color.r, color.g, color.b, addAlpha);
        }
    }
}
