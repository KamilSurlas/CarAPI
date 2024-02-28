using CarAPI.Models;

namespace CarAPI.Services
{
    public interface IInsuranceService
    {
        InsuranceDto GetInsurance(int carId);
        void UpdateInsurance(int carId, UpdateInsuranceDto dto);
    }
}
