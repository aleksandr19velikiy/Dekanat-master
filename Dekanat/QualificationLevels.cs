
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Dekanat
{

using System;
    using System.Collections.Generic;
    
public partial class QualificationLevels
{

    public QualificationLevels()
    {

        this.Groups = new HashSet<Groups>();

        this.SubjectInfo = new HashSet<SubjectInfo>();

    }


    public int QualificationLevelId { get; set; }

    public string QualificationLevelName { get; set; }



    public virtual ICollection<Groups> Groups { get; set; }

    public virtual ICollection<SubjectInfo> SubjectInfo { get; set; }

}

}
