# Vue + C# Fullstack Sample

这是一个适合前后端分离项目的样例目录：

```text
Demo/
├─ frontend-vue/          # Vue 3 + Vite 前端
│  ├─ public/
│  └─ src/
│     ├─ assets/
│     ├─ components/
│     ├─ services/
│     └─ views/
├─ backend-csharp/        # ASP.NET Core Web API 后端
│  ├─ Controllers/
│  ├─ Models/
│  └─ Properties/
└─ README.md
```

## 推荐思路

- `frontend-vue` 负责页面、组件、状态管理、接口调用
- `backend-csharp` 负责 API、业务逻辑、数据访问、鉴权
- 本地开发阶段通过 Vite 代理把 `/api` 转发到 C# 后端

## 启动方式

### 1. 启动后端

```powershell
cd backend-csharp
dotnet run
```

默认监听：

- `http://localhost:5075`
- `https://localhost:7197`

测试接口：

```text
GET /api/health
```

### 2. 启动前端

```powershell
cd frontend-vue
npm install
npm run dev
```

默认前端地址：

```text
http://localhost:5173
```

前端里点击按钮会请求后端 `/api/health`，用于演示联调。

## 后续可扩展

- 前端增加 `router/`、`stores/`、`composables/`
- 后端增加 `Services/`、`Repositories/`、`Data/`
- 根目录补充 `docker-compose.yml`
- 增加统一的 `.editorconfig`、CI、测试目录
