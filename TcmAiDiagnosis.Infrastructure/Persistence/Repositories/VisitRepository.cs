using Microsoft.EntityFrameworkCore;
using TcmAiDiagnosis.Entities;
using TcmAiDiagnosis.Domain.Interfaces;
using TcmAiDiagnosis.EFContext;

namespace TcmAiDiagnosis.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// 就诊记录仓储实现
    /// </summary>
    public class VisitRepository : IVisitRepository
    {
        private readonly TcmAiDiagnosisContext _context;

        public VisitRepository(TcmAiDiagnosisContext context)
        {
            _context = context;
        }

        public async Task<Visit> GetByIdAsync(int id)
        {
            return await _context.Visits.FindAsync(id);
        }

        public async Task<List<Visit>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await _context.Visits
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Visits.CountAsync();
        }

        public async Task AddAsync(Visit visit)
        {
            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Visit visit)
        {
            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit != null)
            {
                _context.Visits.Remove(visit);
                await _context.SaveChangesAsync();
            }
        }
    }
}
