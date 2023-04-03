using System.Collections.Generic;
using System.Linq;

namespace Agava.Merge2.Core
{
    public class ItemProduceInfo
    {
        private readonly CommandFilter _filter;

        public ItemProduceInfo(CommandFilter filter)
        {
            _filter = filter;
        }

        public IResult Compute(Item item)
        {
            var producers = _filter.Filter<ProduceCommand>();
            var producedItems = new List<Item>();
            var produceBy = new List<Item>();

            foreach (var pair in producers)
            {
                if (item.Id == pair.Key)
                {
                    foreach (var producerCommand in pair.Value)
                    {
                        producedItems.AddRange(producerCommand.Levels[item.Level].Items);
                    }
                }

                foreach (var producerCommand in pair.Value)
                {
                    for (int level = 0; level < producerCommand.Levels.Count; level++)
                    {
                        if (producerCommand.Levels[level].Items.Any(produceItem => produceItem.Id == item.Id 
                                                                        && produceItem.Level <= item.Level))
                            produceBy.Add(new Item(pair.Key, level));
                    }
                }
            }

            return new Result(producedItems, produceBy);
        }

        public interface IResult
        {
            IReadOnlyList<Item> ProducedItems { get; }
            IReadOnlyList<Item> ProduceBy { get; }
        }

        private class Result : IResult
        {
            private readonly IReadOnlyList<Item> _producedItems;
            private readonly IReadOnlyList<Item> _produceBy;

            public Result(IReadOnlyList<Item> producedItems, IReadOnlyList<Item> produceBy)
            {
                _producedItems = producedItems;
                _produceBy = produceBy;
            }

            IReadOnlyList<Item> IResult.ProducedItems => _producedItems;
            IReadOnlyList<Item> IResult.ProduceBy => _produceBy;
        }
    }
}
