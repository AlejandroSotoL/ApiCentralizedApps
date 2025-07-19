using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Application.DTOS
{
    public class ValidationResponseDto
    {
        public int CodeStatus { get; set; }
        public bool BooleanStatus { get; set; } = false;
        public string SentencesError { get; set; } = null;
    }
}