using TMPro;
using UnityEngine;

public class TextArea : MonoBehaviour
{
    public static TextArea _instance;
    private TextMeshProUGUI text;
    public virtual void Awake() {
        if (isActiveAndEnabled) {
            if (_instance == null) {
                _instance = this;
            }
            else if (_instance != this) {
                Destroy(gameObject);
            }
        }
        text = GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateText(string textString) {
        text.text = textString;
    }
}
