
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace DekanatForTeacher
{

using System;
    using System.Collections.Generic;
    
public partial class AssociatedSubjects
{

    public int AssociatedSubjectId { get; set; }

    public int Teacher { get; set; }

    public int SubjectInfo { get; set; }

    public int StudyGroup { get; set; }



    public virtual Groups Groups { get; set; }

    public virtual SubjectInfo SubjectInfo1 { get; set; }

    public virtual Teachers Teachers { get; set; }

}

}
