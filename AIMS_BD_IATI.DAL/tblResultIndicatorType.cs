//------------------------------------------------------------------------------
// 
//------------------------------------------------------------------------------

namespace AIMS_BD_IATI.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblResultIndicatorType
    {
        public tblResultIndicatorType()
        {
            this.tblProjectResults = new HashSet<tblProjectResult>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string IATICode { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string IUser { get; set; }
        public string EUser { get; set; }
        public System.DateTime IDate { get; set; }
        public Nullable<System.DateTime> EDate { get; set; }
        public string NameLocal { get; set; }
        public string Remarks { get; set; }
    
        public virtual ICollection<tblProjectResult> tblProjectResults { get; set; }
    }
}
