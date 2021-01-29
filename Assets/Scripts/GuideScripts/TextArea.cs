using TMPro;
using UnityEngine;

public class TextArea : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite;
    SpriteRenderer spriteRenderer;
    public static TextArea _instance;
    bool textWasChanged = false;

    public virtual void Awake() {
        if (isActiveAndEnabled) {
            if (_instance == null) {
                _instance = this;
            }
            else if (_instance != this) {
                Destroy(gameObject);
            }
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite(defaultSprite);
    }
    private void LateUpdate() {
        if(!textWasChanged && Input.GetKeyDown(KeyCode.Mouse0)) {
            UpdateSprite(defaultSprite);
        }
    }

    public void UpdateSprite(Sprite sprite) {
        spriteRenderer.sprite = sprite;
        textWasChanged = true;
    }
}
