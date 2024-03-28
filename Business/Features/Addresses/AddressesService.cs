using Core.Dtos;
using Core.Entities;
using Core.Infrastructure.Base.RepositoriesBase;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Addresses;
public class AddressesService(IRepositoryBase<CustomerAddress> addressRepository) : IAddressesService
{
    public async Task<ICollection<AddressViewModel>> GetAllCustomerAddressesMain(Guid userId)
    {
        return await addressRepository.GetListAsync(query => query.Select(p => new AddressViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Text = p.Text
        }),predicate: p => p.CustomerId == userId, orderBy: query => query.OrderBy(p => p.Name),include: null,withDeleted:false,asNoTracking: true);
    }

    public async Task<AddressViewModel> GetCustomerAddressByIdMain(Guid id)
    {
        return await addressRepository.GetAsync(predicate: p => p.Id == id, select : query => query.Select(p => new AddressViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Text = p.Text
        }), include: null, withDeleted: false, asNoTracking: true);
    }

    public async Task<int> CreateCustomerAddressAsync(AddressInputModel inputModel, Guid userId)
    {
        var result = await addressRepository.CreateAsync(new CustomerAddress { Name = inputModel.Name, Text = inputModel.Text, CustomerId = userId });
        return result;
    }

    public async Task<int> UpdateCustomerAddressAsync(AddressInputModel inputModel, Guid id)
    {
        var result = await addressRepository.ExecuteUpdateAsync(p => p.Id == id, s => s.SetProperty(p => p.Name, inputModel.Name).SetProperty(p => p.Text, inputModel.Text));
        return result;
    }

    public async Task<int> DeleteCustomerAddressAsync(Guid id)
    {
        var result = await addressRepository.ExecuteDeleteAsync(p => p.Id == id);
        return result;
    }
    public async Task<ICollection<KeyNameDTO>> GetKeyNameListAsync(Guid userId)
    {
        return await addressRepository.GetListAsync(query => query.Select(p => new KeyNameDTO
        {
            Id = p.Id,
            Name = p.Name,
        }),predicate: p => p.CustomerId == userId, query => query.OrderBy(p => p.Name), include: null, withDeleted: false, asNoTracking: true);
    }
}
