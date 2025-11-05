const API_BASE = '/api';
let accounts = [];
let updateInterval = null;
let isEditMode = false;
let editingAccountName = null;

// 初始化
document.addEventListener('DOMContentLoaded', () => {
    loadAccounts();
    setupEventListeners();
    startAutoUpdate();
});

// 設定事件監聽器
function setupEventListeners() {
    document.getElementById('addButton').addEventListener('click', showAddModal);
    document.getElementById('cancelButton').addEventListener('click', hideModal);
    document.getElementById('accountForm').addEventListener('submit', handleSubmit);
    document.getElementById('searchInput').addEventListener('input', handleSearch);
    document.getElementById('importQRButton').addEventListener('click', () => {
        document.getElementById('qrCodeFile').click();
    });
    document.getElementById('qrCodeFile').addEventListener('change', handleQRCodeImport);
    document.getElementById('importGoogleButton').addEventListener('click', () => {
        document.getElementById('googleAuthFile').click();
    });
    document.getElementById('googleAuthFile').addEventListener('change', handleGoogleAuthImport);

    // 點擊模態框外部關閉
    document.getElementById('modal').addEventListener('click', (e) => {
        if (e.target.id === 'modal') {
            hideModal();
        }
    });
}

// 載入所有帳戶
async function loadAccounts() {
    try {
        const response = await fetch(`${API_BASE}/accounts`);
        if (response.ok) {
            accounts = await response.json();
            renderAccounts();
            updateAllTotp();
        }
    } catch (error) {
        console.error('載入帳戶失敗:', error);
    }
}

// 渲染帳戶列表
function renderAccounts(filteredAccounts = null) {
    const accountList = document.getElementById('accountList');
    const displayAccounts = filteredAccounts || accounts;

    if (displayAccounts.length === 0) {
        accountList.innerHTML = `
            <div class="empty-state">
                <h3>尚未有任何帳戶</h3>
                <p>點擊「+ 新增」按鈕開始新增您的第一個 TOTP 帳戶</p>
            </div>
        `;
        return;
    }

    accountList.innerHTML = displayAccounts.map(account => createAccountCard(account)).join('');

    // 綁定事件
    displayAccounts.forEach(account => {
        const card = accountList.querySelector(`[data-name="${account.name}"]`);
        card.querySelector('.totp-code').addEventListener('click', () => copyToClipboard(account.name, 'totp'));
        if (account.customString) {
            card.querySelector('.custom-string').addEventListener('click', () => copyToClipboard(account.name, 'custom'));
        }
        card.querySelector('.btn-edit').addEventListener('click', () => showEditModal(account));
        card.querySelector('.btn-delete').addEventListener('click', () => deleteAccount(account.name));
    });
}

// 建立帳戶卡片
function createAccountCard(account) {
    const displayName = account.issuer ? `${account.issuer} - ${account.name}` : account.name;
    const customStringHtml = account.customString ?
        `<div class="custom-string">${account.customString}</div>` : '';

    return `
        <div class="account-card" data-name="${account.name}">
            <div class="countdown-circle" data-name="${account.name}">
                <svg width="60" height="60">
                    <circle class="bg" cx="30" cy="30" r="26"></circle>
                    <circle class="progress" cx="30" cy="30" r="26"
                            stroke-dasharray="163.36" stroke-dashoffset="0"></circle>
                </svg>
                <div class="countdown-text">30s</div>
            </div>
            <div class="account-info">
                <div class="account-name">${displayName}</div>
                <div class="totp-code" data-name="${account.name}">------</div>
                ${customStringHtml}
            </div>
            <div class="account-actions">
                <button class="btn-edit">編輯</button>
                <button class="btn-delete">刪除</button>
            </div>
        </div>
    `;
}

// 更新所有 TOTP
async function updateAllTotp() {
    try {
        const timeResponse = await fetch(`${API_BASE}/time`);
        if (!timeResponse.ok) return;

        const { remainingSeconds } = await timeResponse.json();

        for (const account of accounts) {
            const response = await fetch(`${API_BASE}/totp/${encodeURIComponent(account.name)}`);
            if (response.ok) {
                const { totp } = await response.json();
                updateTotpDisplay(account.name, totp, remainingSeconds);
            }
        }
    } catch (error) {
        console.error('更新 TOTP 失敗:', error);
    }
}

