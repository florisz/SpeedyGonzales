using System.Text;

namespace SpeedyGonzales
{
    public record Kaart(
        Team? Owner,
        string Id,
        Vector[] Bewegingen)
    {
        public Kaart ChangeOwner(Team owner) 
            => this with { Owner = owner };

        public Kaart MoveToStack()
        {
            return this with { 
                Owner = null,
                Bewegingen = Bewegingen
                    .Select(b => b.Flip())
                    .ToArray()
            };
        }

        public static Kaart Parse(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Team? owner = parts[0] == "-1"
                ? null
                : Team.Parse(parts[0]);
            var id = parts[1];
            var bewegingen = new List<Vector>();

            for (int idx = 2; idx < parts.Length; idx += 2)
            {
                var dX = int.Parse(parts[idx]);
                var dY = int.Parse(parts[idx + 1]);
                if (dX != 0 || dY != 0)
                {
                    bewegingen.Add(new Vector(dX, dY));
                }
            }

            return new Kaart(
                owner,
                id,
                bewegingen.ToArray());
        }

        public override string ToString()
        {
            var sb = new StringBuilder(40);
            sb.Append(Owner == null ? "-1" : Owner.Number.ToString());
            sb.Append(' ');
            sb.Append(Id);
            foreach (var b in Bewegingen)
            {
                sb.Append(' ');
                sb.Append(b.DeltaX.ToString());
                sb.Append(' ');
                sb.Append(b.DeltaY.ToString());
            }
            return sb.ToString();
        }
    }
}
