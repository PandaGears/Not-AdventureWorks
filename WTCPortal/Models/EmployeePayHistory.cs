namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HumanResources.EmployeePayHistory")]
    public partial class EmployeePayHistory
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime RateChangeDate { get; set; }

        [Column(TypeName = "money")]
        [Display (Name = "Salary")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Please enter 1 or 2")]
        [Display(Name = "Pay Frequency")]
        public byte PayFrequency { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
