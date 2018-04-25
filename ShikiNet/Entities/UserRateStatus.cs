using System;
using System.Collections.Generic;
using System.Text;

namespace ShikiNet.Entities
{
    public enum UserRateStatus // TitleListStatus ; TitleProgressStatus
    {
        Planned,
        Watching,
        Completed,
        OnHold,
        Dropped,
        ReWatching = 9
    }
}
