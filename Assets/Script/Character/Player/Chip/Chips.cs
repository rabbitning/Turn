using UnityEngine;

public abstract class Chips : ScriptableObject
{
    public Sprite ChipIcon;
    public string ChipName;
    public string ChipDescription;

    protected virtual void OnEnable()
    {
        if(ChipIcon == null)
        {
            ChipIcon = Resources.Load<Sprite>("Chips/ChipsArt/DefaultChipIcon");
        }
        ChipName ??= "Default Chip Name";
        ChipDescription ??= "Default Chip Description";
    }
}