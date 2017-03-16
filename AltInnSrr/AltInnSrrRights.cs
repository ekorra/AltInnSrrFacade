using System;

namespace AltInnSrr
{
    public class AltInnSrrRights
    {
        public int OrgNr { get; set; }
        public DateTime ReadRightValidTo { get; set; }
        public DateTime WriteRightValidTo { get; set; }

        public bool HasMoveRights()
        {
            return WriteRightValidTo > DateTime.Now && ReadRightValidTo > DateTime.Now;
        }
    }
}
