# CLAUDE.md - 中医数字化诊疗平台项目指南

## 项目概述
- **项目名称**: 本草智诊-中医证候分析系统（TcmAiDiagnosis）
- **项目编号**: TCM-DIAG-2024
- **技术栈**: .NET 8 + Entity Framework Core 8 + MySQL 8.0 + Razor Pages
- **AI集成**: Dify 工作流平台
- **项目目标**: 企业级中医SaaS诊疗平台
- **架构模式**: 四层架构（Presentation → Application → Domain → Infrastructure）

## 项目背景与目标

### 业务目标
- 为中小型中医诊所提供低成本、高效率的SaaS诊疗管理系统
- 实现患者信息、就诊记录、病历处方的全流程数字化管理
- 通过AI技术提高中医诊疗的准确性和一致性
- 促进中医知识的传承和标准化

### 技术目标
- 构建稳定可靠的.NET 8后端服务
- 开发用户友好的Web前端界面
- 确保系统具备良好的扩展性、安全性与可维护性
- 集成AI算法模块，实现智能化的诊断辅助

### 核心功能模块
- **多租户管理**: 支持多个医疗机构独立使用，数据完全隔离
- **用户权限管理**: 基于RBAC的细粒度权限控制（110+权限点）
- **患者管理**: 完整的患者档案管理
- **诊疗管理**: 四诊信息结构化采集（望、闻、问、切）
- **AI辅助诊断**: 基于Dify工作流的智能证候分析
- **治疗方案管理**: 智能治疗方案生成，支持配伍禁忌检查
- **就诊系列管理**: 围绕主诉组织连续诊疗过程
- **知识库管理**: 中医理论、方剂、药材知识体系

## 架构说明

### 四层架构职责

```
┌─────────────────────────────────────────────────────────────┐
│  表现层 (Presentation) - TcmAiDiagnosis.Web                 │
│  职责: 处理HTTP请求，渲染页面，返回JSON                       │
│  内容: Razor Pages、Controllers、ViewModels、Middleware      │
├─────────────────────────────────────────────────────────────┤
│  应用层 (Application) - TcmAiDiagnosis.Application (待建)   │
│  职责: 编排业务流程，协调领域服务，DTO映射                     │
│  内容: 应用服务、DTOs、Mappings、Validators                  │
├─────────────────────────────────────────────────────────────┤
│  领域层 (Domain) - TcmAiDiagnosis.Domain                    │
│  职责: 核心业务逻辑，领域规则，领域实体                       │
│  内容: Entities、Services、Events、Exceptions、Interfaces   │
├─────────────────────────────────────────────────────────────┤
│  基础设施层 (Infrastructure) - TcmAiDiagnosis.Infrastructure │
│  职责: 数据访问，外部服务集成，缓存实现                       │
│  内容: Repositories、ExternalServices、Cache、Logging       │
└─────────────────────────────────────────────────────────────┘
```

### 依赖方向

```
表现层 → 应用层 → 领域层 ← 基础设施层
```

**禁止的依赖**:
- ❌ 领域层 → 基础设施层（依赖倒置原则）
- ❌ 基础设施层 → 应用层
- ❌ 表现层 → 领域层（应通过应用层）

### 当前项目引用关系

```
TcmAiDiagnosis.Web
    ├── TcmAiDiagnosis.Domain
    ├── TcmAiDiagnosis.Entities
    ├── TcmAiDiagnosis.EFContext
    ├── TcmAiDiagnosis.Dtos
    └── TcmAiDiagnosis.Infrastructure ✅ 新增
            ├── TcmAiDiagnosis.Domain
            └── TcmAiDiagnosis.Entities
```

### 文件组织

