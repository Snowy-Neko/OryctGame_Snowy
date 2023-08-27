using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTyping : MonoBehaviour
{

    [SerializeField] private float TypingSpeed = 50f;

    public Coroutine Run(string textToType, TMP_Text m_text)
    {
        return StartCoroutine(TypeText(textToType, m_text));
    }

    private IEnumerator TypeText(string textToType, TMP_Text m_text)
    {
        Debug.Log(m_text.text);
        m_text.text = string.Empty;
        Debug.Log(m_text);

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length) 
        {
            t += Time.deltaTime * TypingSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            m_text.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        m_text.text = textToType;
    }
}
