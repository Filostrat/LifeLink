using Application.DTOs.BloodType.Requests;
using MediatR;


namespace Application.Features.BloodTypes.Requests.Queries;

public class GetAllBloodTypesQuery : IRequest<List<BloodTypeDTO>>;