```
TcmAiDiagnosis/
├── TcmAiDiagnosis.Web/              # 表现层
│   ├── Areas/                       # 按角色划分区域
│   │   ├── Doctor/                  # 医生端
│   │   ├── Patient/                 # 患者端（待开发）
│   │   ├── Pharmacist/              # 药剂师端
│   │   └── Manager/                 # 管理员端（待开发）
│   ├── Controller/                  # API控制器
│   ├── ViewModels/                  # 视图模型（每个页面独立）
│   ├── Middleware/                   # 中间件
│   ├── Attributes/                  # 自定义特性
│   └── Data/                        # 数据初始化
│
├── TcmAiDiagnosis.Application/      # 应用层（待创建）
│   ├── Services/                    # 应用服务（*AppService）
│   ├── DTOs/                        # 数据传输对象
│   ├── Mappings/                    # AutoMapper配置
│   ├── Interfaces/                  # 服务接口
│   └── Validators/                  # 输入验证
│
├── TcmAiDiagnosis.Domain/           # 领域层
│   ├── Entities/                    # 领域实体
│   ├── Services/                    # 领域服务（*Domain）
│   ├── Events/                      # 领域事件
│   ├── Exceptions/                  # 领域异常
│   ├── Constants/                   # 业务常量
│   ├── Interfaces/                  # 仓储接口
│   └── Specifications/              # 规约模式
│
├── TcmAiDiagnosis.Infrastructure/   # 基础设施层 ✅ 已创建
│   ├── Persistence/                 # 数据持久化
│   │   ├── DbContext/               # EF上下文 ✅ 已迁移
│   │   ├── Repositories/            # 仓储实现 ✅ 已创建
│   │   ├── Configurations/          # 实体配置
│   │   └── Migrations/              # 数据库迁移
│   ├── ExternalServices/            # 外部服务集成
│   │   ├── Dify/                    # Dify AI服务
│   │   └── Cache/                   # 缓存实现
│   └── Logging/                     # 日志实现
│
├── TcmAiDiagnosis.Entities/         # 保留（兼容过渡期）
├── TcmAiDiagnosis.EFContext/        # 保留（兼容过渡期）
└── TcmAiDiagnosis.Dtos/             # 保留（兼容过渡期）
```

## 代码规范

### 命名规范

#### C# 命名规范
- **类名**: 帕斯卡命名法（`TreatmentDomain`, `PatientService`）
- **接口名**: 帕斯卡命名法，前缀为 I（`IPatientRepository`）
- **方法名**: 帕斯卡命名法（`GetSyndromeOverviewAsync`）
- **属性名**: 帕斯卡命名法（`PatientName`）
- **变量名**: 驼峰命名法（`patientInfo`, `syndromeName`）
- **常量**: 帕斯卡命名法（`PermissionConstants`）
- **私有字段**: 下划线前缀（`_context`, `_logger`）
- **枚举**: 帕斯卡命名法（`DiagnosisStatus`）

#### 数据库命名规范
- **表名**: snake_case，复数形式（`patients`, `visit_series`）
- **字段名**: snake_case（`patient_guid`, `created_at`）
- **主键**: `id`
- **外键**: `{related_table}_id`
- **时间戳**: `created_at`, `updated_at`, `deleted_at`
- **索引**: `idx_{table}_{columns}`

### 类命名约定
- **应用服务**: `*AppService`（如 `PatientAppService`）
- **领域服务**: `*Domain`（如 `PatientDomain`）
- **仓储接口**: `I*Repository`（如 `IPatientRepository`）
- **仓储实现**: `*Repository`（如 `PatientRepository`）
- **视图模型**: `*ViewModel`（如 `PatientListViewModel`）
- **输入对象**: `*Input` 或 `Create*Input`（如 `CreatePatientInput`）
- **DTO**: `*Dto`（如 `PatientDto`）

### 注释规范
- 使用**中文注释**（XML文档注释）
- 公共方法必须有 `<summary>` 注释
- 复杂业务逻辑要有行内注释
- 异常情况要说明原因

### 代码质量目标
```yaml
质量标准:
  代码覆盖率: > 80%
  复杂度: Cyclomatic Complexity < 10
  重复代码: < 3%
  技术债务: 0个Blocker, 0个Critical
  性能要求: API响应时间 < 500ms
```

## 开发流程

### 新功能开发步骤（标准流程）

#### 1. 领域层 - 实体和接口

```csharp
// Domain/Entities/Patient.cs
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    // 业务方法
    public void UpdateName(string name) { ... }
}

// Domain/Interfaces/IPatientRepository.cs
public interface IPatientRepository
{
    Task<Patient> GetByIdAsync(int id);
    Task<List<Patient>> GetAllAsync();
    Task AddAsync(Patient patient);
    Task UpdateAsync(Patient patient);
}
```

#### 2. 基础设施层 - 仓储实现

```csharp
// Infrastructure/Persistence/Repositories/PatientRepository.cs
public class PatientRepository : IPatientRepository
{
    private readonly TcmAiDiagnosisContext _context;
    
    public async Task<Patient> GetByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }
    // ...
}
```

#### 3. 应用层 - 应用服务和DTO

```csharp
// Application/DTOs/Patients/CreatePatientInput.cs
public class CreatePatientInput
{
    [Required]
    public string Name { get; set; }
}

// Application/Services/PatientAppService.cs
public class PatientAppService : IPatientAppService
{
    private readonly IPatientRepository _patientRepository;
    
    public async Task<PatientDto> CreatePatientAsync(CreatePatientInput input)
    {
        var patient = new Patient { Name = input.Name };
        await _patientRepository.AddAsync(patient);
        return patient.ToDto();
    }
}
```

