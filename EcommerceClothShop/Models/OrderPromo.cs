//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EcommerceClothShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderPromo
    {
        public int OrderPromoID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> PromoID { get; set; }
    
        public virtual DiscountPromo DiscountPromo { get; set; }
        public virtual Order Order { get; set; }
    }
}
