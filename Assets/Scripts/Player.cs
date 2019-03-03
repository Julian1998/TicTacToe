public class Player : IPlayer
{
    public string HexColor { get; private set; }
    public string Name { get; private set; }
    public string Sign { get; private set; }
    public string Status { get; private set; }

    public Player()
    {
        HexColor = "#3598AD";
        Name = "Player";
        Sign = "X";
        Status = "Victory!";
    }
}