#### 4. 表现层 - 页面或控制器

```csharp
// Web/Areas/Doctor/Pages/Patients/Create.cshtml.cs
public class CreateModel : PageModel
{
    private readonly IPatientAppService _patientService;
    
    [BindProperty]
    public CreatePatientInput Input { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        
        await _patientService.CreatePatientAsync(Input);
        return RedirectToPage("./Index");
    }
}
```

### 模块开发优先级
1. 先完成核心业务流程（登录→患者→就诊→治疗）
2. 再完善辅助功能（知识库、统计）
3. 最后优化体验（UI美化、性能优化）

## 常用命令

```bash
# 运行项目
dotnet run

# 数据库迁移
dotnet ef migrations add <MigrationName> --project TcmAiDiagnosis.Infrastructure
dotnet ef database update --project TcmAiDiagnosis.Infrastructure

# 运行测试
dotnet test

# 清理构建
dotnet clean && dotnet build
```

## 关键配置

### 数据库连接
- 配置文件: `appsettings.json`
- 连接字符串: `ConnectionStrings:Connection`
- 数据库: MySQL 8.0
- ORM: Entity Framework Core 8

### AI工作流配置（Dify）
```json
{
  "DifyApi": {
    "BaseUrl": "http://192.168.88.253:18001",
    "ApiKey": "app-xxx",
    "OverviewWorkflowApiKey": "app-xxx",  // 证候概览工作流
    "DetailWorkflowApiKey": "app-xxx",    // 证候详情工作流
    "Endpoint": "/v1/workflows/run",
    "ResponseMode": "blocking",
    "TimeoutSeconds": 60
  }
}
```

### 权限系统配置
```json
{
  "PermissionSettings": {
    "EnablePermissionCheck": true,
    "CacheExpirationMinutes": 30,
    "WhitelistPaths": [
      "/api/auth/login",
      "/Identity/Account/Login",
      "/css", "/js"
    ],
    "AuditLogLevel": "Warning"
  }
}
```

### 权限系统
- **权限总数**: 110个
- **角色数量**: 4个（Manager、Doctor、Patient、Pharmacist）
- **权限常量**: `Domain/Constants/PermissionConstants.cs`
- **权限检查**: `Middleware/PermissionAuthorizationMiddleware.cs`
- **权限数据种子**: `Web/Data/PermissionSeeder.cs`
- **角色权限种子**: `Web/Data/RolePermissionSeeder.cs`

#### 角色权限分布
| 角色 | 权限数量 | 主要职责 |
|------|---------|---------|
| 管理员 (Manager) | 23 | 租户管理、用户管理、系统配置 |
| 医生 (Doctor) | 56 | 诊疗操作、治疗方案、病历管理 |
| 患者 (Patient) | 10 | 查看个人信息、诊疗记录、治疗方案 |
| 药剂师 (Pharmacist) | 6 | 查看处方、中药知识库 |

#### 权限代码格式
```
{模块}.{操作}
示例: patient.create, treatment.view, diagnosis.use_ai_diagnosis
```

### 数据库核心表
```sql
-- 用户相关
users, user_details, roles, user_roles

-- 租户相关
tenants, tenant_configurations

-- 患者相关
patients, doctor_patient_associations

-- 诊疗相关
visit_series, visits, treatments, prescriptions, prescription_items
non_drug_therapies, advices, warnings

-- 权限相关
permissions, role_permissions, temporary_permissions, permission_change_logs

-- 知识库相关
knowledge_base, herbs, formulas, acupoints
```

## 页面设计规范

### 设计理念
- **现代化设计**: 采用PlainAdmin的简洁现代风格
- **专业性**: 体现医疗系统的专业性和权威性
- **易用性**: 简洁明了的界面，降低用户学习成本
- **一致性**: 统一的视觉语言和交互模式

### 配色方案（医疗系统专业配色）
```css
:root {
    --medical-primary: #4F6FBF;      /* 柔和医疗蓝 */
    --medical-secondary: #6B85D1;    /* 浅主色调 */
    --medical-accent: #3A5299;       /* 深主色调 */
    --medical-highlight: #365CF5;    /* 强调色 */
    --medical-light: #F8FAFC;        /* 页面背景色 */
    --medical-card-bg: #FFFFFF;      /* 卡片背景色 */
    --medical-border: #E2E8F0;       /* 边框色 */
    --medical-text: #475569;         /* 主文本色 */
    --medical-success: #059669;      /* 健康/正常状态 */
    --medical-warning: #D97706;      /* 需要注意 */
    --medical-danger: #DC2626;       /* 异常/紧急 */
}
```

