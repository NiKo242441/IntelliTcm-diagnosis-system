using TcmAiDiagnosis.Entities;

namespace TcmAiDiagnosis.Domain.Interfaces
{
    /// <summary>
    /// 治疗方案仓储接口
    /// </summary>
    public interface ITreatmentRepository
    {
        /// <summary>
        /// 根据ID获取治疗方案
        /// </summary>
        Task<Treatment> GetByIdAsync(int id);

        /// <summary>
        /// 获取治疗方案列表（分页）
        /// </summary>
        Task<List<Treatment>> GetAllAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 获取治疗方案总数
        /// </summary>
        Task<int> GetCountAsync();

        /// <summary>
        /// 添加治疗方案
        /// </summary>
        Task AddAsync(Treatment treatment);

        /// <summary>
        /// 更新治疗方案
        /// </summary>
        Task UpdateAsync(Treatment treatment);

        /// <summary>
        /// 删除治疗方案
        /// </summary>
        Task DeleteAsync(int id);
    }
}
