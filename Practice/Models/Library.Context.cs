﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Practice.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LibraryContainer1 : DbContext
    {
        public LibraryContainer1()
            : base("name=LibraryContainer1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> AddressSet { get; set; }
        public virtual DbSet<Person> PersonSet { get; set; }
        public virtual DbSet<Publication> PublicationSet { get; set; }
        public virtual DbSet<Publisher> PublisherSet { get; set; }
        public virtual DbSet<Penalty> PenaltySet { get; set; }
        public virtual DbSet<BookGiving> BookGivingSet { get; set; }
        public virtual DbSet<BookReturning> BookReturningSet { get; set; }
    }
}
