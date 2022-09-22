using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class VMMember
    {
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

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請再填寫一次密碼")]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "密碼最少12碼")]
        [MaxLength(20, ErrorMessage = "密碼最多20碼")]
        [Compare("UserPassword", ErrorMessage = "兩次輸入不同")]
        public string ConfirmPassword { get; set; }
    }
}