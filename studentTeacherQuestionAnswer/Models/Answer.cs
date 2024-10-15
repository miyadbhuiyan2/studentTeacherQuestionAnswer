using System;
using System.Collections.Generic;

namespace studentTeacherQuestionAnswer.Models;

public partial class Answer
{
    public int Aid { get; set; }

    public string? Answer1 { get; set; }

    public DateOnly? Adate { get; set; }

    public int? Aby { get; set; }

    public int? Afor { get; set; }

    public virtual UserInfo? AbyNavigation { get; set; }

    public virtual Question? AforNavigation { get; set; }

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
}
