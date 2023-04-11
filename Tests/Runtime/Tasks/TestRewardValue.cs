using System.Linq;

namespace Agava.Merge2.Tasks.Tests
{
    public class TestRewardValue : IRewardValue
    {
        public int Reward(Task task)
        {
            return task.TotalItems.Sum(task => task.Level + 1);
        }
    }
}
