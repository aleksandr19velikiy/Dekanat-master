
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
    
public partial class AcademicRanks
{

    public AcademicRanks()
    {

        this.Teachers = new HashSet<Teachers>();

    }


    public int AcademicRankId { get; set; }

    public string AcademicRankName { get; set; }



    public virtual ICollection<Teachers> Teachers { get; set; }

}

}