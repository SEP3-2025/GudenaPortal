using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Gudena.Data.Repositories;

namespace Gudena.Services
{
    public class AccountDetailsService : IAccountDetailsService
    {
        private readonly IAccountDetailsRepository _repository;

        public AccountDetailsService(IAccountDetailsRepository repository)
        {
            _repository = repository;
        }

        public async Task<AccountDetails?> GetAccountDetailsForUserAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        private static readonly HashSet<string> EuCountries = new(StringComparer.OrdinalIgnoreCase)
        {
            "Austria","Belgium","Bulgaria","Croatia","Cyprus","Czech Republic","Denmark","Estonia",
            "Finland","France","Germany","Greece","Hungary","Ireland","Italy","Latvia","Lithuania",
            "Luxembourg","Malta","Netherlands","Poland","Portugal","Romania","Slovakia","Slovenia",
            "Spain","Sweden"
        };

        public async Task UpdateAddressAsync(string userId, AddressDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Only validate when Country is provided (partial update allowed)
            if (!string.IsNullOrWhiteSpace(dto.Country) && !EuCountries.Contains(dto.Country))
                throw new InvalidOperationException($"Country '{dto.Country}' is not in the EU.");

            await _repository.UpdateAddressAsync(userId, dto);
        }
        
        public async Task<AddressDto?> GetAddressAsync(string userId)
        {
            var acc = await _repository.GetByUserIdAsync(userId);
            if (acc == null) return null;

            return new AddressDto
            {
                Street = acc.Street,
                PostalCode = acc.PostalCode,
                City = acc.City,
                Country = acc.Country
            };
        }
    }
}