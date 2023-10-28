using System;
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
    public Action OnClick;
    private TMP_Text _actualContainer;
    private Animator _textAnimator;


    public void SetText(string text)
    {
        _actualContainer.text = text;
    }

    private void PlaySpawnAnimation()
    {
        _textAnimator.SetTrigger("SpawnTrigger");
    }
    private void Awake() {
        _actualContainer = GetComponent<TMP_Text>();
        _textAnimator = GetComponent<Animator>();
    }

    private void Start() {
        OnClick += PlaySpawnAnimation;
    }
    
    private void OnDestroy() {
        OnClick -= PlaySpawnAnimation;
    }
}
