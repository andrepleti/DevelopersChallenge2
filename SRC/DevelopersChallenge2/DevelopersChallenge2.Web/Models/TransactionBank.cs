using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevelopersChallenge2.Web.Models
{
    public class TransactionBank
    {
        public int Id { get; set; }
        [Display(Name = "Type")]
        public byte TypeOperation { get; set; }
        public string Description { get; set; }
        [Display(Name = "Date")]
        public DateTime DateOperation { get; set; }
        public decimal Value { get; set; }
        public bool Reconciliation { get; set; }
    }
}