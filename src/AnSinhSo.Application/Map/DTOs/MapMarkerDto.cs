using System;

namespace AnSinhSo.Application.Map.DTOs;

public class MapMarkerDto
{
    public Guid Id { get; set; }
    public string MarkerType { get; set; } = string.Empty; // "Citizen", "Household", "PaymentPoint", "Welfare"
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Color { get; set; } = string.Empty; // "green", "yellow", "red", "blue", "purple"
    public string PopupTitle { get; set; } = string.Empty;
    public string PopupContent { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
