using System.Collections.Generic;
using UnityEngine;

public class PlayerChipManager : MonoBehaviour
{
    PlayerController _player;
    [HideInInspector] public List<Chips> AvailableChips; // 所有可用的晶片
    public Chips[] EquippedChips = new Chips[3]; // 玩家裝備的晶片

    private void Awake()
    {
        AvailableChips = new List<Chips>(Resources.LoadAll<Chips>("Chips"));
    }

    void Start()
    {
        LoadEquippedChips();
        _player = PlayerController.Instance;
        _player.UpdateChips(EquippedChips);
    }

    void Update()
    {
        foreach (Chips chip in EquippedChips)
        {
            if (chip is PassiveChips passiveChip)
            {
                passiveChip.ApplyPassiveEffect(_player);
            }
        }
    }

    public void EquipChip(int slotIndex, Chips chip)
    {
        if (slotIndex < 0 || slotIndex >= EquippedChips.Length) return;
        EquippedChips[slotIndex] = chip;

        _player.UpdateChips(EquippedChips);
        SaveEquippedChips();
    }

    public void UnequipChip(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedChips.Length) return;
        EquippedChips[slotIndex] = null;

        _player.UpdateChips(EquippedChips);
        SaveEquippedChips();
    }

    private void SaveEquippedChips()
    {
        for (int i = 0; i < EquippedChips.Length; i++)
        {
            if (EquippedChips[i] != null)
            {
                PlayerPrefs.SetString($"EquippedChip_{i}", EquippedChips[i].name);
            }
            else
            {
                PlayerPrefs.DeleteKey($"EquippedChip_{i}");
            }
        }
        PlayerPrefs.Save();
    }

    private void LoadEquippedChips()
    {
        for (int i = 0; i < EquippedChips.Length; i++)
        {
            string chipName = PlayerPrefs.GetString($"EquippedChip_{i}", null);
            if (!string.IsNullOrEmpty(chipName))
            {
                EquippedChips[i] = AvailableChips.Find(chip => chip.name == chipName);
            }
        }
    }

    public void ActiveChip(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedChips.Length) return;
        if (EquippedChips[slotIndex] != null)
        {
            EquippedChips[slotIndex].Active(_player);
        }
    }
}