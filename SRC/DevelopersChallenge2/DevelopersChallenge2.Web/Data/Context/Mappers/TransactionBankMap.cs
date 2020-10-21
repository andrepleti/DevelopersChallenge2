using DevelopersChallenge2.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace DevelopersChallenge2.Web.Data.Context.Mappers
{
    public class TransactionBankMap : EntityTypeConfiguration<TransactionBank>
    {
        public TransactionBankMap()
        {
            ToTable("Transaction", "dbo");

            HasKey(x => x.Id);

            Property(x => x.TypeOperation).IsRequired();
            Property(x => x.Description).IsRequired().HasMaxLength(300);
            Property(x => x.DateOperation).IsRequired();
            Property(x => x.Value).IsRequired();
            Property(x => x.Reconciliation).IsRequired();
        }
    }
}