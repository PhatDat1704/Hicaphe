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
    
    public partial class TAIKHOANKHACHHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAIKHOANKHACHHANG()
        {
            this.DONDATHANGs = new HashSet<DONDATHANG>();
        }
    
        public int MaTK { get; set; }
        public string HoTenKH { get; set; }
        public string Email { get; set; }
        public string DiachiKH { get; set; }
        public string SDT { get; set; }
        public string Matkhau { get; set; }
        public Nullable<System.DateTime> Ngaysinh { get; set; }
        public Nullable<bool> Daduyet { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONDATHANG> DONDATHANGs { get; set; }
    }
}
