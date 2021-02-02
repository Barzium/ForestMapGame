using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHandler : MonoBehaviour
{
    static CodeHandler _instance;
    [SerializeField] InputButton[] btns;
    List<int> playersInput = new List<int>();
    int[] password;
    private void Awake()
    {
        _instance = this;
    }
    public static CodeHandler GetInstance => _instance;

    public void CheckPassword()
    {
        if (playersInput.Count != password.Length)
        {
            WrongPassword();
            return;
        }
        for (int i = 0; i < playersInput.Count; i++)
        {
            if (playersInput[i] != password[i])
            {
                WrongPassword();
                return;
            }
        }
        CorrectPassword();
    }


    void ResetPassword() 
    {
        playersInput.Clear();
    }

    void WrongPassword() {

        Debug.Log("Wrong Answer");

        ShowAnswer(false);
        ResetPassword(); 
    }
    void CorrectPassword() {
        Debug.Log("Correct Answer");
        ShowAnswer(true);
        ResetPassword();
        //WinGame();
    }

   public  void RegisterPassword(int[] psw) {

        this.password = new int[3];
        for (int i = 0; i < password.Length; i++)
        {
            password[i] = psw[i];
        }
   
    }

    public void InsertNumber(int input)
    {
        if (input == 11)
        {
            CheckPassword();
            return;
        }
        else if (input == 12)
        { 
            ResetPassword();
            return;
        }

        playersInput.Add(input);

        PrintDebug();
    }
    
    public void PrintDebug() 
    {
        /*Debug.Log(string.Format("The Password is {0}, {1} , {2}", password[0], password[1], password[2]));
        Debug.Log(string.Format("The player input is {0}", playersInput.ToString()));
        Debug.Log(playersInput.Count);*/
    }



    void ShowAnswer(bool isCorrect)
    {
        foreach (var item in btns)
        {
            item.ActivateButtonColor(isCorrect);
        }


    }
}
