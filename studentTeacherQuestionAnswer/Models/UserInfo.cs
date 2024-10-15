using System;
using System.Collections.Generic;

namespace studentTeacherQuestionAnswer.Models;

public partial class UserInfo
{
    public int UserId { get; set; }

    public string? EmailId { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public int? IdcardNo { get; set; }

    public string? InstituteName { get; set; }

    public string? UserType { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
}
