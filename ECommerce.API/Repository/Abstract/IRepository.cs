// LINQ ifadeleriyle çalışmak için gerekli namespace'i içeri aktarır
using System.Linq.Expressions;

// Repository arayüzlerini içeren namespace
namespace ECommerce.API.Repository.Abstract
{
    // Generic repository arayüzü. Tüm entity'ler için temel CRUD işlemlerini tanımlar.
    public interface IRepository<T> where T : class
    {
        // Tüm kayıtları asenkron olarak getirir
        Task<List<T>> GetAllAsync();
        // Id'ye göre tek bir kaydı asenkron olarak getirir
        Task<T> GetByIdAsync(int id);
        // Belirtilen koşula uyan kayıtları asenkron olarak getirir
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        // Yeni bir kayıt ekler (asenkron)
        Task AddAsync(T entity);
        // Var olan bir kaydı günceller
        void Update(T entity);
        // Var olan bir kaydı siler
        void Delete(T entity);
        // Yapılan değişiklikleri veritabanına kaydeder (asenkron)
        Task SaveAsync();
    }
}
