namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sales.Store")]
    public partial class Store : Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Store()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Display (Name = "Sales Person")]
        public int? SalesPersonID { get; set; }

        [Column(TypeName = "xml")]
        public string Demographics { get; set; }

        public Guid? rowguid { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual BusinessEntity BusinessEntity { get; set; }
        public virtual BusinessEntityAddress BusinessEntityAddress { get; set; }
        public virtual IEnumerable<StoreContacts> StoreReps { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual StoreDemographics StoreDemographics { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Customer> Customers { get; set; }
        public virtual SalesPerson SalesPerson { get; set; }
    }
    
    public partial class StoreContacts : Person
    {
        [Required]
        public virtual BusinessEntityContact BusinessEntityContact { get; set; }
    }

    public partial class StoreDemographics
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "money")]
        public decimal AnnualSales { get; set; }

        [Column(TypeName = "money")]
        public decimal AnnualRevenue { get; set; }
        [Display(Name = "Bank")]
        public string BankName { get; set; }
        [Display(Name = "Business")]
        public string BusinessType { get; set; }
        [Display(Name = "EST.")]
        public int YearOpened { get; set; }
        public string Speciality { get; set; }
        public int SquareFeet { get; set; }
        public string Brands { get; set; }
        public string Internet { get; set; }
        public int NumberEmployees { get; set; }

        [Required]
        public virtual Store Store { get; set; }
    }

    
}

