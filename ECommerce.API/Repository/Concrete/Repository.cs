// Gerekli namespace'ler projeye dahil ediliyor
using ECommerce.API.Data; // Veritabanı context'i için
using ECommerce.API.Repository.Abstract; // IRepository arayüzü için
using Microsoft.EntityFrameworkCore; // Entity Framework işlemleri için
using System.Linq.Expressions; // LINQ ifadeleri için

// Concrete repository sınıflarını içeren namespace
namespace ECommerce.API.Repository.Concrete
{
    // Generic repository pattern'ini uygulayan temel sınıf. Tüm entity'ler için ortak CRUD işlemlerini sağlar.
    public class Repository<T> : IRepository<T> where T : class
    {
        // Veritabanı context'i
        private readonly MyDbContext _context;
        
        // Entity'e karşılık gelen DbSet
        private readonly DbSet<T> _dbSet;

        // Alt sınıfların kullanması için protected Context özelliği
        protected MyDbContext Context => _context;

        // Repository sınıfı için constructor
        // <param name="context">Veritabanı context'i</param>
        public Repository(MyDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Tüm kayıtları asenkron olarak getirir
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Id'ye göre tek bir kaydı asenkron olarak getirir
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Belirtilen koşula uyan kayıtları asenkron olarak getirir
        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // Yeni bir kayıt ekler (asenkron)
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Var olan bir kaydı günceller
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Var olan bir kaydı siler
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Yapılan değişiklikleri veritabanına kaydeder (asenkron)
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
