using AutoMapper;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Application.Queries.Customers;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.SharedKernel.Domain.Abstraction;
using MediatR;

namespace Mc2.CrudTest.Application.Handlers.Customers.Queries;

public class CustomerGetQueryHandler : IRequestHandler<CustomerGetQuery, ServiceQueryResult<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerGetQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceQueryResult<CustomerDto>> Handle(CustomerGetQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetAsync(request.Id, cancellationToken);

        if (customer is null)
        {
            return new ServiceQueryResult<CustomerDto>(null as CustomerDto);
        }

        return new ServiceQueryResult<CustomerDto>(_mapper.Map<CustomerDto>(customer));

    }
}
