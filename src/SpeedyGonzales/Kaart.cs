﻿namespace SpeedyGonzales
{
    public record Kaart(
        Team? Owner,
        string Id,
        Vector[] Bewegingen)
    {
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
    }
}
