//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace hicaphe2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DONDATHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONDATHANG()
        {
            this.CTDATHANGs = new HashSet<CTDATHANG>();
        }
    
        public int SODH { get; set; }
        public Nullable<int> MaTK { get; set; }
        public Nullable<System.DateTime> NgayDH { get; set; }
        public Nullable<bool> Dagiao { get; set; }
        public Nullable<bool> Dahuy { get; set; }
        public Nullable<System.DateTime> Ngaygiaohang { get; set; }
        public string Tennguoinhan { get; set; }
        public string Diachinhan { get; set; }
        public Nullable<decimal> Trigia { get; set; }
        public string Dienthoainhan { get; set; }
        public Nullable<bool> HTThanhtoan { get; set; }
        public Nullable<bool> HTGiaohang { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDATHANG> CTDATHANGs { get; set; }
        public virtual TAIKHOANKHACHHANG TAIKHOANKHACHHANG { get; set; }
    }
}
