using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class EditLastWorkingDateCommandValidater :AbstractValidator<EditLastWorkingDateCommand>
    {
        public EditLastWorkingDateCommandValidater()
        {
            RuleFor(x => x.LastWorkingDay).NotEmpty().WithMessage("Last Workind date is mandatory");
            RuleFor(x => x.LastWorkingDay).GreaterThanOrEqualTo(DateTime.Now).WithMessage("Last Working Date can not be in the past");
        }
    }
}
