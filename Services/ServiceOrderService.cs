using System.Text.Json;
using Noested.Data;
using Noested.Models;

namespace Noested.Services
{
    public class ServiceOrderService
    {
        private readonly IServiceOrderRepository _repository;
        private readonly ChecklistService _checklistService;
        private readonly ILogger<ServiceOrderService> _logger;

        public ServiceOrderService(IServiceOrderRepository repository, ChecklistService checklistService, ILogger<ServiceOrderService> logger)
        {
            _repository = repository;
            _logger = logger;
            _checklistService = checklistService;
        }

        // GET SO
        public async Task<ServiceOrder> GetOrderByIdAsync(int id)
        {
            var order = await _repository.GetOrderByIdAsync(id);
            return order ?? throw new InvalidOperationException($"SERVICE ORDER ({id}) NOT FOUND");
        }

        // VIEW FOR OPEN SO
        public async Task<FillOrderViewModel> PopulateOrderViewModel(int id)
        {
            var order = await GetOrderByIdAsync(id);

            var viewModel = new FillOrderViewModel
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                ProductName = order.ProductName,
                ProductT = order.Product,
                OrderStatus = order.Status,
                OrderReceived = order.OrderReceived,
                OrderDescription = order.OrderDescription,
                UpdWinchlist = order.Product == ProductType.Winch ? await _checklistService.GetWinchChecklistByIdAsync(id) : null
            };

            return viewModel;
        }

        public async Task<IEnumerable<ServiceOrder>> Search(String query)
        {
            _logger.LogInformation("Search(): Called");
            if (query == null)
            {
                _logger.LogError("Search(): Null argument");
            }
            var searchResults = await _repository.Search(query);
            if (searchResults == null || !searchResults.Any())
            {
                _logger.LogError("Search(): No results found");
            }
            return searchResults;
        }

        public async Task<bool> CreateNewServiceOrderAsync(OrderViewModel viewModel)
        {
            IEnumerable<ServiceOrder> allServiceOrders = await _repository.GetAllServiceOrdersAsync(); // Duplicate Order?
            IEnumerable<Customer> allCustomers = await _repository.GetAllCustomersAsync(); // Duplicate Customer?

            ServiceOrder? newOrder = viewModel.NewOrder;
            Customer? newCustomer = viewModel.NewCustomer;
            Checklist? newChecklist = viewModel.NewChecklist;

            if (newOrder == null || newChecklist == null)
            {
                throw new InvalidOperationException("CreateNewSOA: Order or Checklist is Null!");
            }

            var viewModelJson = JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { WriteIndented = true }); // Debug
            _logger.LogInformation("CreateOrder ViewModel: {ViewModelJson}", viewModelJson); // Debug

            var duplicateOrder = allServiceOrders.FirstOrDefault(o =>
                newOrder != null &&
                o.ProductName == newOrder.ProductName &&
                o.SerialNumber == newOrder.SerialNumber &&
                o.OrderReceived.Date == newOrder.OrderReceived.Date // Same day?
            );

            if (duplicateOrder != null)
            {
                _logger.LogError("Duplicate ServiceOrder found. Skipping this order.");
                return false;
            }

            if (newOrder!.CustomerId == 0) // New customer?
            {
                Customer? existingCustomer = allCustomers.FirstOrDefault(c => c.Email == newCustomer!.Email);
                if (existingCustomer != null)
                {
                    newOrder.CustomerId = existingCustomer.CustomerId;
                }
                else
                {
                    await _repository.AddCustomerAsync(newCustomer!);
                    newOrder.CustomerId = newCustomer!.CustomerId;
                }
            }
            switch (newOrder.Product)
            {
                case ProductType.Winch:
                    var winchChecklist = new WinchChecklist();
                    {
                        winchChecklist.ProductType = newChecklist!.ProductType;
                        winchChecklist.PreparedBy = newChecklist.PreparedBy;
                        winchChecklist.ServiceProcedure = newChecklist.ServiceProcedure;
                    };
                    newOrder.Checklist = winchChecklist;
                    break;
                    // case ProductType.LiftEquip:
            }


            await _repository.AddServiceOrderAsync(newOrder); // Save order


