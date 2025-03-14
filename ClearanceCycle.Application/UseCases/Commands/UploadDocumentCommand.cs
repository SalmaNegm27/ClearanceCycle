using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public record UploadDocumentCommand :IRequest<ReponseDto>
    {
        public int RequestId { get; set; }
        public IFormFile File { get; set; }
    }
}
