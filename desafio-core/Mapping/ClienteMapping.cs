using desafio_core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Mapping
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Nome).HasColumnType("varchar(100)").IsRequired();

            // Relacionamentos
            builder.HasMany(f => f.Dividas).WithOne(p => p.Cliente).HasForeignKey(p => p.ClienteId);
        }
    }
}
