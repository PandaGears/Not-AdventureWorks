namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HumanResources.Employee")]
    public partial class Employee  : Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            PurchaseOrderHeaders = new HashSet<PurchaseOrderHeader>();
        }
        [Required]
        [StringLength(15)]
        [Display(Name = "I.D Number")]
        public string NationalIDNumber { get; set; }

        [StringLength(256)]
        public string LoginID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public short? OrganizationLevel { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Position")]
        public string JobTitle { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }


        [Required]
        [StringLength(1)]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Hire Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Union member")]
        public bool SalariedFlag { get; set; }
        [Display(Name = "Vacation Hours")]
        public short VacationHours { get; set; }

        [Display(Name = "Sick Leave Hours")]
        public short SickLeaveHours { get; set; }

        public bool CurrentFlag { get; set; }
       
        public virtual BusinessEntityAddress BusinessEntityAddress { get; set; }
        public virtual EmployeeDepartmentHistory EmployeeDepartmentHistory { get; set; }
        public virtual EmployeePayHistory EmployeePayHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }

        public virtual SalesPerson SalesPerson { get; set; }
    }
}
