using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class AddClearanceCommandValidator : AbstractValidator<AddClearanceCommand>
    {
        public AddClearanceCommandValidator()
        {
            RuleFor(x => x.ResigneeName).NotEmpty().MaximumLength(50).WithMessage("Employee name must valid.");
            RuleFor(x => x.ResigneeHrId).NotEmpty().MaximumLength(30).WithMessage("Employee Hrid must valid.");
            RuleFor(x => x.CompanyId).GreaterThan(0).WithMessage("Copmany name must valid.");
            RuleFor(x => x.ResigneeId).GreaterThan(0).WithMessage("Employee Id must valid.");
            RuleFor(x => x.ResignationReasonId).GreaterThan(0).WithMessage("Resignation Reasons must valid.");
            RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(30).WithMessage("Requester name must valid.");
            RuleFor(x => x.LastWorkingDay).NotEmpty().WithMessage("Last working day must be valid.");


        }
    }
}
