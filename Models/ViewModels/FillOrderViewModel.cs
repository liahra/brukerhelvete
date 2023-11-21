using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Noested.Models
{
    public class FillOrderViewModel
	{
        public WinchChecklist? UpdWinchlist { get; set; }

        public int OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderCompleted { get; set; }
        public string? MechSignature { get; set; }
        public string? RepairComment { get; set; }
        public int WorkHours { get; set; }

        // READONLY DISPLAY IN VIEW
        public int ServiceOrderId { get; set; }
        public int CustomerId { get; set; }
        public string? ProductName { get; set; }
        public ProductType ProductT { get; set; }
        public DateTime OrderReceived { get; set; }
        public string? OrderDescription { get; set; }
    }
}

