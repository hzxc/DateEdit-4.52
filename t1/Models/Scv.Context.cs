﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace t1.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SCVEntities : DbContext
    {
        public SCVEntities()
            : base("name=SCVEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ATTRIBUTE> ATTRIBUTE { get; set; }
        public virtual DbSet<ITEM_BEIYONG> ITEM_BEIYONG { get; set; }
        public virtual DbSet<LOCATION_INVENTORY> LOCATION_INVENTORY { get; set; }
    }
}
