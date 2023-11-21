using Noested.Data;
using Noested.Models;

public static class DatabaseSeeder
{
    public static async Task SeedServiceOrders(IServiceOrderRepository dbContext)
    {
        List<ServiceOrder> serviceOrders = new()
            {
                new ServiceOrder
                {
                    IsActive = true,
                    OrderReceived = DateTime.Now,
                    OrderCompleted = null,
                    Status = OrderStatus.Received,
                    AgreedFinishedDate = DateTime.Now.AddDays(7),
                    ProductName = "IGLAND 5002 Pento TL",
                    Product = ProductType.Winch,
                    ModelYear = "2022",
                    SerialNumber = "12345",
                    Warranty = WarrantyType.Full,
                    CustomerAgreement = "Standard Agreement",
                    OrderDescription = "Problem with motor",
                    DiscardedParts = "",
                    ReplacedPartsReturned = "",
                    Shipping = "",
                    WorkHours = 0,
                    Customer = new Customer
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Street = "Univeien 20",
                        PostalCode = "5034",
                        City = "Cityville",
                        Email = "john.doe@gmail.com",
                        Phone = "85429854"
                    },
                    Checklist = new WinchChecklist
                    {
                        ProductType = ProductType.Winch,
                        ServiceProcedure = "Standard",
                        PreparedBy = "DAN",
                    }
                },
                new ServiceOrder
                {
                    IsActive = true,
                    OrderReceived = DateTime.Now,
                    OrderCompleted = null,
                    Status = OrderStatus.Received,
                    AgreedFinishedDate = DateTime.Now.AddDays(7),
                    ProductName = "IGLAND 6002 Pento TL",
                    Product = ProductType.Winch,
                    ModelYear = "2021",
                    SerialNumber = "67890",
                    Warranty = WarrantyType.Limited,
                    CustomerAgreement = "Premium Agreement",
                    OrderDescription = "Problem with cable",
                    DiscardedParts = "",
                    ReplacedPartsReturned = "",
                    Shipping = "",
                    WorkHours = 0,
                    Customer = new Customer
                    {
                        FirstName = "Jane",
                        LastName = "Doe",
                        Street = "Univeien 21",
                        PostalCode = "5035",
                        City = "Cityville",
                        Email = "jane.doe@gmail.com",
                        Phone = "85429855"
                    },
                    Checklist = new WinchChecklist
                    {
                        ProductType = ProductType.Winch,
                        ServiceProcedure = "Special",
                        PreparedBy = "MIM",
                    }
                },
                new ServiceOrder
                {
                    IsActive = true,
                    OrderReceived = DateTime.Now,
                    OrderCompleted = null,
                    Status = OrderStatus.Received,
                    AgreedFinishedDate = DateTime.Now.AddDays(7),
                    ProductName = "IGLAND 7002 Pento TL",
                    Product = ProductType.Winch,
                    ModelYear = "2020",
                    SerialNumber = "11111",
                    Warranty = WarrantyType.None,
                    CustomerAgreement = "Basic Agreement",
                    OrderDescription = "Problem with remote",
                    DiscardedParts = "",
                    ReplacedPartsReturned = "",
                    Shipping = "",
                    WorkHours = 0,
                    Customer = new Customer
                    {
                        FirstName = "Emily",
                        LastName = "Smith",
                        Street = "Univeien 22",
                        PostalCode = "5036",
                        City = "Cityville",
                        Email = "emily.smith@gmail.com",
                        Phone = "85429856"
                    },
                    Checklist = new WinchChecklist
                    {
                        ProductType = ProductType.Winch,
                        ServiceProcedure = "Grand Overhaul",
                        PreparedBy = "KRISS",
                    }
                }
            };

        foreach (ServiceOrder order in serviceOrders)
        {
            IEnumerable<ServiceOrder> allServiceOrders = await dbContext.GetAllServiceOrdersAsync();
            IEnumerable<Customer> allCustomers = await dbContext.GetAllCustomersAsync();
            

            // Ny eller eksisterende serviceordre
            var duplicateOrder = allServiceOrders.FirstOrDefault(o =>
                o.ProductName == order.ProductName &&
                o.SerialNumber == order.SerialNumber &&
                o.OrderReceived.Date == order.OrderReceived.Date // Samme dag
            );

            if (duplicateOrder != null)
            {
                Console.WriteLine("Duplicate ServiceOrder found. Skipping this order.");
                continue;
            }

            // Ny eller eksisterende kunde 
            if (order.CustomerId != 0)
            {
                await dbContext.AddCustomerAsync(order.Customer!);
                order.CustomerId = order.Customer!.CustomerId;
            }
            else
            {
                Customer? existingCustomer = allCustomers.FirstOrDefault(c => c.Email == order.Customer!.Email);
                if (existingCustomer != null)
                {
                    order.CustomerId = existingCustomer.CustomerId;
                }
                else
                {
                    await dbContext.AddCustomerAsync(order.Customer!);
                    order.CustomerId = order.Customer!.CustomerId;
                }

                await dbContext.AddServiceOrderAsync(order);

                if (order.Checklist != null)
                {
                    order.Checklist.ChecklistId = order.OrderId;
                }
            }
        }
    }
}

