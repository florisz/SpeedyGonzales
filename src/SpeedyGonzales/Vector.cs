namespace SpeedyGonzales
{
    public record Vector(
        int DeltaX,
        int DeltaY)
    {
        public Vector Flip() => new Vector(-DeltaX, -DeltaY);
    }
}
