namespace SpeedyGonzales
{
    public record struct Veld(
        Renner? Renner)
    {
        public override string ToString()
            => Renner == null
            ? " "
            : Renner.ToString();
    }
}
