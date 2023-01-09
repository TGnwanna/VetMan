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
	public class VisitTreatmentViewModel
	{
		public Guid Id { get; set; }
		public double Discount { get; set; }
		public double Cost { get; set; }
		public int TreatmentId { get; set; }

		public int VisitId { get; set; }
		public string? CreatedById { get; set; }
		public Guid CompanyBranchId { get; set; }
	}
}
