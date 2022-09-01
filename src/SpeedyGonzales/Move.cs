namespace SpeedyGonzales
{
    public record Move(
        string KaartId,
        Positie Van,
        Positie Naar)
    {
        public static Move Parse(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var van = Positie.Parse(parts[1]);
            var naar = Positie.Parse(parts[2]);
            return new Move(parts[0], van, naar);
        }
    }
}
