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
        }
    }
}
