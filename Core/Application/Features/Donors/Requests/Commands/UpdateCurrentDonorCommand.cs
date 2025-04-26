using Application.DTOs.Donors.Responses;
using MediatR;
using NetTopologySuite.Geometries;

namespace Application.Features.Donors.Requests.Commands;

public class UpdateCurrentDonorCommand : IRequest<DonorResponseDTO>
{
	public string Email { get; set; } = null!;
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public double? Height { get; set; }
	public double? Weight { get; set; }
	public string? Gender { get; set; }
	public int? BloodTypeId { get; set; }
	public DateTime? LastDonation { get; set; }
	public string? AddressLine { get; set; }
	public string? City { get; set; }
	public string? PostalCode { get; set; }
	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
}