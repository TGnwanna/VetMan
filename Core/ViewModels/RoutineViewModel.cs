using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.ViewModels
{
	public class RoutineViewModel
	{
		public Guid Id { get; set; }
		public int? PatientId { get; set; }
		public DateTime CurrentDate { get; set; }
		public DateTime NextDate { get; set; }
		public string? Purpose { get; set; }
		public bool Active { get; set; }
		public bool SMSAllowed { get; set; }
		public bool EmailAllowed { get; set; }
		public Guid? CompanyBranchId { get; set; }
	}
}
