namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sales.vEmployeeHistory")]
    public partial class vEmployeeHistory
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(1)]
        public string Gender { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime HireDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string JobTitle { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime StartDate { get; set; }
    }
}
