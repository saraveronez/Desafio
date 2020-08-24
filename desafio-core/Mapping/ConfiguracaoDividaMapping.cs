using desafio_core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Mapping
{
    public class ConfiguracaoDividaMapping : IEntityTypeConfiguration<ConfiguracaoDivida>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoDivida> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.PorcentagemPaschoalotto).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(p => p.Juros).HasColumnType("decimal(5,2)").IsRequired();

            // Relacionamentos
            builder.HasMany(f => f.Dividas).WithOne(p => p.ConfiguracaoDivida).HasForeignKey(p => p.ConfiguracaoDividaId);
        }
    }
}
