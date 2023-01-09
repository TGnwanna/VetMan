using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
	public interface IVaccineHelper
	{
		bool CreateProductVaccine(ProductVaccineViewModel ProductVaccineDetails);
		List<ProductVaccineViewModel> GetProductVaccineList(int id);
		bool DeleteProductVaccine(ProductVaccineViewModel ProductVaccineDetails);
		bool EditProductVaccine(ProductVaccineViewModel ProductVaccineDetails);
	}
}
