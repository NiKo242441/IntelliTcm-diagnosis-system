using Microsoft.EntityFrameworkCore;
using TcmAiDiagnosis.Entities;
using TcmAiDiagnosis.Domain.Interfaces;
using TcmAiDiagnosis.EFContext;

namespace TcmAiDiagnosis.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// 治疗方案仓储实现
    /// </summary>
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly TcmAiDiagnosisContext _context;

        public TreatmentRepository(TcmAiDiagnosisContext context)
        {
            _context = context;
        }

        public async Task<Treatment> GetByIdAsync(int id)
        {
            return await _context.Treatments.FindAsync(id);
        }

        public async Task<List<Treatment>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await _context.Treatments
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Treatments.CountAsync();
        }

        public async Task AddAsync(Treatment treatment)
        {
            await _context.Treatments.AddAsync(treatment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Treatment treatment)
        {
            _context.Treatments.Update(treatment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            if (treatment != null)
            {
                _context.Treatments.Remove(treatment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
