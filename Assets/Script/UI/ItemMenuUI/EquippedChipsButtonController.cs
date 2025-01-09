using UnityEngine;
// using UnityEngine.UI;

public class EquippedChipsButtonController : MonoBehaviour
{
//     [HideInInspector] public ChipsMenuManager ChipsMenuManager = null;
//     [HideInInspector] public ChipButtonController SourceChipButton = null;
//     Button _button = null;

//     void Start()
//     {
//         _button = GetComponent<Button>();
//         _button.interactable = false;
//         _button.onClick.AddListener(OnClick);
//         _button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Chips/ChipsArt/DefaultChipIcon");
//     }

//     void OnClick()
//     {
//         ChipsMenuManager.UnequippedChip(SourceChipButton);
//         SetButtonInteractable(false);
//     }


//     public void SetChip(ChipButtonController chipButtonController)
//     {
//         if (chipButtonController == null)
//         {
//             SourceChipButton = null;
//             _button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Chips/ChipsArt/DefaultChipIcon");
//             return;
//         }

//         SourceChipButton = chipButtonController;
//         _button.GetComponent<Image>().sprite = SourceChipButton.Chip.ChipIcon;
//         SetButtonInteractable(true);
//     }

//     public void SetButtonInteractable(bool interactable)
//     {
//         _button.interactable = interactable;
//     }
}
