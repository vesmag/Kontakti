//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContactsApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tag
    {
        public Tag() { }
        public Tag (int id, string tag)
        {
            PersonId = id;
            Tag1 = tag;
        }
        public int EntryId { get; set; }
        public int PersonId { get; set; }
        public string Tag1 { get; set; }
    }
}