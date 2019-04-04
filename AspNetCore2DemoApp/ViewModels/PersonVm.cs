using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore2DemoApp.ViewModels
{
    public class PersonVm
    {
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$"), Required, StringLength(30)]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$"), Required, StringLength(30)]
        public string Lastname { get; set; }

        [Display(Name = "Date of Birth"), DataType(DataType.Date)]
        public DateTime DoB { get; set; }

        [Display(Name = "Deposit")]
        public string HasDeposit { get; set; }

        [Display(Name = " Deposit Description")]
        public string DepositDesc { get; set; }
    }

    public class PersonValidator : AbstractValidator<PersonVm>
    {
        public PersonValidator()
        {
            RuleFor(x => x.HasDeposit).NotEmpty();
            RuleFor(x => x.DepositDesc).NotNull().When(y => y.HasDeposit == "2");
        }

    }

}
