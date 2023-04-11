namespace Agava.Merge2.Tasks.Tests
{
    public class TestCurrency : IRewardCurrency
    {
        public int Value { get; private set; }

        public void Add(int value)
        {
            Value += value;
        }
    }
}
