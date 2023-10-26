using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TextContainerRow
{
    public List<TextContainer> elements;
}
[System.Serializable]
public class TextContainer : MonoBehaviour
{
    private TMP_Text _actualContainer;

    public void SetText(string text)
    {
        _actualContainer.text = text;
    }
    private void Awake() {
        _actualContainer = GetComponent<TMP_Text>();
    }

    
}
