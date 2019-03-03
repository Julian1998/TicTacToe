public class Computer : IPlayer
{
    public string HexColor { get; private set; }
    public string Name { get; private set; }
    public string Sign { get; private set; }
    public string Status { get; private set; }

    public Computer()
    {
        HexColor = "#AE3578";
        Name = "Computer";
        Sign = "O";
        Status = "Game Over!";
    }
}
