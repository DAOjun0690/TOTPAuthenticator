# TOTPAuthenticator

TOTPAuthenticator 是一個基於 Windows Forms 的桌面應用程式，用於生成和管理基於時間的一次性密碼 (TOTP)。它提供了一個現代化的使用者介面，支援手動新增帳戶、從 QR Code 檔案匯入，並允許使用者自訂和編輯每個驗證碼的額外資訊。

## 主要功能

*   **TOTP 生成**：根據帳戶密鑰生成標準的 TOTP 驗證碼。
*   **帳戶管理**：
    *   **手動新增**：允許使用者手動輸入帳戶名稱、密鑰和自訂字串。
    *   **QR Code 匯入**：支援從 QR Code 圖片檔案中讀取帳戶資訊（名稱、密鑰、發行者）。
    *   **編輯自訂字串**：為每個帳戶提供一個可編輯的自訂字串欄位。
    *   **刪除帳戶**：從列表中移除不再需要的帳戶。
*   **使用者介面**：
    *   **現代化設計**：採用深色主題和卡片式佈局，提供清晰直觀的視覺體驗。
    *   **即時倒數**：每個驗證碼卡片上顯示一個圓形倒數計時器，指示當前 TOTP 的剩餘有效時間。
    *   **搜尋功能**：快速篩選和查找特定帳戶。
    *   **一鍵複製**：點擊 TOTP 驗證碼或自訂字串即可複製到剪貼簿。
*   **資料持久化**：帳戶資料會自動保存到 `accounts.json` 檔案中，確保應用程式重啟後資料不會丟失。

## 專案結構

```
TOTPAuthenticator/
├───.gitattributes
├───.gitignore
├───accounts.json             # 帳戶資料儲存檔案
├───Form1.cs                  # 主視窗的程式碼
├───Form1.Designer.cs         # 主視窗的 UI 設計器程式碼
├───Form1.resx                # 主視窗的資源檔
├───ManualAddForm.cs          # 手動新增帳戶視窗的程式碼
├───ManualAddForm.Designer.cs # 手動新增帳戶視窗的 UI 設計器程式碼
├───ManualAddForm.resx        # 手動新增帳戶視窗的資源檔
├───EditCustomStringForm.cs   # 編輯自訂字串視窗的程式碼
├───EditCustomStringForm.Designer.cs # 編輯自訂字串視窗的 UI 設計器程式碼
├───EditCustomStringForm.resx # 編輯自訂字串視窗的資源檔
├───Account.cs                # 帳戶資料模型定義
├───AccountControl.cs         # 自訂帳戶卡片使用者控制項的程式碼
├───AccountControl.Designer.cs# 自訂帳戶卡片使用者控制項的 UI 設計器程式碼
├───AccountControl.resx       # 自訂帳戶卡片使用者控制項的資源檔
├───Program.cs                # 應用程式的進入點
├───README.md                 # 專案說明文件
├───TOTPAuthenticator.csproj  # C# 專案檔
├───TOTPAuthenticator.sln     # Visual Studio 解決方案檔
└───bin/                      # 編譯後的執行檔
└───obj/                      # 編譯中間檔案
```

## 如何建置與執行

1.  **安裝 .NET SDK**：確保您的系統已安裝 .NET 9.0 SDK 或更高版本。您可以從 [Microsoft 官方網站](https://dotnet.microsoft.com/download) 下載。
2.  **複製專案**：
    ```bash
    git clone https://github.com/your-repo/TOTPAuthenticator.git
    cd TOTPAuthenticator
    ```
3.  **建置專案**：
    ```bash
    dotnet build
    ```
4.  **執行應用程式**：
    ```bash
    dotnet run
    ```
    或者，您也可以在 Visual Studio 中開啟 `TOTPAuthenticator.sln` 解決方案並執行。
