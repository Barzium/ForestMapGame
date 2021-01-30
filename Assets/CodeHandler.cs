using UnityEngine;

public class CodeHandler : MonoBehaviour
{
    static CodeHandler _instance;
    [SerializeField] InputButton[] btns;
   [SerializeField] int[] playersInput = new int[4];
   [SerializeField] int[] password = new int[4];
    int currentInsert = 0;
    private void Awake()
    {
        _instance = this;
    }
    public static CodeHandler GetInstance => _instance;

    public void CheckPassword()
    {
        bool checkAnswer = true;
        for (int i = 0; i < playersInput.Length; i++)
        {
            checkAnswer &= playersInput[i] == password[i];
            if (!checkAnswer)
            {
                WrongPassword();
                return;
            }
        }
        CorrectPassword();
    }


    void ResetPassword() {

        for (int i = 0; i < playersInput.Length; i++)
            playersInput[i] = 0;

        currentInsert = 0;
    }

    void WrongPassword() {

        Debug.Log("Wrong Answer");

        ShowAnswer(false);
        ResetPassword(); 
    }
    void CorrectPassword() {
        Debug.Log("Correct Answer");
        ShowAnswer(true);
    }

    public void RegisterPassword(int[] password) {

        if (password.Length == this.password.Length)
         this.password = password;
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


        playersInput[currentInsert] = input;
        currentInsert++;

        if (currentInsert >= password.Length - 1)
            currentInsert = password.Length - 1;

        PrintDebug();
    }
    
    public void PrintDebug() {

        Debug.Log(string.Format("The Password is {0}, {1} , {2}", password[0], password[1], password[2]));
        Debug.Log(string.Format("The player input is {0}, {1} , {2} ", playersInput[0], playersInput[1], playersInput[2]));
        Debug.Log(currentInsert);
    }



    void ShowAnswer(bool isCorrect)
    {
        foreach (var item in btns)
        {
            item.ActivateButtonColor(isCorrect);
        }


    }
}
