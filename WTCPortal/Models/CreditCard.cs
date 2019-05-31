namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sales.CreditCard")]
    public partial class CreditCard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CreditCard()
        {
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
        }
        [Key]
        public int CreditCardID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Type")]
        public string CardType { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Card number")]
        [RegularExpression(@"^\(?([0-9]{10})\)?", ErrorMessage = "Only Digits Allowed") ]
        public string CardNumber { get; set; }

        [Display(Name = "Expiration Month")]
        public byte ExpMonth { get; set; }

        [Display(Name = "Expiration Year")]
        public short ExpYear { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        public virtual PersonCreditCard PersonCreditCard { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
