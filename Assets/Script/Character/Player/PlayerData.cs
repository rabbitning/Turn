public static class PlayerData
{
    public static Chips[] Chips;

    public static void Save()
    {
        Chips = PlayerController.Instance.EquippedChips;
    }

    public static void Load()
    {
        PlayerController.Instance.UpdateChips(Chips);
    }
}
