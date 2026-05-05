#!/bin/bash

echo "=== 计分器小程序后端部署脚本 ==="
echo ""

APP_NAME="score-counter"
APP_PORT="5000"

echo "1. 清理旧版本..."
rm -rf publish

echo "2. 构建项目..."
dotnet publish -c Release -o publish --no-self-contained

echo "3. 创建服务配置..."
cat > /etc/systemd/system/${APP_NAME}.service << EOF
[Unit]
Description=Score Counter API Service
After=network.target

[Service]
Type=simple
User=www-data
WorkingDirectory=/var/www/${APP_NAME}
ExecStart=/usr/bin/dotnet /var/www/${APP_NAME}/backend-csharp.dll --urls=http://*:${APP_PORT}
Restart=always
RestartSec=10

[Install]
WantedBy=multi-user.target
EOF

echo "4. 复制文件到部署目录..."
mkdir -p /var/www/${APP_NAME}
cp -r publish/* /var/www/${APP_NAME}/

echo "5. 设置权限..."
chown -R www-data:www-data /var/www/${APP_NAME}

echo "6. 启动服务..."
systemctl daemon-reload
systemctl enable ${APP_NAME}
systemctl start ${APP_NAME}

echo ""
echo "=== 部署完成 ==="
echo "服务已启动，监听端口: ${APP_PORT}"
echo "API地址: http://localhost:${APP_PORT}/api/score"