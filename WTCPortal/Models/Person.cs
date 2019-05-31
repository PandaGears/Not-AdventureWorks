namespace WTCPortal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Person.Person")]
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int BusinessEntityID { get; set; }

        [StringLength(2)]
        public string PersonType { get; set; }

        public bool NameStyle { get; set; }

        [StringLength(8)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [StringLength(10)]
        public string Suffix { get; set; }

        [Display(Name = "Email notifications")]
        public int EmailPromotion { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual EmailAddress EmailAddress { get; set; }
        public virtual PersonPhone PersonPhone { get; set; }
        public virtual Password Password { get; set; }

        public IEnumerable<SelectListItem> TitleList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public IEnumerable<SelectListItem> MaritalList { get; set; }
        public Dictionary<int, string> Emailnotifications { get; set; }
        public Dictionary<int, string> AddressIding { get; set; }
        public Dictionary<int, string> Phonetype { get; set; }
        public Dictionary<int, string> GetDepartments { get; set; }
        public Dictionary<int, string> GetShift { get; set; }
        public Dictionary<int, string> GetSalesRep { get; set; }
        public Dictionary<int, string> GetState { get; set; }
        public Dictionary<int, string> CredRate { get; set; }
        public Dictionary<int, string> GetCards { get; set; }
        public Dictionary<int, string> Territorys { get; set; }
    }
}
