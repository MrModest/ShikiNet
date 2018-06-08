using System;
using System.Collections.Generic;
using System.Text;
using ShikiNet.Entities.Enums;

namespace ShikiNet.Entities
{
    interface ITitle
    {
        int Id { get; }

        string Name { get; }

        string Russian { get; }

        Image Image { get; }

        string Url { get; }

        TitleStatus Status { get; }

        DateTime? AiredOn { get; }

        DateTime? ReleasedOn { get; }
    }
}
