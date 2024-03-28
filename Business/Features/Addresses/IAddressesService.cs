using Core.Dtos;
using Core.Entities;
using Core.ViewModels;

namespace Business.Features.Addresses;

public interface IAddressesService
{
    Task<ICollection<AddressViewModel>> GetAllCustomerAddressesMain(Guid userId);
    Task<AddressViewModel> GetCustomerAddressByIdMain(Guid id);
    Task<int> CreateCustomerAddressAsync(AddressInputModel inputModel, Guid userId);
    Task<int> UpdateCustomerAddressAsync(AddressInputModel inputModel, Guid id);
    Task<int> DeleteCustomerAddressAsync(Guid id);
    Task<ICollection<KeyNameDTO>> GetKeyNameListAsync(Guid userId);
}