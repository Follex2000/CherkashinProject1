
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace CherkashinProject.Entity
{

using System;
    using System.Collections.Generic;
    
public partial class GetTovara
{

    public int GetId { get; set; }

    public int TovarId { get; set; }

    public int SkladId { get; set; }

    public int Count { get; set; }

    public double Price { get; set; }

    public int UserId { get; set; }

    public System.DateTime DateOfGet { get; set; }



    public virtual Sklad Sklad { get; set; }

    public virtual Tovares Tovares { get; set; }

    public virtual Users Users { get; set; }

}

}