### 字体规范
- **主字体**: "Plus Jakarta Sans", sans-serif
- **字体大小**: 14px（表单和按钮标准大小）
- **字体权重**: 600（标签和按钮），500（普通文本）

### 布局规范
- **页面容器**: `.medical-container`
- **卡片布局**: `.medical-card`（圆角7px，阴影效果）
- **卡片头部**: `.medical-card-header`（背景色var(--medical-primary)）
- **卡片内容**: `.medical-card-body`（内边距25px 30px）

## AI提示词规范

### 证候概览工作流
- **输入**: `patient_description`（患者基本信息和主诉）、`visit_description`（详细问诊信息）
- **输出**: 3-5个证候，每个包含证候名称、置信度、主要症状、常见疾病
- **置信度标准**: 80-100=高度典型，60-79=较典型，40-59=部分符合

### 证候详情工作流
- **输入**: 证候名称、患者详细信息
- **输出**: 证候详细分析、病因病机、治则治法

### 治疗方案工作流
- **输入**: 证候分析结果、患者信息
- **输出**: 中药方剂推荐、针灸穴位推荐、食疗药膳、生活方式指导

## 异常处理规范

### 异常分类
```csharp
// 业务异常
public class BusinessException : Exception
{
    public string ErrorCode { get; }
}

// 验证异常
public class ValidationException : BusinessException

// 资源未找到异常
public class NotFoundException : BusinessException
```

### 全局异常处理中间件
```csharp
// 返回格式
{
    "statusCode": 400,
    "errorCode": "VALIDATION_ERROR",
    "message": "验证失败",
    "details": {...},
    "timestamp": "2024-01-01T00:00:00Z",
    "path": "/api/patients"
}
```

## 日志规范

### 日志级别
- **Debug**: 调试信息，仅开发环境
- **Information**: 一般信息
- **Warning**: 警告信息
- **Error**: 错误信息
- **Critical**: 严重错误

### 结构化日志示例
```csharp
_logger.LogInformation("创建患者: {PatientName}, ID: {PatientId}", patient.Name, patient.Id);
_logger.LogError(ex, "获取患者失败: {PatientId}", patientId);
```

## 测试规范

### 单元测试命名
```csharp
// 格式: {MethodName}_{Scenario}_{ExpectedResult}
[Fact]
public void GetPatientById_ValidId_ReturnsPatient()

[Fact]
public void GetPatientById_InvalidId_ThrowsArgumentException()
```

### 测试数据构建器
```csharp
var patient = new PatientBuilder()
    .WithId(123)
    .WithName("张三")
    .Build();
```

## 注意事项

### 安全规范
- ❌ 不要在代码中硬编码密码或API Key
- ✅ 使用环境变量或 User Secrets 存储敏感信息
- ✅ 生产环境必须启用 HTTPS
- ✅ 数据库连接使用SSL加密

### 代码质量
- ✅ 每个公共方法要有XML注释
- ✅ 异常处理要统一（使用自定义异常类）
- ✅ 避免重复代码，提取公共方法
- ✅ 代码审查清单：功能正确性、代码质量、性能、安全性、测试

### 性能优化
- ✅ 数据库查询要分页
- ✅ 热点数据使用缓存（Redis）
- ✅ 避免N+1查询问题
- ✅ API响应时间 < 500ms

## 已知问题（待修复）
1. ~~`Program.cs` 中重复注册服务（AddDbContext、InventoryAlertRuleDomain）~~ ✅ 已修复
2. `TestController.cs` 应移除
3. 测试数据中手机号格式不正确（12位）
4. 密码配置应外部化
5. ~~缺少 Application 层和 Infrastructure 层~~ ✅ Infrastructure 已创建，Application 待创建

## 当前实施进度

### ✅ 已完成
- 创建 `TcmAiDiagnosis.Infrastructure` 项目
- 迁移 `TcmAiDiagnosisContext` 到 Infrastructure/Persistence/DbContext
- 创建 `PatientRepository` 实现
- 创建 `Domain/Interfaces/IPatientRepository` 接口
- 创建 `Domain/Entities/Patient.cs` 实体
- 更新 `Program.cs` 注册仓储服务
- 清理重复的 `AddDbContext` 注册
- 更新项目依赖关系

### 🔄 进行中
- 统一实体定义（Entities vs Domain/Entities）
- 创建其他仓储接口和实现

