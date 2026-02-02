using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicCRM.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Получение всех записей
        Task<IEnumerable<T>> GetAllAsync();

        // Получение записи по ID
        Task<T> GetByIdAsync(int id);

        // Поиск по условию
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Добавление записи
        Task<T> AddAsync(T entity);

        // Обновление записи
        Task UpdateAsync(T entity);

        // Удаление записи
        Task DeleteAsync(int id);

        // Проверка существования записи
        Task<bool> ExistsAsync(int id);

        // Сохранение изменений
        Task<int> SaveChangesAsync();
    }
}
