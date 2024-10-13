using System;
using System.Collections.Generic;

namespace studentTeacherQuestionAnswer.Models;

public partial class Question
{
    public int Qid { get; set; }

    public string? Question1 { get; set; }

    public DateOnly? Qdate { get; set; }

    public int? Qby { get; set; }

    public virtual UserInfo? QbyNavigation { get; set; }
}
