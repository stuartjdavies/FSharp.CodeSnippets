//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FSharp.CodeSnippets.Data.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Group
    {
        public Group()
        {
            this.Students = new HashSet<Student>();
            this.Subj_Teach = new HashSet<Subj_Teach>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Subj_Teach> Subj_Teach { get; set; }
    }
}
