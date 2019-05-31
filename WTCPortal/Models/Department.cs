namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HumanResources.Department")]
    public partial class Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
        }

        public short DepartmentID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Department")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupName { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual EmployeeDepartmentHistory EmployeeDepartmentHistory { get; set; }
    }
}
