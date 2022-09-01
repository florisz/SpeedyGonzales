namespace SpeedyGonzales
{
    public record struct Positie(
        int X,
        int Y)
    {
        public bool IsOnBoard => X >= 0 && Y >= 0 && X < 5 && Y < 5;

        public override string ToString()
            => (char)((int)'A' + X) + (Y + 1).ToString();

        public static Positie Parse(string input)
        {
            var x = input[0] - 'A';
            var y = input[1] - '1';
            return new Positie(x, y);
        }
    }
}
