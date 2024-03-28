using Core.Dtos;
using Core.Entities;
using Core.Enums;

namespace Core.ViewModels;
public class OrderViewModel
{
    public Guid Id { get; set; }    
    public string? CargoTrackingNumber { get; set; } 
    public DateTime DateCreated { get; set; }
    public DeliveryStatus Status { get; set; }
    public AddressDTO Address { get; set; } = null!;
    public IEnumerable<OrderDetailDTO> OrderDetails { get; set; } = new HashSet<OrderDetailDTO>();
    public decimal GrandTotal => OrderDetails.Sum(p => p.LineTotal);
}

