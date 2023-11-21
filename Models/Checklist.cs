using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public enum ComponentStatus // Used in subclasses
    {
        Unchecked,
        OK,
        NeedsReplacement,
        Defective
    }

    public class Checklist
    {
        [Key]
        [ForeignKey("ServiceOrder")]
        public int ChecklistId { get; set; }

        public virtual ServiceOrder? ServiceOrder { get; set; }  // nav

        [Required]
        [Display(Name = "Produkttype")]
        public ProductType ProductType { get; set; }

        [Display(Name = "Utført av")]
        [MaxLength(50)]
        public string? PreparedBy { get; set; } = null;

        [Display(Name = "Prosedyre")]
        [MaxLength(200)]
        public string? ServiceProcedure { get; set; } = null;

        [Display(Name = "Signatur")]
        [MaxLength(50)]
        public string? MechSignature { get; set; } = "N/A";

        [Display(Name = "Kommentar")]
        [MaxLength(200)]
        public string? RepairComment { get; set; } = "N/A";

        [Display(Name = "Dato fullført")]
        public DateTime? DateCompleted { get; set; } = null;


    }
}
