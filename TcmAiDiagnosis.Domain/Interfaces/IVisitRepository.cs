using TcmAiDiagnosis.Entities;

namespace TcmAiDiagnosis.Domain.Interfaces
{
    /// <summary>
    /// 就诊记录仓储接口
    /// </summary>
    public interface IVisitRepository
    {
        /// <summary>
        /// 根据ID获取就诊记录
        /// </summary>
        Task<Visit> GetByIdAsync(int id);

        /// <summary>
        /// 获取就诊记录列表（分页）
        /// </summary>
        Task<List<Visit>> GetAllAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 获取就诊记录总数
        /// </summary>
        Task<int> GetCountAsync();

        /// <summary>
        /// 添加就诊记录
        /// </summary>
        Task AddAsync(Visit visit);

        /// <summary>
        /// 更新就诊记录
        /// </summary>
        Task UpdateAsync(Visit visit);

        /// <summary>
        /// 删除就诊记录
        /// </summary>
        Task DeleteAsync(int id);
    }
}
