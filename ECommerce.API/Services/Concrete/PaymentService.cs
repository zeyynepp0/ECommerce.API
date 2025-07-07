using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;

        public PaymentService(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Payment>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Payment> GetByIdAsync(int id)
        {
            var payment = await _repo.GetByIdAsync(id);
            if (payment == null) throw new Exception("Payment not found");
            return payment;
        }

        public async Task AddAsync(Payment payment)
        {
            await _repo.AddAsync(payment);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _repo.Update(payment);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _repo.GetByIdAsync(id);
            if (payment != null)
            {
                _repo.Delete(payment);
                await _repo.SaveAsync();
            }
        }
    }
}
