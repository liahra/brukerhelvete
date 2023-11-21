using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noested.Models
{
    public enum OrderStatus
    {
        Received,
        InProgress,
        Completed,
        Billed
    }
    public enum WarrantyType
    {
        None,
        Limited,
        Full
    }
    public enum ProductType
    {
        Winch,
        TimberTrailer,
        TractorShears,
        LiftEquip,
        WoodEquip,
        SnowBloPlo,
        SandBlaster
    }

    public class ServiceOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Ordrenr")]
        public int OrderId { get; set; }

        [ForeignKey("Customer")]
        [Display(Name = "Kundenr")]
        public int CustomerId { get; set; }

        public virtual Customer? Customer { get; set; } // nav
        public virtual Checklist? Checklist { get; set; } // nav
        
        [Required]
        [Display(Name = "Ordrestatus")]
        public OrderStatus Status { get; set; }

        [Display(Name = "Godkjent av")]
        [MaxLength(50)]
        public string? ApprovedBy { get; set; }

        [Required]
        [Display(Name = "Aktiv")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Garanti")]
        public WarrantyType Warranty { get; set; }

        [Required]
        [Display(Name = "Mottatt")]
        public DateTime OrderReceived { get; set; }

        [Display(Name = "Fullført")]
        public DateTime? OrderCompleted { get; set; }

        [Display(Name = "Agreed Finished Date")]
        public DateTime? AgreedFinishedDate { get; set; }

        [Display(Name = "Produktnavn")]
        public string? ProductName { get; set; }

        [Required]
        [Display(Name = "Produkttype")]
        public ProductType Product { get; set; }

        [MaxLength(4)]
        [Display(Name = "Årsmodell")]
        public string? ModelYear { get; set; }

        [MaxLength(50)]
        [Display(Name = "Serienummer")]
        public string? SerialNumber { get; set; }

        [MaxLength(200)]
        [Display(Name = "Kundeavtale")]
        public string? CustomerAgreement { get; set; }

        [MaxLength(200)]
        [Display(Name = "Beskrivelse")]
        public string? OrderDescription { get; set; }

        [MaxLength(200)]
        [Display(Name = "Avlagte deler")]
        public string? DiscardedParts { get; set; }

        [MaxLength(200)]
        [Display(Name = "Deleretur")]
        public string? ReplacedPartsReturned { get; set; }

        [MaxLength(50)]
        [Display(Name = "Frakt")]
        public string? Shipping { get; set; }

        [Display(Name = "Working Hours")]
        public int WorkHours { get; set; } = 0;

        
    }
}

