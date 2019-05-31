namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Production.ProductProductPhoto")]
    public partial class ProductProductPhoto : ProductPhoto
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        public bool Primary { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductPhoto ProductPhoto { get; set; }
    }
}
