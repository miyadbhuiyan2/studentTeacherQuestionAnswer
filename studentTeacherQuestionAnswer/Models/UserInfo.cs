using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace studentTeacherQuestionAnswer.Models;

public partial class UserInfo
{
    public int UserId { get; set; }
    public string? EmailId { get; set; }
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public string? Name { get; set; }
    
    public int? IdcardNo { get; set; }
   
    public string? InstituteName { get; set; }
    
    public string? UserType { get; set; }
}
