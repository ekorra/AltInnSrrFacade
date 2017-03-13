using System;

namespace MoveAdmin.Commons
{
    public class AltInnSrrRights
    {
        public DateTime ReadRightValidTo { get; set; }
        public DateTime WriteRightValidTo { get; set; }

        public bool HasMoveRights()
        {
            return WriteRightValidTo > DateTime.Now && ReadRightValidTo > DateTime.Now;
        }
    }
}
