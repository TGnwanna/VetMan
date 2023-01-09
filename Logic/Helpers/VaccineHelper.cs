using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
	public class VaccineHelper : IVaccineHelper
	{
		private readonly AppDbContext _context;
		private readonly IUserHelper _userHelper;
		public VaccineHelper(AppDbContext context, IUserHelper userHelper)
		{
			_context = context;
			_userHelper = userHelper;
		}
		public bool CreateProductVaccine(ProductVaccineViewModel productVaccineDetails)
		{
			var loggedInuser = Utitily.GetCurrentUser();
			if (loggedInuser.Id == null)
			{
				loggedInuser = _userHelper.UpdateSessionAsync(loggedInuser.UserName).Result;

			}
			if (loggedInuser != null)
            {
				var productVaccine = _context.ProductVaccines.Where(c => c.Name == productVaccineDetails.Name && c.Id == productVaccineDetails.ProductId && c.CompanyBranchId == loggedInuser.CompanyBranchId).FirstOrDefault();
				if (productVaccine == null)
				{
					var productVaccineModel = new ProductVaccine()
					{
						CompanyBranchId = loggedInuser.CompanyBranchId,
						Name = productVaccineDetails.Name,
						ProductId = productVaccineDetails.ProductId,
						Week = productVaccineDetails.Week,
						Active = true,
						Deleted = false
					};

					_context.ProductVaccines.Add(productVaccineModel);
					_context.SaveChanges();
					return true;
				}
			}
			return false;
		}

		public List<ProductVaccineViewModel> GetProductVaccineList(int productId = 0)
		{
			var productVaccineList = new List<ProductVaccineViewModel>();
			var loggedInUser = Utitily.GetCurrentUser();
			if (loggedInUser.Id == null)
			{
				loggedInUser = _userHelper.UpdateSessionAsync(loggedInUser.UserName).Result;

			}
			if (loggedInUser != null)
            {
				if (productId > 0)
				{
					productVaccineList = _context.ProductVaccines.Where(c => c.Id != null && !c.Deleted && c.ProductId == productId).Include(x => x.CompanyBranch).Include(p => p.Product)
				   .Select(x => new ProductVaccineViewModel
				   {
					   Name = x.Name,
					   ProductId = x.ProductId,
					   Id = x.Id,
					   ProductName = x.Product.Name,
					   BranchId = x.CompanyBranchId,
					   Week = x.Week
				   }).ToList();
				}
				else
				{
					productVaccineList = _context.ProductVaccines.Where(c => !c.Deleted && c.CompanyBranchId == loggedInUser.CompanyBranchId).Include(x => x.CompanyBranch).Include(p => p.Product)
					.Select(x => new ProductVaccineViewModel
					{
						Name = x.Name,
						ProductId = x.ProductId,
						Id = x.Id,
						ProductName = x.Product.Name,
						BranchId = x.CompanyBranchId,
						Week = x.Week
					}).ToList();
				}
			}
			return productVaccineList;
		}

		public bool EditProductVaccine(ProductVaccineViewModel productVaccineDetails)
		{
			if (productVaccineDetails != null)
			{
				var productVaccineEdit = _context.ProductVaccines.Where(c => c.Id == productVaccineDetails.Id).FirstOrDefault();
				if (productVaccineEdit != null)
				{
					productVaccineEdit.Name = productVaccineDetails.Name;
					productVaccineEdit.Week = productVaccineDetails.Week;
					productVaccineEdit.ProductId = productVaccineDetails.ProductId;
				}
				_context.ProductVaccines.Update(productVaccineEdit);
				_context.SaveChanges();
				return true;
			}
			return false;
		}

		public bool DeleteProductVaccine(ProductVaccineViewModel productVaccineDetails)
		{
			if (productVaccineDetails != null)
			{
				var productVaccine = _context.ProductVaccines.Where(c => c.Id == productVaccineDetails.Id).FirstOrDefault();
				if (productVaccine != null)
				{
					productVaccine.Deleted = true;
					productVaccine.Active = false;
				}
				_context.ProductVaccines.Update(productVaccine);
				_context.SaveChanges();
				return true;
			}
			return false;
		}
	}
}
