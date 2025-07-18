using ECommerce.API.Entities.Concrete;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Services.Abstract;

namespace ECommerce.API.Services.Concrete
{
   
    /// Ödeme işlemleriyle ilgili iş mantığını yöneten servis sınıfı.
   
    public class PaymentService : IPaymentService
    {
       
        /// Ödeme repository'si (veri erişim katmanı).
       
        private readonly IPaymentRepository _repo;

       
        /// PaymentService constructor.
       
        /// <param name="repo">Ödeme repository'si</param>
        public PaymentService(IPaymentRepository repo)
        {
            _repo = repo;
        }

       
        /// Tüm ödemeleri getirir.
       
        public async Task<List<Payment>> GetAllAsync() => await _repo.GetAllAsync();

       
        /// Id'ye göre ödemeyi getirir. Ödeme yoksa hata fırlatır.
       
        public async Task<Payment> GetByIdAsync(int id)
        {
            var payment = await _repo.GetByIdAsync(id);
            if (payment == null) throw new Exception("Payment not found");
            return payment;
        }

       
        /// Yeni ödeme ekler.
       
        public async Task AddAsync(Payment payment)
        {
            await _repo.AddAsync(payment);
            await _repo.SaveAsync();
        }

       
        /// Var olan ödemeyi günceller.
       
        public async Task UpdateAsync(Payment payment)
        {
            _repo.Update(payment);
            await _repo.SaveAsync();
        }

       
        /// Id'ye göre ödemeyi siler (varsa).
       
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
