namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Purchasing.ProductVendor")]
    public partial class ProductVendor : ProductSubcategory
    {

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        public int AverageLeadTime { get; set; }

        [Column(TypeName = "money")]
        public decimal StandardPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? LastReceiptCost { get; set; }

        public DateTime? LastReceiptDate { get; set; }

        public int MinOrderQty { get; set; }

        public int MaxOrderQty { get; set; }

        public int? OnOrderQty { get; set; }

        [Required]
        [StringLength(3)]
        public string UnitMeasureCode { get; set; }

        public virtual UnitMeasure UnitMeasure { get; set; }

        public virtual Vendor Vendor { get; set; }
    }
}
