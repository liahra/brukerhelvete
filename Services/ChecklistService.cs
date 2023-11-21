using Noested.Data;
using Noested.Models;

namespace Noested.Services
{
    public class ChecklistService
	{
        private readonly IServiceOrderRepository _repository;
        private readonly ILogger<ChecklistService> _logger;

        public ChecklistService(IServiceOrderRepository repository, ILogger<ChecklistService> logger)
		{
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        ///     GET CHECKLIST BY ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Checklist</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Checklist> GetChecklistByIdAsync(int id)
        {
            var checklist = await _repository.GetChecklistByIdAsync(id);
            if (checklist == null)
            {
                throw new InvalidOperationException("Checklist not found");
            }
            return checklist;
        }

        /// <summary>
        ///     GET ALL CHECKLISTS
        /// </summary>
        /// <returns>IEnumerable of Checklist</returns>
        public async Task<IEnumerable<Checklist>> GetAllChecklistsAsync()
        {
            var checklists = await _repository.GetAllCheckListsAsync();
            if (checklists == null)
            {
                throw new InvalidOperationException("No checklists found");
            }
            return checklists;
        }

        /// <summary>
        ///     ADD CHECKLIST
        /// </summary>
        /// <param name="newChecklist"></param>
        /// <returns></returns>
        public async Task AddChecklistAsync(Checklist newChecklist)
        {
            if (newChecklist == null)
            {
                throw new InvalidOperationException("Checklist is null");
            }
            await _repository.AddChecklistAsync(newChecklist);
        }

        /// <summary>
        ///     UPDATE CHECKLIST
        /// </summary>
        /// <param name="updatedChecklist"></param>
        /// <returns></returns>
        public async Task UpdateChecklistAsync(Checklist updatedChecklist)
        {
            if (updatedChecklist == null)
            {
                throw new InvalidOperationException("Checklist is null");
            }
            await _repository.UpdateChecklistAsync(updatedChecklist);
        }

        /// <summary>
        ///     GET WINCH-CHECKLIST BY ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>WinchChecklist</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<WinchChecklist> GetWinchChecklistByIdAsync(int id)
        {
            var winchChecklist = await _repository.GetWinchChecklistByIdAsync(id);
            if (winchChecklist == null)
            {
                throw new InvalidOperationException("Checklist not found");
            }
            return winchChecklist;
        }

        /// <summary>
        ///     GET ALL WINCH CHECKLISTS
        /// </summary>
        /// <returns>IEnumerable of WinchChecklist</returns>
        public async Task<IEnumerable<WinchChecklist>> GetAllWinchChecklistsAsync()
        {
            var winchChecklists = await _repository.GetAllWinchChecklistsAsync();
            if (winchChecklists == null)
            {
                throw new InvalidOperationException("No winch checklists found");
            }
            return winchChecklists;
        }

        /// <summary>
        ///     ADD WINCH CHECKLIST
        /// </summary>
        /// <param name="newWinchChecklist"></param>
        /// <returns></returns>
        public async Task AddWinchChecklistAsync(WinchChecklist newWinchChecklist)
        {
            if (newWinchChecklist == null)
            {
                throw new InvalidOperationException("WinchChecklist is null");
            }
            await _repository.AddWinchChecklistAsync(newWinchChecklist);
        }

        /// <summary>
        ///     UPDATE WINCH CHECKLIST
        /// </summary>
        /// <param name="updatedWinchChecklist"></param>
        /// <returns></returns>
        public async Task UpdateWinchChecklistAsync(WinchChecklist updatedWinchChecklist)
        {
            if (updatedWinchChecklist == null)
            {
                throw new InvalidOperationException("WinchChecklist is null");
            }
            await _repository.UpdateWinchChecklistAsync(updatedWinchChecklist);
        }
    }
}

