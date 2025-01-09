using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChipsMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _chipButtonPrefab = null;

    [Header("Equipped Chips")]

    [SerializeField] Transform _equippedChipsPanel = null;
    // [SerializeField] EquippedChipsButtonController _equippedChipsButtonPrefab = null;
    // EquippedChipsButtonController[] _newEquippedChips = new EquippedChipsButtonController[3];

    [Header("Chips List")]

    [SerializeField] Transform _availableChipsPanel = null;
    // [SerializeField] ChipButtonController _chipButtonPrefab = null;
    // [SerializeField] List<Chips> _chipsList = null;

    PlayerController _player = null;
    PlayerChipManager _playerChipManager;

    void Start()
    {
        _player = GameManager.Instance.Player;
        _playerChipManager = _player.GetComponent<PlayerChipManager>();

        PopulateAvailableChips();
        PopulateEquippedChips();
        // for (int i = 0; i < _newEquippedChips.Length; i++)
        // {
        //     _newEquippedChips[i] = Instantiate(_equippedChipsButtonPrefab, _equippedChipsMenu);
        //     _newEquippedChips[i].ChipsMenuManager = this;
        // }

        // _chipsList = new List<Chips>(Resources.LoadAll<Chips>("Chips"));
        // foreach (Chips chip in _chipsList)
        // {
        //     ChipButtonController chipButtonController = Instantiate(_chipButtonPrefab, _chipsListMenu);
        //     chipButtonController.ChipsMenuManager = this;
        //     chipButtonController.Chips = chip;
        // }

        // for (int i = 0; i < _newEquippedChips.Length; i++)
        // {
        //     if (_player.EquippedChips[i] != null)
        //     {
        //         foreach (ChipButtonController chipButton in _chipsListMenu.GetComponentsInChildren<ChipButtonController>())
        //         {
        //             if (chipButton.Chips == _player.EquippedChips[i])
        //             {
        //                 chipButton.OnClick();
        //                 break;
        //             }
        //         }
        //     }
        // }
    }

    // void OnEnable()
    // {
    //     _player = GameManager.Instance.Player;


    // }

    private void PopulateAvailableChips()
    {
        foreach (Chips chip in _playerChipManager.AvailableChips)
        {
            GameObject chipButton = Instantiate(_chipButtonPrefab, _availableChipsPanel);
            chipButton.GetComponentInChildren<TextMeshProUGUI>().text = chip.ChipName;
            chipButton.GetComponent<Image>().sprite = chip.ChipIcon;
            chipButton.GetComponent<Button>().onClick.AddListener(() => OnChipSelected(chip));
        }
    }

    private void PopulateEquippedChips()
    {
        for (int i = 0; i < _playerChipManager.EquippedChips.Length; i++)
        {
            Chips chip = _playerChipManager.EquippedChips[i];
            GameObject chipButton = Instantiate(_chipButtonPrefab, _equippedChipsPanel);
            chipButton.GetComponentInChildren<TextMeshProUGUI>().text = chip != null ? chip.ChipName : "Empty";
            chipButton.GetComponent<Image>().sprite = chip != null ? chip.ChipIcon : null;
            int slotIndex = i;
            chipButton.GetComponent<Button>().onClick.AddListener(() => OnEquippedChipSelected(slotIndex));
        }
    }

    private void OnChipSelected(Chips chip)
    {
        // 檢查是否已經裝備了相同的晶片
        for (int i = 0; i < _playerChipManager.EquippedChips.Length; i++)
        {
            if (_playerChipManager.EquippedChips[i] == chip)
            {
                Debug.LogWarning("Cannot equip the same chip multiple times.");
                return;
            }
        }

        for (int i = 0; i < _playerChipManager.EquippedChips.Length; i++)
        {
            if (_playerChipManager.EquippedChips[i] == null)
            {
                _playerChipManager.EquipChip(i, chip);
                UpdateEquippedChipsUI();
                return;
            }
        }
    }

    private void OnEquippedChipSelected(int slotIndex)
    {
        _playerChipManager.UnequipChip(slotIndex);
        UpdateEquippedChipsUI();
    }

    private void UpdateEquippedChipsUI()
    {
        foreach (Transform child in _equippedChipsPanel)
        {
            Destroy(child.gameObject);
        }
        PopulateEquippedChips();
    }

    // public bool EquippedChip(ChipButtonController chipButtonController)
    // {
    //     for (int i = 0; i < _newEquippedChips.Length; i++)
    //     {
    //         if (_newEquippedChips[i].SourceChipButton == null)
    //         {
    //             _newEquippedChips[i].SetChip(chipButtonController);
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // public void UnequippedChip(ChipButtonController chipButtonController)
    // {
    //     for (int i = 0; i < _newEquippedChips.Length; i++)
    //     {
    //         if (_newEquippedChips[i].SourceChipButton == null) continue;

    //         if (_newEquippedChips[i].SourceChipButton.Chip == chipButtonController.Chip)
    //         {
    //             chipButtonController.SetButtonInteractable(true);
    //             _newEquippedChips[i].SetChip(null);
    //             break;
    //         }
    //     }
    // }

    // public void ApplyChips()
    // {
    //     Chips[] newChips = new Chips[_newEquippedChips.Length];

    //     for (int i = 0; i < newChips.Length; i++)
    //     {
    //         if (_newEquippedChips[i] != null)
    //             if (_newEquippedChips[i].SourceChipButton != null)
    //                 if (_newEquippedChips[i].SourceChipButton.Chip != null)
    //                     newChips[i] = _newEquippedChips[i].SourceChipButton.Chip;
    //     }
    //     _player.UpdateChips(newChips);
    //     // gameObject.SetActive(false);
    // }
}