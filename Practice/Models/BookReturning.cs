//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BookReturning
    {
        [Key, ForeignKey("BookGiving")]
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Дата принятия книги")]
        public System.DateTime Real_Return_Date { get; set; }
    
        public virtual Penalty Penalty { get; set; }
        [Required]
        public virtual BookGiving BookGiving { get; set; }
        public virtual Librarian Librarian { get; set; }
    }
}
