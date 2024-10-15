using System;
using System.Collections.Generic;

namespace studentTeacherQuestionAnswer.Models;

public partial class Reply
{
    public int Rid { get; set; }

    public string? Reply1 { get; set; }

    public DateOnly? Rdate { get; set; }

    public int? Rby { get; set; }

    public int? Rfor { get; set; }

    public virtual UserInfo? RbyNavigation { get; set; }

    public virtual Answer? RforNavigation { get; set; }
}
