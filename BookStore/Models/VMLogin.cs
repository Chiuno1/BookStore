using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class VMLogin
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請填寫帳號")]
        [StringLength(40, ErrorMessage = "帳號不得超過40字")]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請填寫密碼")]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "密碼最少12碼")]
        [MaxLength(20, ErrorMessage = "密碼最多20碼")]
        [RegularExpression("[A-Za-z0-9]{12,20}", ErrorMessage = "密碼格式錯誤")]
        public string Password { get; set; }
    }
}