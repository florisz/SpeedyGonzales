namespace SpeedyGonzales
{
    public class Bord
    {
        private readonly Veld[,] _board = new Veld[5, 5];

        public Veld this[Positie pos] => this[pos.X, pos.Y];

        public Veld this[int x, int y]
        {
            get
            {
                return _board[x, y];
            }
        }

        public static Bord Parse(string[] input)
        {
            var result = new Bord();

            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    switch (input[y][x])
                    {
                        case 'A':
                            result[x, y].Renner = new Renner(IsLeader: true, Team.A);
                            break;
                        case 'a':
                            result[x, y].Renner = new Renner(IsLeader: false, Team.A);
                            break;
                        case 'B':
                            result[x, y].Renner = new Renner(IsLeader: true, Team.B);
                            break;
                        case 'b':
                            result[x, y].Renner = new Renner(IsLeader: false, Team.B);
                            break;
                        case '.':
                            result[x, y].Renner = null;
                            break;
                        default:
                            throw new InvalidOperationException($"Invalid bord definitie: {string.Join(Environment.NewLine, input)}");
                    }
                }
            }

            return result;
        }

        public Bord Clone()
        {
            var result = new Bord();
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    result[x, y].Renner = this[x, y].Renner;
                }
            }
            return result;
        }
    }
}
