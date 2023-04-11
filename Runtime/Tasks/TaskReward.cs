
namespace Agava.Merge2.Tasks
{
    public class TaskReward
    {
        private readonly IRewardCurrency _rewardCurrency;
        private readonly IRewardValue _rewardValue;

        public TaskReward(IRewardCurrency rewardCurrency, IRewardValue rewardValue)
        {
            _rewardCurrency = rewardCurrency;
            _rewardValue = rewardValue;
        }

        public int RewardValue(Task task) => _rewardValue.Reward(task);

        internal void Award(Task task)
        {
            _rewardCurrency.Add(_rewardValue.Reward(task));
        }
    }
}
