using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Core.Models
{
    public class Breed : BaseModel
    {
        public Species SpeciesId { get; set; }
    }
}
