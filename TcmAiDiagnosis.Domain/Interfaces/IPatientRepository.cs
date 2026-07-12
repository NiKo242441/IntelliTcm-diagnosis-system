using TcmAiDiagnosis.Entities;

namespace TcmAiDiagnosis.Domain.Interfaces
{
    /// <summary>
    /// 患者仓储接口
    /// </summary>
    public interface IPatientRepository
    {
        /// <summary>
        /// 根据PatientGuid获取患者
        /// </summary>
        Task<Patient> GetByIdAsync(int id);

        /// <summary>
        /// 获取患者列表（分页）
        /// </summary>
        Task<List<Patient>> GetAllAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 获取患者总数
        /// </summary>
        Task<int> GetCountAsync();

        /// <summary>
        /// 添加患者
        /// </summary>
        Task AddAsync(Patient patient);

        /// <summary>
        /// 更新患者
        /// </summary>
        Task UpdateAsync(Patient patient);

        /// <summary>
        /// 删除患者
        /// </summary>
        Task DeleteAsync(string patientGuid);
    }
}
