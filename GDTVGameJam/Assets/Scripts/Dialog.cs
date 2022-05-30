using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [SerializeField] float typingSpeed = 0.2f;
    public TextMeshProUGUI textDisplay;
    public string sentence;
    private int index;
    void OnEnable()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
        sentence = textDisplay.text;
        textDisplay.text = "";
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        yield return new WaitForSeconds(1f);
        foreach(char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