            _logger.LogInformation("CreateOrder ViewModel: {ViewModelJson}", viewModelJson); // Debug
            return true;

        }
        /// <summary>
        ///     ALL SERVICEORDERS
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IEnumerable<ServiceOrder>> GetAllServiceOrdersAsync()
        {
            var allServiceOrders = await _repository.GetAllServiceOrdersAsync();
            if (allServiceOrders == null || !allServiceOrders.Any())
            {
                throw new InvalidOperationException("Null or no ServiceOrders in database");
            }
            return allServiceOrders;
        }


        public async Task<bool> UpdateCompletedOrderAsync(FillOrderViewModel form)
        {
            var viewModelJson = JsonSerializer.Serialize(form, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogInformation("UPDATE COMPLETED ORDER: {ViewModelJson}", viewModelJson);

            ServiceOrder? dbOrder = await _repository.GetOrderByIdAsync(form.OrderId);
            Checklist? dbChecklist = await _repository.GetChecklistByIdAsync(form.OrderId);

            if (dbOrder != null || dbChecklist != null)
            {
                // Checklist baseklasse
                dbChecklist!.MechSignature = form.MechSignature;
                dbChecklist.RepairComment = form.RepairComment;
                dbChecklist.DateCompleted = form.OrderCompleted;

                // Update Order Properties
                dbOrder!.WorkHours = form.WorkHours;
                dbOrder.Status = form.OrderStatus;
                dbOrder.OrderCompleted = DateTime.Now;

            } else
            {
                _logger.LogError("Order or Checklist with ID {OrderId} not found.", form.OrderId);
                return false;
            }

            

            // Checklist should match the one stored in DB
            if (dbChecklist is WinchChecklist dbWinch && form.UpdWinchlist is WinchChecklist updWinch)
            {
                dbWinch.MechBrakes = updWinch.MechBrakes;
                dbWinch.MechDrumBearing = updWinch.MechDrumBearing;
                dbWinch.MechStoragePTO = updWinch.MechStoragePTO;
                dbWinch.MechWire = updWinch.MechWire;
                dbWinch.MechChainTensioner = updWinch.MechChainTensioner;
                dbWinch.MechPinionBearing = updWinch.MechPinionBearing;
                dbWinch.MechClutch = updWinch.MechClutch;
                dbWinch.MechSprocketWedges = updWinch.MechSprocketWedges;
                dbWinch.HydCylinder = updWinch.HydCylinder;
                dbWinch.HydHydraulicBlock = updWinch.HydHydraulicBlock;
                dbWinch.HydTankOil = updWinch.HydTankOil;
                dbWinch.HydGearboxOil = updWinch.HydGearboxOil;
                dbWinch.HydBrakeCylinder = updWinch.HydBrakeCylinder;
                dbWinch.ElCableNetwork = updWinch.ElCableNetwork;
                dbWinch.ElRadio = updWinch.ElRadio;
                dbWinch.ElButtonBox = updWinch.ElButtonBox;
                dbWinch.TensionCheckBar = updWinch.TensionCheckBar;
                dbWinch.TestWinch = updWinch.TestWinch;
                dbWinch.TestTraction = updWinch.TestTraction;
                dbWinch.TestBrakes = updWinch.TestBrakes;

                await _repository.UpdateWinchChecklistAsync(dbWinch);
            }
            else
            {
                _logger.LogError("Checklist with ID {OrderId} is not of type WinchChecklist.", form.OrderId);
                return false;
            }

            

            await _repository.UpdateOrderAsync(dbOrder); // Save to DB.

            return true;
        }
    }
}

        /*
        /// <summary>
        ///     Updates Checklist in an existing order
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCompletedOrderAsync(FillOrderViewModel form)
        {
            var viewModelJson = JsonSerializer.Serialize(form, new JsonSerializerOptions { WriteIndented = true }); // Debug
            _logger.LogInformation("UPDATE COMPLETED ORDER: {ViewModelJson}", viewModelJson); // Debug

            ServiceOrder? DbOrder = await _repository.GetOrderByIdAsync(form.OrderId);
            Checklist? DbChecklist = await _repository.GetChecklistByIdAsync(form.OrderId);

            if (DbOrder == null || DbChecklist == null)
            {
                _logger.LogError("Neither Order or Checklist with ID {OrderId} found.", form.OrderId);
                return false;
            }

            if (DbChecklist is WinchChecklist DbWinch)
            {
                if (form.UpdWinchlist is not WinchChecklist updCheck)
                {
                    _logger.LogError("Checklist ({NewChecklist}) is not of type Winch", form.UpdWinchlist);
                    return false;
                }

                DbWinch.MechSignature = updCheck!.MechSignature;
                DbWinch.RepairComment = updCheck.RepairComment;
                DbWinch.DateCompleted = updCheck.DateCompleted;
                DbWinch.MechBrakes = updCheck.MechBrakes;
                DbWinch.MechDrumBearing = updCheck.MechDrumBearing;
                DbWinch.MechStoragePTO = updCheck.MechStoragePTO;
                DbWinch.MechWire = updCheck.MechWire;
                DbWinch.MechChainTensioner = updCheck.MechChainTensioner;
                DbWinch.MechPinionBearing = updCheck.MechPinionBearing;
                DbWinch.MechClutch = updCheck.MechClutch;
                DbWinch.MechSprocketWedges = updCheck.MechSprocketWedges;
                DbWinch.HydCylinder = updCheck.HydCylinder;
                DbWinch.HydHydraulicBlock = updCheck.HydHydraulicBlock;
                DbWinch.HydTankOil = updCheck.HydTankOil;
                DbWinch.HydGearboxOil = updCheck.HydGearboxOil;
                DbWinch.HydBrakeCylinder = updCheck.HydBrakeCylinder;
                DbWinch.ElCableNetwork = updCheck.ElCableNetwork;
                DbWinch.ElRadio = updCheck.ElRadio;
                DbWinch.ElButtonBox = updCheck.ElButtonBox;
                DbWinch.TensionCheckBar = updCheck.TensionCheckBar;
                DbWinch.TestWinch = updCheck.TestWinch;
                DbWinch.TestTraction = updCheck.TestTraction;
                DbWinch.TestBrakes = updCheck.TestBrakes;

                await _repository.UpdateChecklistAsync(DbWinch);
            }

            DbOrder.WorkHours = form.WorkHours;
            DbOrder.Status = form.OrderStatus;
            DbOrder.OrderCompleted = form.OrderCompleted;


            await _repository.UpdateOrderAsync(DbOrder);

            return true;
        }
  */
