//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class OrderDetails
    {
        [Key]
        [DisplayName("訂單編號")]
        [StringLength(15)]
        public string OrderID { get; set; }

        [DisplayName("商品編號")]
        [StringLength(20)]
        [RegularExpression("[A-F][0-9]{19}")]
        public string ProductID { get; set; }

        [DisplayName("數量")]
        [Required(ErrorMessage = "請輸入商品數量")]
        [Range(0, short.MaxValue, ErrorMessage = "數量不可小於0")]
        public short Quantity { get; set; }

        [DisplayName("商品售價")]
        [Required(ErrorMessage = "請輸入商品售價")]
        [Range(0, int.MaxValue, ErrorMessage = "售價不可小於0")]
        public decimal SellingPrice { get; set; }

        [DisplayName("小計")]
        public Nullable<decimal> Subtotal { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}
