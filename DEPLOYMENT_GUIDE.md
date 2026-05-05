# 计分器小程序云端上线指南

## 一、项目概述

本项目包含：
- **后端**: .NET Core 8.0 + SQLite数据库
- **前端**: 微信小程序

## 二、部署方式

### 方式一：Docker部署（推荐）

#### 1. 准备服务器
- 安装 Docker 和 Docker Compose
- 开放端口 5000（或自定义端口）

#### 2. 部署命令
```bash
# 克隆项目
git clone <项目仓库地址>
cd <项目目录>

# 创建数据目录
mkdir -p data

# 启动服务
docker-compose up -d
```

#### 3. 验证服务
```bash
curl http://localhost:5000/api/score
```

### 方式二：Linux服务器直接部署

#### 1. 安装依赖
```bash
# 安装 .NET 8.0 SDK
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.100

# 设置环境变量
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools
```

#### 2. 构建项目
```bash
cd backend-csharp
dotnet publish -c Release -o publish
```

#### 3. 运行服务
```bash
# 方式1：直接运行
dotnet publish/backend-csharp.dll --urls=http://*:5000

# 方式2：使用 systemd 服务（推荐）
chmod +x deploy.sh
./deploy.sh
```

## 三、域名与SSL配置

### 1. 配置域名
- 在域名服务商处添加 A 记录指向服务器IP
- 示例：`api.yourdomain.com -> 服务器IP`

### 2. 配置 Nginx 反向代理

创建 `/etc/nginx/sites-available/score-api`：

```nginx
server {
    listen 80;
    server_name api.yourdomain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

启用配置：
```bash
ln -s /etc/nginx/sites-available/score-api /etc/nginx/sites-enabled/
nginx -t
systemctl reload nginx
```

### 3. 配置 HTTPS（Let's Encrypt）
```bash
# 安装 Certbot
apt-get update && apt-get install certbot python3-certbot-nginx -y

# 获取证书
certbot --nginx -d api.yourdomain.com
```

## 四、微信小程序配置

### 1. 配置服务器域名
登录 [微信公众平台](https://mp.weixin.qq.com) -> 开发 -> 开发设置：

- **request 合法域名**: `https://api.yourdomain.com`
- **socket 合法域名**: （如不需要WebSocket可留空）

### 2. 更新小程序API地址

修改 `wechat-score/utils/api.js`：

```javascript
const BASE_URL = 'https://api.yourdomain.com';
```

### 3. 上传小程序代码
- 使用微信开发者工具打开 `wechat-score` 目录
- 点击"上传"按钮
- 填写版本号和项目备注

## 五、数据库迁移（可选）

### SQLite 数据库位置
- 默认路径：`backend-csharp/score.db`
- Docker部署：`./data/score.db`

### 数据备份
```bash
# 备份
cp score.db score.db.backup

# 恢复
cp score.db.backup score.db
```

## 六、监控与日志

### 查看服务状态
```bash
# Docker方式
docker-compose logs -f

# systemd方式
systemctl status score-counter
journalctl -u score-counter -f
```

### 常见问题

| 问题 | 解决方案 |
|------|----------|
| 服务启动失败 | 检查端口是否被占用、查看日志 |
| 数据库权限问题 | 确保 `www-data` 用户有读写权限 |
| 小程序无法调用API | 检查域名配置、HTTPS证书、CORS设置 |

## 七、安全建议

1. ✅ 使用 HTTPS 加密传输
2. ✅ 限制 API 访问频率（可选添加限流中间件）
3. ✅ 定期备份数据库
4. ✅ 隐藏 Swagger 文档（生产环境）
5. ✅ 配置防火墙规则

## 八、上线检查清单

- [ ] 后端服务正常运行
- [ ] API接口测试通过
- [ ] 域名解析配置正确
- [ ] HTTPS证书安装完成
- [ ] 微信小程序域名配置完成
- [ ] 小程序代码上传并提交审核
- [ ] 数据库备份策略已制定