### ⏳ 待完成
- 创建 `TcmAiDiagnosis.Application` 项目
- 创建 DTOs 和应用服务
- 创建 ViewModels
- 重构 Razor Pages 使用新的应用服务
- 添加全局异常处理中间件

## 技术选型详情

### 后端技术栈
| 技术 | 版本 | 用途 |
|------|------|------|
| .NET | 8.0 | 后端框架 |
| Entity Framework Core | 8.0 | ORM |
| MySQL | 8.0 | 数据库 |
| Redis | 7.0 | 缓存 |
| ASP.NET Core Identity | - | 用户认证 |
| Docker | - | 容器化部署 |

### AI技术栈
| 技术 | 用途 |
|------|------|
| Dify | 工作流平台 |
| TensorFlow.NET | 机器学习 |
| ML.NET | .NET机器学习 |
| BERT | 自然语言处理 |

### 前端技术栈
| 技术 | 用途 |
|------|------|
| Razor Pages | SSR页面 |
| Bootstrap 5 | UI框架 |
| jQuery | JS库 |
| PlainAdmin | 管理模板 |

## 配伍禁忌系统

### 已实现的配伍禁忌数据

#### 十八反（18组配伍禁忌）
- 甘草反：甘遂、大戟、海藻、芫花
- 乌头反：贝母、瓜蒌、半夏、白蔹、白及
- 藜芦反：人参、沙参、丹参、玄参、细辛、芍药

#### 十九畏（10组配伍禁忌）
- 硫黄畏朴硝、水银畏砒霜、狼毒畏密陀僧
- 巴豆畏牵牛、丁香畏郁金、牙硝畏三棱
- 川乌/草乌畏犀角、人参畏五灵脂、官桂畏石脂

### 配伍禁忌检查流程
1. 处方创建时自动检查配伍禁忌
2. 发现禁忌时显示风险警告
3. 医生确认后方可继续
4. 记录风险确认日志

## 相关文档索引

### 项目文档
- `docs/01-项目启动阶段/项目章程.md` - 项目目标和范围
- `docs/02-需求分析阶段/需求规格说明书.md` - 详细需求
- `docs/03-系统设计阶段/系统架构设计说明书.md` - 架构设计
- `docs/03-系统设计阶段/数据库设计文档.md` - 数据库设计
- `docs/04-开发实现阶段/编码规范.md` - 编码规范

### 权限系统文档
- `docs/权限管理-快速参考.md` - 权限系统快速入门
- `docs/权限管理-角色权限对照表.md` - 角色权限详情
- `docs/权限中间件-实施完成报告.md` - 中间件实现

### AI配置文档
- `docs/DifyApi配置说明.md` - Dify API配置
- `docs/AI提示词/证候概览.md` - 证候概览提示词
- `docs/AI提示词/证候详情.md` - 证候详情提示词
- `docs/AI提示词/治疗方案生成.md` - 治疗方案提示词

### 架构文档
- `docs/分层架构说明.md` - 四层架构详细说明
- `docs/企业级架构迁移步骤.md` - 架构迁移实施指南

### 数据扩展文档
- `docs/配伍禁忌数据扩展指南.md` - 配伍禁忌数据扩展
- `docs/样式使用指南.md` - UI样式使用指南
- `docs/页面设计规范.md` - 页面设计规范

### 开发文档
- `docs/开发文档/TreatmentDomain实现说明.md` - 治疗方案领域服务实现
- `docs/开发计划/治疗方案/` - 治疗方案模块开发计划

---

## 角色体系设计（重要）

### 角色定义

| 角色 | 英文名 | 职责 | 权限数 |
|------|--------|------|--------|
| 平台管理员 | PlatformAdmin | 管理整个SaaS平台 | 待定 |
| 租户管理员 | TenantAdmin | 管理单个诊所 | 待定 |
| 医生 | Doctor | 核心业务角色，诊疗执行者 | 56 |
| 患者 | Patient | 服务对象，查看个人信息 | 10 |
| 药剂师 | Pharmacist | 辅助角色（可选） | 6 |

### 每个角色的功能清单

#### 1. 平台管理员（PlatformAdmin）

**职责**：管理整个SaaS平台

**功能清单**：
```
✅ 租户管理
   - 创建/禁用租户
   - 查看租户列表
   - 管理租户套餐

✅ 系统监控
   - 查看系统使用统计
   - 监控API调用情况
   - 查看系统日志

✅ 全局配置
   - 系统参数设置
   - 功能开关控制
```

**不做的事情**：
- ❌ 不管理具体患者
- ❌ 不参与诊疗流程
- ❌ 不操作业务数据

