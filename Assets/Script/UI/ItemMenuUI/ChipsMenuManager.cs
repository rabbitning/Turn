using System.Collections.Generic;
using UnityEngine;

public class ChipsMenuManager : MonoBehaviour
{

    [Header("Equipped Chips")]

    [SerializeField] Transform _equippedChipsMenu = null;
    [SerializeField] EquippedChipsButtonController _equippedChipsButtonPrefab = null;
    EquippedChipsButtonController[] _newEquippedChips = new EquippedChipsButtonController[3];

    [Header("Chips List")]

    [SerializeField] Transform _chipsListMenu = null;
    [SerializeField] ChipButtonController _chipButtonPrefab = null;
    [SerializeField] List<Chips> _chipsList = null;

    PlayerController _player = null;

    void Start()
    {
        _player = GameManager.Instance.Player;

        for (int i = 0; i < _newEquippedChips.Length; i++)
        {
            _newEquippedChips[i] = Instantiate(_equippedChipsButtonPrefab, _equippedChipsMenu);
            _newEquippedChips[i].ChipsMenuManager = this;
        }

        _chipsList = new List<Chips>(Resources.LoadAll<Chips>("Chips"));
        foreach (Chips chip in _chipsList)
        {
            ChipButtonController chipButtonController = Instantiate(_chipButtonPrefab, _chipsListMenu);
            chipButtonController.ChipsMenuManager = this;
            chipButtonController.Chip = chip;
        }

        for (int i = 0; i < _newEquippedChips.Length; i++)
        {
            if (_player.EquippedChips[i] != null)
            {
                foreach (ChipButtonController chipButton in _chipsListMenu.GetComponentsInChildren<ChipButtonController>())
                {
                    Debug.Log("Chip: " + chipButton.Chip.name);
                    if (chipButton.Chip == _player.EquippedChips[i])
                    {
                        Debug.Log("Equipped: " + chipButton.Chip.name);
                        chipButton.OnClick();
                        break;
                    }
                }
            }
        }
    }

    // void OnEnable()
    // {
    //     _player = GameManager.Instance.Player;


    // }

    public bool EquippedChip(ChipButtonController chipButtonController)
    {
        for (int i = 0; i < _newEquippedChips.Length; i++)
        {
            if (_newEquippedChips[i].SourceChipButton == null)
            {
                _newEquippedChips[i].SetChip(chipButtonController);
                return true;
            }
        }
        return false;
    }

    public void UnequippedChip(ChipButtonController chipButtonController)
    {
        for (int i = 0; i < _newEquippedChips.Length; i++)
        {
            if (_newEquippedChips[i].SourceChipButton == null) continue;

            if (_newEquippedChips[i].SourceChipButton.Chip == chipButtonController.Chip)
            {
                chipButtonController.SetButtonInteractable(true);
                _newEquippedChips[i].SetChip(null);
                break;
            }
        }
    }

    public void ApplyChips()
    {
        Chips[] newChips = new Chips[_newEquippedChips.Length];

        for (int i = 0; i < newChips.Length; i++)
        {
            if (_newEquippedChips[i] != null)
                if (_newEquippedChips[i].SourceChipButton != null)
                    if (_newEquippedChips[i].SourceChipButton.Chip != null)
                        newChips[i] = _newEquippedChips[i].SourceChipButton.Chip;
        }
        _player.UpdateChips(newChips);
        // gameObject.SetActive(false);
    }
}