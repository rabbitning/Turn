using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUIController : UIController
{
    [SerializeField] Image _viewChangeCdImg = null;
    [SerializeField] Image _playerHpBar = null;
    [SerializeField] TextMeshProUGUI _playerHpNum = null;
    PlayerController _player = null;

    [Space(10)]
    [SerializeField] BossController _boss = null;
    [SerializeField] Image _bossHpBar = null;

    protected override void Start()
    {
        base.Start();
        _player = GameManager.Instance.Player;
    }

    void LateUpdate()
    {
        UpdatePlayerHp();
        UpdateBossHp();
    }

    void UpdatePlayerHp()
    {
        if (_player == null) return;

        float currentHealth = _player.CurrentStatsData[StatName.Health];
        float maxHealth = _player.CurrentStatsData[StatName.MaxHealth];

        if (maxHealth <= 0) return;

        _playerHpBar.fillAmount = Mathf.Lerp(_playerHpBar.fillAmount, currentHealth / maxHealth, Time.deltaTime * 5 + 0.01f);
        _playerHpNum.SetText($"{Mathf.FloorToInt(currentHealth)} / {Mathf.FloorToInt(maxHealth)}");
    }

    void UpdateBossHp()
    {
        if (_boss == null) return;

        float currentHealth = _boss.CurrentStatsData[StatName.Health];
        float maxHealth = _boss.CurrentStatsData[StatName.MaxHealth];

        if (maxHealth <= 0) return;

        _bossHpBar.fillAmount = Mathf.Lerp(_bossHpBar.fillAmount, currentHealth / maxHealth, Time.deltaTime * 5 + 0.01f);
    }

    protected override void ViewChanged(bool nextView)
    {
        if (_viewChangeCdImg != null)
        {
            StartCoroutine(FillCdImg(GameManager.Instance.ViewChangeCooldown));
        }
    }

    IEnumerator FillCdImg(float cd)
    {
        float cdTimer = 0;
        while (cdTimer <= cd)
        {
            _viewChangeCdImg.fillAmount = 1 - cdTimer / cd;
            yield return null;
            cdTimer += Time.deltaTime;
        }
        _viewChangeCdImg.fillAmount = 0;
    }
}