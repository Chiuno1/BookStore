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
    using System.Linq;

    public partial class Members
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Members()
        {
            this.Comments = new HashSet<Comments>();
            this.Orders = new HashSet<Orders>();
        }
    
        public long MemberID { get; set; }

        [Key]
        [DisplayName("會員帳號")]
        [StringLength(40)]
        public string MemberAccount { get; set; }

        [DisplayName("會員姓名")]
        [StringLength(20)]
        [Required]
        public string MemberName { get; set; }

        [DisplayName("地址")]
        [StringLength(40)]
        [Required]
        public string Address { get; set; }

        [DisplayName("電話")]
        [StringLength(24)]
        [Required]
        public string Phone { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請填寫密碼")]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "密碼最少12碼")]
        [MaxLength(20, ErrorMessage = "密碼最多20碼")]
        public string UserPassword { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comments> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }

        //自訂驗證規則寫法
        public class CheckAccount : ValidationAttribute
        {
            public CheckAccount()
            {
                ErrorMessage = "此帳號有人使用";
            }
            public override bool IsValid(object value)
            {
                BookStoreEntities db = new BookStoreEntities();
                var account = db.Members.Where(m => m.MemberAccount == value.ToString()).FirstOrDefault();
                return (account == null) ? true : false;
            }
        }

    }
}