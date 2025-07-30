#!/usr/bin/env pwsh

# TeraLinkaCareApi 建置和發佈腳本
# 作者: AI Assistant
# 日期: $(Get-Date -Format "yyyy-MM-dd")

Write-Host "========================================" -ForegroundColor Green
Write-Host "  TeraLinkaCareApi 建置和發佈腳本" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# 設定變數
$ProjectPath = $PSScriptRoot
$PublishPath = Join-Path $ProjectPath "publish"

Write-Host "專案路徑: $ProjectPath" -ForegroundColor Yellow
Write-Host "發佈路徑: $PublishPath" -ForegroundColor Yellow
Write-Host ""

# 步驟 1: 清理舊的發佈檔案
Write-Host "[步驟 1] 清理舊的發佈檔案..." -ForegroundColor Cyan
if (Test-Path $PublishPath) {
    Remove-Item -Path $PublishPath -Recurse -Force
    Write-Host "✓ 已清理舊的發佈檔案" -ForegroundColor Green
} else {
    Write-Host "✓ 沒有舊的發佈檔案需要清理" -ForegroundColor Green
}
Write-Host ""

# 步驟 2: 建置專案
Write-Host "[步驟 2] 建置專案 (Release 模式)..." -ForegroundColor Cyan
try {
    $buildResult = dotnet build --configuration Release
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ 建置成功！" -ForegroundColor Green
    } else {
        Write-Host "✗ 建置失敗！" -ForegroundColor Red
        Write-Host "錯誤詳情：" -ForegroundColor Red
        Write-Host $buildResult -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ 建置過程中發生錯誤：$($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
Write-Host ""

# 步驟 3: 發佈專案
Write-Host "[步驟 3] 發佈專案..." -ForegroundColor Cyan
try {
    $publishResult = dotnet publish --configuration Release --output ./publish --self-contained false --runtime win-x64
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ 發佈成功！" -ForegroundColor Green
    } else {
        Write-Host "✗ 發佈失敗！" -ForegroundColor Red
        Write-Host "錯誤詳情：" -ForegroundColor Red
        Write-Host $publishResult -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ 發佈過程中發生錯誤：$($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
Write-Host ""

# 步驟 4: 顯示發佈資訊
Write-Host "[步驟 4] 發佈資訊..." -ForegroundColor Cyan
$publishedFiles = Get-ChildItem -Path $PublishPath -File | Measure-Object
$publishedSize = (Get-ChildItem -Path $PublishPath -Recurse -File | Measure-Object -Property Length -Sum).Sum
$publishedSizeMB = [math]::Round($publishedSize / 1MB, 2)

Write-Host "✓ 發佈檔案數量: $($publishedFiles.Count)" -ForegroundColor Green
Write-Host "✓ 發佈檔案大小: $publishedSizeMB MB" -ForegroundColor Green
Write-Host "✓ 發佈路徑: $PublishPath" -ForegroundColor Green
Write-Host ""

# 步驟 5: 開啟發佈資料夾
Write-Host "[步驟 5] 開啟發佈資料夾..." -ForegroundColor Cyan
try {
    if (Test-Path $PublishPath) {
        Start-Process explorer.exe -ArgumentList $PublishPath
        Write-Host "✓ 已開啟發佈資料夾" -ForegroundColor Green
    } else {
        Write-Host "✗ 發佈資料夾不存在：$PublishPath" -ForegroundColor Red
    }
} catch {
    Write-Host "✗ 無法開啟資料夾：$($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# 完成
Write-Host "========================================" -ForegroundColor Green
Write-Host "           建置和發佈完成！" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "接下來的步驟：" -ForegroundColor Yellow
Write-Host "1. 將 publish 資料夾內容複製到 IIS 伺服器" -ForegroundColor Yellow
Write-Host "2. 設定 IIS 應用程式池和網站" -ForegroundColor Yellow
Write-Host "3. 更新生產環境設定檔" -ForegroundColor Yellow
Write-Host ""

# 等待用戶按鍵
Write-Host "按任意鍵繼續..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") 