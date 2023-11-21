using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noested.Models
{
    [Table("WinchChecklist")] // Specify the table name for the subclass
    public class WinchChecklist : Checklist
	{
        [Required]
        [Display(Name = "Bremser")]
        public ComponentStatus MechBrakes { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Trommellager")]
        public ComponentStatus MechDrumBearing { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "PTO & opplagring")]
        public ComponentStatus MechStoragePTO { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Wire")]
        public ComponentStatus MechWire { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Kjedestrammer")]
        public ComponentStatus MechChainTensioner { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Pinionlager")]
        public ComponentStatus MechPinionBearing { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Clutch")]
        public ComponentStatus MechClutch { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Kjedehjulkile")]
        public ComponentStatus MechSprocketWedges { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Hydraulikksylinder")]
        public ComponentStatus HydCylinder { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Hydraulikkblokk")]
        public ComponentStatus HydHydraulicBlock { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Tankolje")]
        public ComponentStatus HydTankOil { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Girboksolje")]
        public ComponentStatus HydGearboxOil { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Bremsesylinder")]
        public ComponentStatus HydBrakeCylinder { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Ledningsnett")]
        public ComponentStatus ElCableNetwork { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Radio")]
        public ComponentStatus ElRadio { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Knappekasse")]
        public ComponentStatus ElButtonBox { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Bar")]
        public ComponentStatus TensionCheckBar { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Vinsjtest")]
        public ComponentStatus TestWinch { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Trekkraft")]
        public ComponentStatus TestTraction { get; set; } = ComponentStatus.Unchecked;

        [Required]
        [Display(Name = "Bremsekraft")]
        public ComponentStatus TestBrakes { get; set; } = ComponentStatus.Unchecked;

    }
}

