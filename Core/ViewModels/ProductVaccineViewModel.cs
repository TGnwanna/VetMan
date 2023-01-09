using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
	public class ProductVaccineViewModel
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string? Products { get; set; }
		public string? Name { get; set; }
		public string? ProductName  { get; set; }

		public int Week { get; set; }
		public int Day { get; set; }
		public Guid? BranchId { get; set; }
	}
}
