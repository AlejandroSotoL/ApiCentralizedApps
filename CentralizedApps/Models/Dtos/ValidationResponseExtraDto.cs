using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos
{
    public class ValidationResponseExtraDto
    {
        public int CodeStatus { get; set; }
        public string SentencesError { get; set; }
        public bool BooleanStatus { get; set; }
        public string ExtraData { get; set; }
    }
}