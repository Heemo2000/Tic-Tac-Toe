using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Random = UnityEngine.Random;

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

    private void PlayClickSound()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        int soundNo = Random.Range(1,3);
        if(soundNo == 1)
        {
            SoundManager.Instance.PlaySFX(SoundType.GridClick1);
        }
        else
        {
            SoundManager.Instance.PlaySFX(SoundType.GridClick2);
        }
        
    }
    private void Awake() {
        _actualContainer = GetComponent<TMP_Text>();
        _textAnimator = GetComponent<Animator>();
    }

    private void Start() {
        OnClick += PlaySpawnAnimation;
        OnClick += PlayClickSound;
    }
    
    private void OnDestroy() {
        OnClick -= PlaySpawnAnimation;
        OnClick -= PlayClickSound;
    }
}