// 更新 TOTP 顯示
function updateTotpDisplay(accountName, totp, remainingSeconds) {
    const card = document.querySelector(`.account-card[data-name="${accountName}"]`);
    if (!card) return;

    const totpCode = card.querySelector('.totp-code');
    const countdownCircle = card.querySelector('.countdown-circle');
    const countdownText = countdownCircle.querySelector('.countdown-text');
    const progress = countdownCircle.querySelector('.progress');

    // 更新 TOTP 碼
    totpCode.textContent = totp;

    // 更新倒數計時
    countdownText.textContent = `${remainingSeconds}s`;

    // 更新進度圓圈
    const circumference = 163.36;
    const offset = circumference * (1 - remainingSeconds / 30);
    progress.style.strokeDashoffset = offset;

    // 少於 5 秒時顯示警告色
    if (remainingSeconds <= 5) {
        countdownCircle.classList.add('warning');
    } else {
        countdownCircle.classList.remove('warning');
    }
}

// 自動更新
function startAutoUpdate() {
    updateInterval = setInterval(() => {
        updateAllTotp();
    }, 1000);
}

// 複製到剪貼簿
async function copyToClipboard(accountName, type) {
    const card = document.querySelector(`.account-card[data-name="${accountName}"]`);
    let text = '';
    let element = null;

    if (type === 'totp') {
        element = card.querySelector('.totp-code');
        text = element.textContent;
    } else {
        element = card.querySelector('.custom-string');
        text = element.textContent;
    }

    try {
        await navigator.clipboard.writeText(text);
        const originalText = element.textContent;
        element.textContent = '已複製!';
        element.style.color = '#10b981';
        setTimeout(() => {
            element.textContent = originalText;
            element.style.color = '';
        }, 1000);
    } catch (error) {
        console.error('複製失敗:', error);
    }
}

// 顯示新增模態框
function showAddModal() {
    isEditMode = false;
    editingAccountName = null;
    document.getElementById('modalTitle').textContent = '新增帳戶';
    document.getElementById('accountForm').reset();
    document.getElementById('secret').disabled = false;
    document.getElementById('modal').classList.add('show');
}

// 顯示編輯模態框
function showEditModal(account) {
    isEditMode = true;
    editingAccountName = account.name;
    document.getElementById('modalTitle').textContent = '編輯帳戶';
    document.getElementById('name').value = account.name;
    document.getElementById('secret').value = account.secret;
    document.getElementById('secret').disabled = true;
    document.getElementById('issuer').value = account.issuer || '';
    document.getElementById('customString').value = account.customString || '';
    document.getElementById('modal').classList.add('show');
}

// 隱藏模態框
function hideModal() {
    document.getElementById('modal').classList.remove('show');
    document.getElementById('accountForm').reset();
    document.getElementById('secret').disabled = false;
}

// 處理表單提交
async function handleSubmit(e) {
    e.preventDefault();

    const account = {
        name: document.getElementById('name').value.trim(),
        secret: document.getElementById('secret').value.trim().replace(/\s/g, ''),
        issuer: document.getElementById('issuer').value.trim() || null,
        customString: document.getElementById('customString').value.trim() || null
    };

    try {
        if (isEditMode) {
            await updateAccount(editingAccountName, account);
        } else {
            await addAccount(account);
        }
        hideModal();
        await loadAccounts();
    } catch (error) {
        alert(error.message);
    }
}

// 新增帳戶
async function addAccount(account) {
    const response = await fetch(`${API_BASE}/accounts`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(account)
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.error || '新增帳戶失敗');
    }
}

// 更新帳戶
async function updateAccount(originalName, account) {
    const response = await fetch(`${API_BASE}/accounts/${encodeURIComponent(originalName)}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(account)
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.error || '更新帳戶失敗');
    }
}

// 刪除帳戶
async function deleteAccount(name) {
    if (!confirm(`確定要刪除帳戶「${name}」嗎？`)) {
        return;
    }

    try {
        const response = await fetch(`${API_BASE}/accounts/${encodeURIComponent(name)}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            await loadAccounts();
        } else {
            const error = await response.json();
            alert(error.error || '刪除帳戶失敗');
        }
    } catch (error) {
        alert('刪除帳戶失敗: ' + error.message);
    }
}

