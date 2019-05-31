namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vPerson")]
    public partial class vPerson
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        [Column(Order = 1)]
        [StringLength(50)]
        public string  FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Column(Order = 2)]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(10)]
        public string Suffix { get; set; }

        [Column(Order = 3)]
        [StringLength(2)]
        public string PersonType { get; set; }

        [Column(Order = 4)]
        public bool NameStyle { get; set; }

        [StringLength(8)]
        public string Title { get; set; }


        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmailPromotion { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        [Column(Order = 6)]
        [StringLength(25)]
        public string PhoneNumber { get; set; }


        [Column(Order = 7)]
        [StringLength(50)]
        public string PhoneType { get; set; }
    }

}
