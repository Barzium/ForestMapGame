using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    [SerializeField]  int buttonID;
    MeshRenderer mr;
   
    Color defaultColor = new Color();
    bool isActivated = false;
    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        defaultColor = mr.material.color;
    }
    public  void PushTheButton() {

        if (isActivated)
            return;
        isActivated = true;
        CodeHandler.GetInstance.InsertNumber(buttonID);
        StopAllCoroutines();
        StartCoroutine(ButtonCooldown());

    }
    IEnumerator ButtonCooldown() {
        mr.material.color = Color.green;
        yield return new WaitForSeconds(0.3f);
        mr.material.color = defaultColor;
        isActivated = false;
    }

    
    private void OnMouseOver()
    {
        if (isActivated) 
            mr.material.color = Color.green;
        
        else
       mr.material.color = Color.white;
        
       
        
    }
    private void OnMouseExit()
    {
        mr.material.color = defaultColor;
    }
    public void ActivateButtonColor(bool isGreen) {
        StopAllCoroutines();
        StartCoroutine(ButtonCooldownColor(isGreen));
    
    }
    IEnumerator ButtonCooldownColor(bool isGreen)
    {
        isActivated = true;
        if (isGreen)
            mr.material.color = Color.green;
        else
            mr.material.color = Color.red;


        yield return new WaitForSeconds(0.3f);
        mr.material.color = defaultColor;
        isActivated = false;
    }
}
