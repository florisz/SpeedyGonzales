namespace SpeedyGonzales
{
    public record Veld()
    {
        public Renner? Renner { get; set; }

        public override string ToString()
            => Renner == null
            ? " "
            : Renner.ToString();
    }
}
