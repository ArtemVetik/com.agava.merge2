namespace Agava.Merge2.Core
{
    public struct Cooldown
    {
        public readonly int MaxClicks;
        public readonly int ColldownSeconds;

        public Cooldown(int maxClicks, int colldownSecond)
        {
            MaxClicks = maxClicks;
            ColldownSeconds = colldownSecond;
        }
    }
}
