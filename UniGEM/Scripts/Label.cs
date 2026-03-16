using UnityEngine;
using TMPro;

public class Label : MonoBehaviour
{
    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    } 

    public void SetText(string text) {
        if (this.text == null) {
            Awake(); // Force awake, sorry
        }
        this.text.text = text;
    }

    public void AddText(string text) {
        if (this.text == null) {
            Awake(); // Force awake, sorry
        }
        this.text.text += text;
    }

    public void ShiftText(string text) {
        if (this.text == null) {
            Awake(); // Force awake, sorry
        }
        this.text.text = text + this.text.text;
    }

    public void Clear() {
        if (this.text == null) {
            Awake(); // Force awake, sorry
        }
        text.text = "";
    }
}
