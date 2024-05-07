using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MenuController
{
    [SerializeField] Image viewChangeCdImg = null;

    void Start()
    {
        GameManager.gameManager.OnViewChanged.AddListener(ViewChanged);
    }

    // void Update()
    // {

    // }

    void ViewChanged(bool currentView)
    {
        if (viewChangeCdImg)
        {
            StartCoroutine(FillCdImg(GameManager.gameManager.changeViewCd));
        }
    }

    IEnumerator FillCdImg(float cd)
    {
        float cdTimer = 0;
        while (cdTimer <= cd)
        {
            viewChangeCdImg.fillAmount = 1 - cdTimer / cd;
            yield return null;
            cdTimer += Time.deltaTime;
        }
    }
}