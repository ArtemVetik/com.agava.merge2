using Newtonsoft.Json;
using System.Collections.Generic;

namespace Agava.Merge2.Core
{
    public class OpenedItemListSave
    {
        private readonly IJsonSaveRepository _saveRepository;

        public OpenedItemListSave(IJsonSaveRepository saveRepository)
        {
            _saveRepository = saveRepository;
        }

        public void Save(OpenedItemList openedItemList)
        {
            var json = JsonConvert.SerializeObject(openedItemList.OpenedItems);
            _saveRepository.Save(json);
        }

        public OpenedItemList Load(IReadOnlyBoard board)
        {
            if (_saveRepository.HasSave == false)
                return new OpenedItemList(board);

            var openedItems = JsonConvert.DeserializeObject<IEnumerable<Item>>(_saveRepository.Load());
            return new OpenedItemList(board, openedItems);
        }
    }
}
