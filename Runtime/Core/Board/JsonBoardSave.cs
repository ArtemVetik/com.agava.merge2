using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agava.Merge2.Core
{
    public class JsonBoardSave
    {
        private readonly IJsonSaveRepository _saveRepository;

        public JsonBoardSave(IJsonSaveRepository saveRepository)
        {
            _saveRepository = saveRepository;
        }

        public IBoard Load(IShape shape, IContourAlgorithm contourAlgorithm)
        {
            if (_saveRepository.HasSave == false)
                throw new InvalidOperationException();

            var json = _saveRepository.Load();
            var save = JsonConvert.DeserializeObject<BoardSave>(json);

            var board = new Board(shape, contourAlgorithm, save.OpenedPositions);

            foreach (var itemPair in save.Items)
                board.Add(itemPair.Item, itemPair.Position);

            return board;
        }

        public void Save(IBoard board)
        {
            var save = new BoardSave
            {
                OpenedPositions = board.OpenedCollection,
                Items = CreateItemsPair(board),
            };

            var json = JsonConvert.SerializeObject(save);
            _saveRepository.Save(json);
        }

        private IReadOnlyCollection<Pair> CreateItemsPair(IBoard board)
        {
            var items = new List<Pair>();

            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    var position = new MapCoordinate(x, y);

                    if (board.HasItem(position))
                        items.Add(new Pair(board.Item(position), position));
                }
            }

            return items;
        }

        [Serializable]
        private struct BoardSave
        {
            [JsonProperty] public IReadOnlyCollection<MapCoordinate> OpenedPositions;
            [JsonProperty] public IReadOnlyCollection<Pair> Items;
        }

        [Serializable]
        private struct Pair
        {
            [JsonProperty] private Item _item;
            [JsonProperty] private MapCoordinate _position;

            public Pair(Item item, MapCoordinate position)
            {
                _item = item;
                _position = position;
            }

            [JsonIgnore] public Item Item => _item;
            [JsonIgnore] public MapCoordinate Position => _position;
        }
    }
}