---

#### 2. 租户管理员（TenantAdmin）

**职责**：管理单个诊所/医疗机构

**功能清单**：
```
✅ 医生管理
   - 创建医生账号
   - 编辑医生信息
   - 启用/禁用医生

✅ 诊所配置
   - 修改诊所信息
   - 配置功能模块
   - 设置资源配额

✅ 数据查看
   - 查看本诊所统计
   - 查看操作日志
   - 导出数据报表
```

**不做的事情**：
- ❌ 不管理患者（医生管理）
- ❌ 不做诊疗（医生做）
- ❌ 不管其他诊所

---

#### 3. 医生（Doctor）⭐ 核心角色

**职责**：诊疗业务的主要执行者

**功能清单**：
```
✅ 患者管理
   - 创建患者档案
   - 编辑患者信息
   - 查看患者列表

✅ 诊疗管理
   - 创建就诊记录
   - 采集四诊信息
   - 使用AI诊断

✅ 治疗方案
   - 创建治疗方案
   - 开具中药处方
   - 推荐针灸/艾灸
   - 提供生活建议

✅ 病历管理
   - 生成AI医案
   - 编辑病历
   - 打印病历
```

**核心工作流程**：
```
医生工作流程：
1. 查看今日预约
2. 接诊患者
3. 录入四诊信息
4. 使用AI辅助诊断
5. 确认/修改诊断
6. 创建治疗方案
7. 开具处方
8. 生成病历
9. 结束就诊
```

---

#### 4. 患者（Patient）

**职责**：查看自己的健康信息

**功能清单**：
```
✅ 个人信息
   - 查看个人档案
   - 编辑联系方式

✅ 就诊记录
   - 查看就诊历史
   - 查看诊断结果

✅ 治疗方案
   - 查看当前方案
   - 查看用药指导

✅ 预约管理
   - 查看预约记录
   - 取消预约（未来）
```

**不做的事情**：
- ❌ 不能创建其他患者
- ❌ 不能做诊断
- ❌ 不能开处方
- ❌ 不能看其他患者

---

#### 5. 药剂师（Pharmacist）- 可选角色

**职责**：药品管理和处方审核

**功能清单**：
```
✅ 处方审核
   - 查看待审核处方
   - 审核处方合理性
   - 标记问题处方

✅ 药品知识
   - 查看中药信息
   - 查看配伍禁忌
   - 查看药品库存
```

**注意**：这个角色可以先不实现，等核心功能完成后再考虑

---

### 角色-功能矩阵

```
功能模块          | 平台管理员 | 租户管理员 | 医生 | 患者 | 药剂师
-----------------|-----------|-----------|------|------|--------
租户管理          |    ✅     |     ❌    |  ❌  |  ❌  |   ❌
医生管理          |    ❌     |     ✅    |  ❌  |  ❌  |   ❌
患者管理          |    ❌     |     ❌    |  ✅  |  ❌  |   ❌
就诊管理          |    ❌     |     ❌    |  ✅  |  ❌  |   ❌
治疗方案          |    ❌     |     ❌    |  ✅  |  ❌  |   ❌
病历查看          |    ❌     |     ❌    |  ✅  |  ✅  |   ❌
处方查看          |    ❌     |     ❌    |  ✅  |  ✅  |   ✅
处方审核          |    ❌     |     ❌    |  ❌  |  ❌  |   ✅
系统监控          |    ✅     |     ✅    |  ❌  |  ❌  |   ❌
知识库查看        |    ✅     |     ✅    |  ✅  |  ❌  |   ✅
```

---

## 功能开发优先级

### 优先级1：医生角色（核心业务）- 第1-4周

**为什么先做医生？**
- 医生是系统的核心用户
- 医生功能最复杂，最能体现技术能力
- 医生功能完成后，其他角色都依赖它

**开发顺序**：
```
第1周：医生-患者管理
├── 创建患者档案页面
├── 患者列表页面
├── 患者详情页面
└── 患者编辑功能

第2周：医生-就诊管理
├── 创建就诊记录
├── 四诊信息采集表单
├── 就诊历史列表
└── 就诊详情页面

第3周：医生-AI诊断
├── 调用Dify工作流
├── 显示证候分析结果
├── 确认/修改诊断
└── 保存诊断结果

第4周：医生-治疗方案
├── 创建治疗方案
├── 中药处方管理
├── 针灸/艾灸推荐
├── 配伍禁忌检查
└── 生活建议
```

---

### 优先级2：患者角色（服务对象）- 第5-6周

