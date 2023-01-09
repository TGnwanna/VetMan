using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.VetManEnums;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;

namespace Core.ViewModels.Treatments
{
    public class PatientViewModel
    {
        public DateTime DOB { get; set; }
        public string? BirthDay { get; set; }
        public string? Profession { get; set; }
        public Species SpecieId { get; set; }
        public int GenderId { get; set; }
        public int CustomerGenderId { get; set; }
        public int Id { get; set; }
        public string? Gender { get; set; }
        public string? Name { get; set; }
        public int? BreedId { get; set; }
        public string? Breed { get; set; }
        public string? Colour { get; set; }
        public string? SpecialMarking { get; set; }
        public IFormFile? Image  { get; set; }
        public string? Picture { get; set; }
        public string? CaseNumber { get; set; }

        //Customer
        public Guid? CustomerId { get; set; }
        public string? Customer { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsExistingClient { get; set; }
    }
}
