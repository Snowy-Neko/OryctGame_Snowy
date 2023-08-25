using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTyping : MonoBehaviour
{

    [SerializeField] private float TypingSpeed = 50f;

    public Coroutine Run(string textToType, TMP_Text m_Text)
    {
        return StartCoroutine(TypeText(textToType, m_Text));
    }

    private IEnumerator TypeText(string textToType, TMP_Text m_Text)
    {
        m_Text.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length) 
        {
            t += Time.deltaTime * TypingSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            m_Text.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        m_Text.text = textToType;
    }
}