**为什么第二做患者？**
- 患者功能相对简单
- 患者是医生服务的对象
- 完成后可以形成完整闭环

**开发顺序**：
```
第5周：患者-个人中心
├── 查看个人信息
├── 编辑联系方式
├── 查看就诊历史
└── 查看治疗方案

第6周：患者-就诊记录
├── 就诊列表
├── 就诊详情
├── 诊断结果
└── 病历查看
```

---

### 优先级3：租户管理员（管理功能）- 第7周

**为什么第三做？**
- 管理功能相对独立
- 不影响核心业务流程
- 可以后期完善

**开发顺序**：
```
第7周：租户管理员
├── 医生管理
├── 诊所配置
└── 数据统计
```

---

### 优先级4：平台管理员（运维功能）- 第8周

**为什么最后做？**
- 运维功能不是核心业务
- 可以用其他工具替代
- 可以后期完善

**开发顺序**：
```
第8周：平台管理员
├── 租户列表
├── 系统监控
└── 全局配置
```

---

### 优先级5：药剂师（可选角色）- 第9周+

**什么时候做？**
- 等核心功能完成后再考虑
- 可以作为扩展功能
- 可以先用医生角色代替

---

## 每个角色的页面清单

### 医生端页面（最重要）

```
Web/Areas/Doctor/Pages/
├── Patients/
│   ├── Index.cshtml          # 患者列表
│   ├── Create.cshtml         # 创建患者
│   ├── Detail.cshtml         # 患者详情
│   └── Edit.cshtml           # 编辑患者
├── Visits/
│   ├── Index.cshtml          # 就诊列表
│   ├── Create.cshtml         # 创建就诊
│   ├── Detail.cshtml         # 就诊详情
│   ├── Syndrome.cshtml       # 证候分析
│   └── Treatment.cshtml      # 治疗方案
└── VisitSeries/
    ├── Index.cshtml          # 就诊系列列表
    └── Detail.cshtml         # 系列详情
```

### 患者端页面

```
Web/Areas/Patient/Pages/
├── Profile/
│   ├── Index.cshtml          # 个人信息
│   └── Edit.cshtml           # 编辑信息
├── Visits/
│   ├── Index.cshtml          # 就诊历史
│   └── Detail.cshtml         # 就诊详情
└── Treatments/
    └── Index.cshtml          # 治疗方案
```

### 租户管理端页面

```
Web/Areas/Manager/Pages/
├── Doctors/
│   ├── Index.cshtml          # 医生列表
│   ├── Create.cshtml         # 创建医生
│   └── Edit.cshtml           # 编辑医生
├── Settings/
│   └── Index.cshtml          # 诊所设置
└── Reports/
    └── Index.cshtml          # 数据统计
```

---

## 项目评估（2026-07-12）

### 项目评分
- **文档完整度**: 9/10 ⭐⭐⭐⭐⭐
- **架构设计**: 8/10 ⭐⭐⭐⭐
- **代码质量**: 6/10 ⭐⭐⭐
- **实施进度**: 6/10 ⭐⭐⭐
- **综合评分**: 7/10 ⭐⭐⭐⭐

### 项目亮点
1. **文档驱动开发** - 完善的文档体系（63个文档）
2. **权限系统完善** - 110个细粒度权限点
3. **AI集成创新** - Dify工作流集成
4. **架构设计清晰** - 四层架构+DDD
5. **业务逻辑完整** - 覆盖中医诊疗全流程

### 待解决问题
1. Patient实体存在两个版本（需要统一）
2. 缺少Application层（需要创建）
3. 命名空间不一致（需要规范）
4. 缺少单元测试（需要补充）

---

## 学习路径建议

### 阶段一：基础巩固（1-2周）

**学习内容**：
- ASP.NET Core 基础（中间件、依赖注入、配置）
- Razor Pages 基础（页面模型、路由、表单处理）
- Entity Framework Core 基础（DbContext、迁移、查询）

**实践任务**：
```bash
# 1. 完成患者管理的CRUD
- 创建患者列表页面
- 实现患者详情页面
- 完成患者创建/编辑功能
- 添加删除确认对话框

# 2. 完成就诊管理
- 创建就诊记录
- 实现四诊信息采集表单
- 保存就诊信息到数据库
```

---

### 阶段二：进阶提升（2-3周）

**学习内容**：
- 依赖注入深入（生命周期、注册方式）
- 中间件开发（自定义中间件）
- 认证与授权（Identity、策略授权）

**实践任务**：
```bash
# 1. 完善权限系统
- 理解现有的权限中间件
- 添加新的权限点
- 实现权限缓存

# 2. 实现全局异常处理
- 创建 GlobalExceptionMiddleware
- 统一错误返回格式
- 添加日志记录
```

