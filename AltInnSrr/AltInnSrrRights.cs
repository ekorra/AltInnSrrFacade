using System;
using Newtonsoft.Json;

namespace AltInnSrr
{
    public class AltInnSrrRights
    {
        [JsonIgnore]
        public int OrgNr { get; set; }
        public DateTime ReadRightValidTo { get; set; } = DateTime.MinValue;
        public DateTime WriteRightValidTo { get; set; } = DateTime.MinValue;

        public  bool HasMoveRights
        {
            get { return WriteRightValidTo > DateTime.Now && ReadRightValidTo > DateTime.Now; }
        }
    }
}
