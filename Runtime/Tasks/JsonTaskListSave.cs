using Agava.Merge2.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Agava.Merge2.Tasks
{
    public class JsonTaskListSave
    {
        private IJsonSaveRepository _saveRepository;

        public JsonTaskListSave(IJsonSaveRepository saveRepository)
        {
            _saveRepository = saveRepository;
        }

        public void Save(TaskList taskList)
        {
            var json = JsonConvert.SerializeObject(taskList.Tasks);
            _saveRepository.Save(json);
        }

        public TaskList Load(IBoard board, TaskReward reward)
        {
            if (_saveRepository.HasSave == false)
                return new TaskList(board, reward);

            var json = _saveRepository.Load();
            var tasks = JsonConvert.DeserializeObject<IEnumerable<Task>>(json);
            return new TaskList(board, tasks, reward);
        }
    }
}
