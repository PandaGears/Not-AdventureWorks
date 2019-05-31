using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WTCPortal.Models
{
    public class IndividualCustomer : Person
    {
        [Required]
        public virtual Customer Customer { get; set; }
        public virtual BusinessEntityAddress BusinessEntityAddress { get; set; }
        [Required]
        public virtual PersonCreditCard PersonCreditCard { get; set; }

    }
}