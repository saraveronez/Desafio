using desafio_core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Mapping
{
    public class ParcelaDividaMapping : IEntityTypeConfiguration<ParcelaDivida>
    {
        public void Configure(EntityTypeBuilder<ParcelaDivida> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.ValorOriginal).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(p => p.ValorComJuros).HasColumnType("decimal(10,2)").IsRequired();
        }
    }
}
