using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Treatments
{
    public class EditPatientViewModel
    {
        public virtual Patient? Patient { get; set; }
        public virtual List<Breed>? Breed { get; set; }
    }
}
