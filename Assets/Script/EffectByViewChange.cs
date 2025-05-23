using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectByViewChange : MonoBehaviour
{
    protected bool IsSS = true;

    [Serializable]
    protected struct ViewChangeSprites
    {
        public SpriteRenderer SpriteRenderer;
        public Sprite SpriteSS;
        public Sprite SpriteTD;
    }

    [SerializeField] List<GameObject> _showInSS;
    [SerializeField] List<GameObject> _showInTD;
    [SerializeField] List<ViewChangeSprites> _viewChangeSprites;

    protected virtual void Start()
    {
        ViewInit();
    }

    void ViewInit()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnViewChanged.AddListener(ViewChanged);
            ViewChanged(GameManager.Instance.GetView());
        }
    }

    protected virtual void ViewChanged(bool isSS)
    {
        IsSS = isSS;
        foreach (var item in _showInSS) item.SetActive(isSS);
        foreach (var item in _showInTD) item.SetActive(!isSS);
        foreach (var item in _viewChangeSprites) item.SpriteRenderer.sprite = isSS ? item.SpriteSS : item.SpriteTD;

        if (isSS) OnSS();
        else OnTD();
    }

    protected virtual void OnSS() { }

    protected virtual void OnTD() { }

    public void AddViewChangeSprites(SpriteRenderer spriteRenderer, Sprite spriteSS, Sprite spriteTD)
    {
        ViewChangeSprites viewChangeSprites = new() { SpriteRenderer = spriteRenderer, SpriteSS = spriteSS, SpriteTD = spriteTD };
        _viewChangeSprites.Add(viewChangeSprites);
    }
}