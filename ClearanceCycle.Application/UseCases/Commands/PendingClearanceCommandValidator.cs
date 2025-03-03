using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class PendingClearanceCommandValidator :AbstractValidator<PendingClearanceCommand>
    {
        public PendingClearanceCommandValidator()
        {
            RuleFor(x => x.Comment).NotEmpty().MinimumLength(5).WithMessage("Comment is Mandatory");
        }
    }
}
