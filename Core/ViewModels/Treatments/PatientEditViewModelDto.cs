using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;

namespace Core.ViewModels.Treatments
{
	public class PatientEditViewModelDto
	{
		public DateTime DOB { get; set; }
		public string? BirthDay { get; set; }
		public Species SpecieId { get; set; }
		public int GenderId { get; set; }
		public int Id { get; set; }
		public string? Name { get; set; }
		public int? BreedId { get; set; }
		public string? Colour { get; set; }
		public string? SpecialMarking { get; set; }
		public string? Picture { get; set; }
		public string? CaseNumber { get; set; }
	}
}
