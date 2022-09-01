using System.Text;

namespace SpeedyGonzales
{
    public class Bord
    {
        private const int XWidth = 5;
        private const int YHeight = 5;
        private const int VeldCount = XWidth * YHeight;

        private readonly Veld[] _board = new Veld[VeldCount];

        //public Bord()
        //{
        //    for (int i = 0; i < VeldCount; i++)
        //    {
        //        _board[i] = new Veld();
        //    }
        //}

        private int CalcPos(int x, int y) => y * XWidth + x;

        public void SetRenner(int x, int y, Renner? renner) => _board[CalcPos(x, y)].Renner = renner;

        public Veld this[Positie pos] => _board[CalcPos(pos.X, pos.Y)];

        public Veld this[int x, int y] => _board[CalcPos(x, y)];

        public IEnumerable<Positie> GetRennerPosities(Team team)
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    var veld = this[x, y];
                    if (veld.Renner != null && veld.Renner.Team == team)
                    {
                        yield return new Positie(x, y);
                    }
                }
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
                            result.SetRenner(x,y, new Renner(IsLeader: true, Team.A));
                            break;
                        case 'a':
                            result.SetRenner(x, y, new Renner(IsLeader: false, Team.A));
                            break;
                        case 'B':
                            result.SetRenner(x, y, new Renner(IsLeader: true, Team.B));
                            break;
                        case 'b':
                            result.SetRenner(x, y, new Renner(IsLeader: false, Team.B));
                            break;
                        case '.':
                            result.SetRenner(x, y, null);
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
            for (int i = 0; i < VeldCount; i++)
            {
                result._board[i] = this._board[i];
            }
            return result;
        }

        public void WriteTo(Action<string> writeLine)
        {
            for (int y = 0; y < 5; y++)
            {
                var sb = new StringBuilder(5);
                for (int x = 0; x < 5; x++)
                {
                    sb.Append(this[x, y].ToString());
                }
                writeLine(sb.ToString());
            }
        }
    }
}
