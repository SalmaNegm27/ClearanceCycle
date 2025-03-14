using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearanceCycle.Application.Dtos;
using ClearanceCycle.Application.Interfaces;
using MediatR;

namespace ClearanceCycle.Application.UseCases.Commands
{
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, ReponseDto>
    {
        private readonly IWriteRepository _writeRepository;
        public UploadDocumentCommandHandler(IWriteRepository writeRepository)
        {
            _writeRepository = writeRepository;
        }
        public async Task<ReponseDto> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
          return await  _writeRepository.UploadClearanceFile(request);
        }
    }
}
