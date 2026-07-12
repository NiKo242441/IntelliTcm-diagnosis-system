using Microsoft.EntityFrameworkCore;
using TcmAiDiagnosis.Entities;
using TcmAiDiagnosis.Domain.Interfaces;
using TcmAiDiagnosis.EFContext;

namespace TcmAiDiagnosis.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// 患者仓储实现
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly TcmAiDiagnosisContext _context;

        public PatientRepository(TcmAiDiagnosisContext context)
        {
            _context = context;
        }

        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientGuid == id.ToString());
        }

        public async Task<List<Patient>> GetAllAsync(int pageIndex, int pageSize)
        {
            return await _context.Patients
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Patients.CountAsync();
        }

        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string patientGuid)
        {
            var patient = await _context.Patients.FindAsync(patientGuid);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}