// 搜尋處理
function handleSearch(e) {
    const searchTerm = e.target.value.toLowerCase();

    if (!searchTerm) {
        renderAccounts();
        return;
    }

    const filtered = accounts.filter(account =>
        account.name.toLowerCase().includes(searchTerm) ||
        (account.issuer && account.issuer.toLowerCase().includes(searchTerm))
    );

    renderAccounts(filtered);
}

// 處理 Google 驗證器匯入
async function handleGoogleAuthImport(e) {
    const file = e.target.files[0];
    if (!file) return;

    // 驗證檔案類型
    if (!file.type.startsWith('image/')) {
        alert('請選擇圖片檔案');
        e.target.value = '';
        return;
    }

    try {
        const formData = new FormData();
        formData.append('file', file);

        // 顯示載入提示
        const importButton = document.getElementById('importGoogleButton');
        const originalText = importButton.textContent;
        importButton.textContent = '處理中...';
        importButton.disabled = true;

        const response = await fetch(`${API_BASE}/import/google-authenticator`, {
            method: 'POST',
            body: formData
        });

        const result = await response.json();

        if (response.ok) {
            if (result.addedCount > 0) {
                alert(`成功匯入 ${result.addedCount} 個帳戶！`);
                await loadAccounts();
            } else {
                alert(result.message || '圖片中未找到任何 OTP 帳戶資料');
            }
        } else {
            alert(result.error || '匯入失敗');
        }

        // 恢復按鈕狀態
        importButton.textContent = originalText;
        importButton.disabled = false;

    } catch (error) {
        alert('匯入時發生錯誤: ' + error.message);

        // 恢復按鈕狀態
        const importButton = document.getElementById('importGoogleButton');
        importButton.textContent = '📱 Google 驗證器';
        importButton.disabled = false;
    }

    // 清空檔案選擇
    e.target.value = '';
}

// 處理 QR Code 圖片匯入
async function handleQRCodeImport(e) {
    const file = e.target.files[0];
    if (!file) return;

    // 驗證檔案類型
    if (!file.type.startsWith('image/')) {
        alert('請選擇圖片檔案');
        e.target.value = '';
        return;
    }

    try {
        // 讀取圖片
        const imageData = await readImageFile(file);

        // 使用 jsQR 解析 QR Code
        const code = jsQR(imageData.data, imageData.width, imageData.height);

        if (!code || !code.data) {
            alert('無法從圖片中讀取 QR Code，請確認圖片包含有效的 QR Code');
            e.target.value = '';
            return;
        }

        // 解析 otpauth:// URI
        const uri = code.data;
        if (!uri.startsWith('otpauth://totp/')) {
            alert('QR Code 不是有效的 TOTP 格式');
            e.target.value = '';
            return;
        }

        try {
            const url = new URL(uri);
            const label = decodeURIComponent(url.pathname.substring(1)); // 移除開頭的 '/'
            const secret = url.searchParams.get('secret');
            const issuer = url.searchParams.get('issuer');

            if (!secret) {
                alert('QR Code 中未找到 Secret Key');
                e.target.value = '';
                return;
            }

            // 建立帳戶物件
            const account = {
                name: label,
                secret: secret,
                issuer: issuer || null,
                customString: null
            };

            // 呼叫 API 新增帳戶
            await addAccount(account);
            alert('成功從 QR Code 新增帳戶！');
            await loadAccounts();

        } catch (parseError) {
            alert('解析 QR Code 資料時發生錯誤: ' + parseError.message);
        }

    } catch (error) {
        alert('讀取 QR Code 時發生錯誤: ' + error.message);
    }

    // 清空檔案選擇
    e.target.value = '';
}

// 讀取圖片檔案並轉換為 ImageData
function readImageFile(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onload = (e) => {
            const img = new Image();

            img.onload = () => {
                // 建立 canvas 來取得圖片資料
                const canvas = document.createElement('canvas');
                canvas.width = img.width;
                canvas.height = img.height;

                const ctx = canvas.getContext('2d');
                ctx.drawImage(img, 0, 0);

                const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
                resolve(imageData);
            };

            img.onerror = () => {
                reject(new Error('無法載入圖片'));
            };

            img.src = e.target.result;
        };

        reader.onerror = () => {
            reject(new Error('無法讀取檔案'));
        };

        reader.readAsDataURL(file);
    });
}
