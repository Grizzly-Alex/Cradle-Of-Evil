namespace Extensions
{
    public static class IntExtensions
    {
        public static bool IsNegative(this int value) => value < 0;
        public static bool IsPositive(this int value) => value > 0;
        public static bool IsNotZero(this int value) => value > 0;
    }
}
