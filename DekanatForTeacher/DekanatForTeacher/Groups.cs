
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
    
public partial class Groups
{

    public Groups()
    {

        this.AssociatedSubjects = new HashSet<AssociatedSubjects>();

        this.Students1 = new HashSet<Students>();

    }


    public int GroupId { get; set; }

    public int GroupNum { get; set; }

    public int GroupQuantity { get; set; }

    public Nullable<int> Elder { get; set; }

    public int Course { get; set; }

    public int QualificationLevel { get; set; }

    public int StudyForm { get; set; }



    public virtual ICollection<AssociatedSubjects> AssociatedSubjects { get; set; }

    public virtual Courses Courses { get; set; }

    public virtual QualificationLevels QualificationLevels { get; set; }

    public virtual Students Students { get; set; }

    public virtual StudyForms StudyForms { get; set; }

    public virtual ICollection<Students> Students1 { get; set; }

}

}