---

### 阶段三：架构深入（3-4周）

**学习内容**：
- DDD领域驱动设计
- 仓储模式
- 规约模式

**实践任务**：
```bash
# 1. 创建Application层
- 定义DTOs
- 创建应用服务
- 实现AutoMapper映射

# 2. 完善Infrastructure层
- 创建所有仓储实现
- 集成Redis缓存
- 实现数据访问优化
```

---

### 阶段四：AI集成（2-3周）

**学习内容**：
- HTTP客户端调用（HttpClient）
- JSON序列化/反序列化
- 异步编程

**实践任务**：
```bash
# 1. 理解Dify API调用
- 阅读现有代码
- 理解工作流调用方式

# 2. 实现新的AI功能
- 证候分析
- 治疗方案推荐
- 知识库查询
```

---

## 简历项目建议

### 项目描述模板

```markdown
## 项目名称：本草智诊-中医证候分析系统

**项目描述**：
基于.NET 8的智能化中医SaaS诊疗平台，为中小型中医诊所提供完整的数字化诊疗解决方案。

**技术架构**：
- 采用四层架构设计（Presentation/Application/Domain/Infrastructure）
- 基于ASP.NET Core Identity实现用户认证
- 使用Entity Framework Core进行数据访问
- 集成Dify工作流平台实现AI辅助诊断

**核心功能**：
- 多租户管理：支持多个医疗机构独立使用，数据完全隔离
- 权限管理：基于RBAC的细粒度权限控制（110+权限点，4种角色）
- 患者管理：完整的患者档案管理，支持跨租户信息共享
- 诊疗管理：四诊信息结构化采集（望、闻、问、切）
- AI辅助诊断：基于证候分析的智能诊断建议
- 治疗方案：智能治疗方案生成，支持配伍禁忌检查

**个人职责**：
- 负责后端架构设计和核心业务开发
- 实现权限验证中间件和数据初始化
- 集成AI工作流实现智能诊断功能
- 设计并实现配伍禁忌检查系统

**技术难点**：
- 多租户数据隔离方案设计
- 细粒度权限控制实现
- AI工作流集成与优化
- 中医配伍禁忌规则引擎
```

---

## 关键技术点（面试准备）

### 1. 为什么选择四层架构？
```
答：四层架构实现了关注点分离，便于测试和维护。
表现层只负责UI，应用层编排业务流程，
领域层封装核心逻辑，基础设施层处理技术细节。
依赖倒置原则使得核心业务不依赖外部框架。
```

### 2. 如何实现多租户数据隔离？
```
答：采用共享数据库方案，通过TenantId字段隔离数据。
在仓储层自动过滤当前租户数据，
中间件中解析租户信息并注入到请求上下文。
```

### 3. 权限系统是如何设计的？
```
答：基于RBAC模型，定义了110个细粒度权限点。
使用自定义中间件在请求管道中验证权限，
支持临时权限授予和权限缓存。
权限代码采用模块.操作的命名规范。
```

### 4. 如何集成AI工作流？
```
答：通过HttpClient调用Dify API，
支持阻塞和流式两种响应模式。
定义了结构化的输入输出格式，
实现了超时控制和错误处理。
```

---

## 下次继续开发的内容

### 待办事项清单

1. **解决Patient实体重复问题**
   - 删除 `Domain/Entities/Patient.cs`
   - 统一使用 `Entities/Patient.cs`
   - 更新所有引用

2. **清理Program.cs重复注册**
   - 移除重复的 `AddDbContext`
   - 使用扩展方法组织服务注册

3. **创建Application层**
   - 创建项目和目录结构
   - 创建DTOs
   - 创建应用服务接口

4. **完善医生端患者管理功能**
   - 创建患者列表页面
   - 实现患者CRUD
   - 这是最基础的功能

5. **完成就诊管理**
   - 创建就诊记录
   - 四诊信息采集
   - 这是核心业务流程

---

## 开发规范提醒

### 命名规范
- 类名：帕斯卡命名法（`PatientService`）
- 方法名：帕斯卡命名法（`GetPatientAsync`）
- 变量名：驼峰命名法（`patientInfo`）
- 私有字段：下划线前缀（`_context`）
- 接口名：I前缀（`IPatientRepository`）

### 注释规范
- 使用中文注释
- 公共方法必须有XML注释
- 复杂业务逻辑要有行内注释

### 代码质量
- 每个公共方法要有XML注释
- 异常处理要统一
- 避免重复代码
- 数据库查询要分页
