//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DigitalSignage.Data.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Display
    {
        public int DisplayId { get; set; }
        public string DisplayName { get; set; }
        public string Location { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> AccountID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
