using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChipButtonController : MonoBehaviour
{
    [HideInInspector] public ChipsMenuManager ChipsMenuManager = null;
    [HideInInspector] public Chips Chip = null;
    [SerializeField] GameObject _chipDescriptionBG = null;
    [SerializeField] TextMeshProUGUI _chipDescription = null;
    Button _button = null;
    // bool IsEquipped = false;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        if (Chip != null)
        {
            _button.GetComponent<Image>().sprite = Chip.ChipIcon;
            _chipDescription.text = Chip.ChipDescription;
            _chipDescriptionBG.SetActive(false);
        }
    }

    public void OnClick()
    {
        SetButtonInteractable(!ChipsMenuManager.EquippedChip(this));
    }

    public void OnMouseEnter()
    {
        if (Chip != null)
        {
            _chipDescriptionBG.SetActive(true);
        }
    }

    public void OnMouseExit()
    {
        _chipDescriptionBG.SetActive(false);
    }

    public void SetChip(ChipButtonController chipButtonController)
    {
        if (chipButtonController == null)
        {
            Chip = null;
            _button = null;
            _button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Chips/ChipsArt/DefaultChipIcon");
            return;
        }

        Chip = chipButtonController.Chip;
        _button = chipButtonController._button;
        _button.GetComponent<Image>().sprite = Chip.ChipIcon;
    }

    public void SetButtonInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }
}