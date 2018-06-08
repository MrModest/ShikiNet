using System;
using System.Collections.Generic;
using System.Text;

namespace ShikiNet.Entities.Enums
{
    //ToDo: refactor to PascalCase
    public enum UserRateStatus // TitleListStatus ; TitleProgressStatus
    {
        PLANNED,
        WATCHING,
        COMPLETED,
        ON_HOLD,
        DROPPED,
        REWATCHING = 9 // 'rewatching' without '_'
    }
}
