// using System.Collections.Generic;
// using TMPro;
using UnityEngine;
// using UnityEngine.UI;

public class ChipsMenuManager : MonoBehaviour
{
//     [SerializeField] GameObject _chipButtonPrefab = null;

//     [Header("Equipped Chips")]

//     [SerializeField] Transform _equippedChipsPanel = null;

//     [Header("Chips List")]

//     [SerializeField] Transform _availableChipsPanel = null;

//     PlayerController _player = null;
//     PlayerChipManager _playerChipManager;

//     void Start()
//     {
//         _player = GameManager.Instance.Player;
//         _playerChipManager = _player.GetComponent<PlayerChipManager>();

//         PopulateAvailableChips();
//         PopulateEquippedChips();
//     }

//     private void PopulateAvailableChips()
//     {
//         foreach (Chips chip in _playerChipManager.AvailableChips)
//         {
//             GameObject chipButton = Instantiate(_chipButtonPrefab, _availableChipsPanel);
//             chipButton.GetComponentInChildren<TextMeshProUGUI>().text = chip.ChipName;
//             chipButton.GetComponent<Image>().sprite = chip.ChipIcon;
//             chipButton.GetComponent<Button>().onClick.AddListener(() => OnChipSelected(chip));
//         }
//     }

//     private void PopulateEquippedChips()
//     {
//         for (int i = 0; i < _playerChipManager.EquippedChips.Length; i++)
//         {
//             Chips chip = _playerChipManager.EquippedChips[i];
//             GameObject chipButton = Instantiate(_chipButtonPrefab, _equippedChipsPanel);
//             chipButton.GetComponentInChildren<TextMeshProUGUI>().text = chip != null ? chip.ChipName : "Empty";
//             chipButton.GetComponent<Image>().sprite = chip != null ? chip.ChipIcon : null;
//             int slotIndex = i;
//             chipButton.GetComponent<Button>().onClick.AddListener(() => OnEquippedChipSelected(slotIndex));
//         }
//     }

//     private void OnChipSelected(Chips chip)
//     {
//         // 檢查是否已經裝備了相同的晶片
//         for (int i = 0; i < _playerChipManager.EquippedChips.Length; i++)
//         {
//             if (_playerChipManager.EquippedChips[i] == chip)
//             {
//                 Debug.LogWarning("Cannot equip the same chip multiple times.");
//                 return;
//             }
//         }

//         for (int i = 0; i < _playerChipManager.EquippedChips.Length; i++)
//         {
//             if (_playerChipManager.EquippedChips[i] == null)
//             {
//                 _playerChipManager.EquipChip(i, chip);
//                 UpdateEquippedChipsUI();
//                 return;
//             }
//         }
//     }

//     private void OnEquippedChipSelected(int slotIndex)
//     {
//         _playerChipManager.UnequipChip(slotIndex);
//         UpdateEquippedChipsUI();
//     }

//     private void UpdateEquippedChipsUI()
//     {
//         foreach (Transform child in _equippedChipsPanel)
//         {
//             Destroy(child.gameObject);
//         }
//         PopulateEquippedChips();
//     }
}