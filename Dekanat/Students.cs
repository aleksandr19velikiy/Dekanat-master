
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
    
public partial class Students
{

    public Students()
    {

        this.Groups = new HashSet<Groups>();

        this.Marks = new HashSet<Marks>();

    }


    public int StudentId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MiddleName { get; set; }

    public System.DateTime BirthDate { get; set; }

    public string Address { get; set; }

    public string MobilePhone { get; set; }

    public int StudyGroup { get; set; }



    public virtual ICollection<Groups> Groups { get; set; }

    public virtual Groups Groups1 { get; set; }

    public virtual ICollection<Marks> Marks { get; set; }

}

}
