$dataDir = "d:\AnSinhSo_Enterprise\Data"
$files = Get-ChildItem -Path $dataDir -Filter *.csv

Write-Host "--- LIST OF FILES ---"
foreach ($f in $files) {
    Write-Host "$($f.Name) - $([math]::Round($f.Length / 1KB, 2)) KB"
}

Write-Host "`n--- PARSING DATA ---"
$diaBan = Import-Csv "$dataDir\Stg_DiaBan.csv" -Encoding UTF8
$nhomDT = Import-Csv "$dataDir\Stg_NhomDoiTuong.csv" -Encoding UTF8
$chinhSach = Import-Csv "$dataDir\Stg_ChinhSachTroCap.csv" -Encoding UTF8
$hoGiaDinh = Import-Csv "$dataDir\Stg_HoGiaDinh.csv" -Encoding UTF8
$lichSuHGD = Import-Csv "$dataDir\Stg_LichSuHoGiaDinh.csv" -Encoding UTF8
$thanhVienHGD = Import-Csv "$dataDir\Stg_ThanhVienHoGiaDinh.csv" -Encoding UTF8
$doiTuong = Import-Csv "$dataDir\Stg_DoiTuongAnSinh.csv" -Encoding UTF8
$dotChiTra = Import-Csv "$dataDir\Stg_DotChiTra.csv" -Encoding UTF8
$chiTra = Import-Csv "$dataDir\Stg_ChiTraTroCap.csv" -Encoding UTF8
$roles = Import-Csv "$dataDir\Stg_Roles.csv" -Encoding UTF8
$users = Import-Csv "$dataDir\Stg_Users.csv" -Encoding UTF8
$loiDuLieu = Import-Csv "$dataDir\Stg_LoiDuLieu.csv" -Encoding UTF8

Write-Host "Data parsed."

Write-Host "`n--- CHECKING DUPLICATE PRIMARY KEYS ---"
function Check-Duplicates($data, $keyColumn, $tableName) {
    if (-not $data) { return }
    $duplicates = $data | Group-Object $keyColumn | Where-Object Count -gt 1
    if ($duplicates.Count -gt 0) {
        Write-Host "[$tableName] Found $($duplicates.Count) duplicate $keyColumn!"
        $duplicates | Select-Object Name, Count -First 5 | Out-String | Write-Host
    } else {
        Write-Host "[$tableName] No duplicate $keyColumn."
    }
}

Check-Duplicates $diaBan "MaDiaBan" "Stg_DiaBan"
Check-Duplicates $nhomDT "MaNhom" "Stg_NhomDoiTuong"
Check-Duplicates $chinhSach "MaChinhSach" "Stg_ChinhSachTroCap"
Check-Duplicates $hoGiaDinh "MaHo" "Stg_HoGiaDinh"
Check-Duplicates $doiTuong "MaDoiTuong" "Stg_DoiTuongAnSinh"
Check-Duplicates $dotChiTra "MaDotChiTra" "Stg_DotChiTra"
Check-Duplicates $roles "MaRole" "Stg_Roles"
Check-Duplicates $users "Username" "Stg_Users"

Write-Host "`n--- CHECKING FOREIGN KEYS ---"
function Check-FK($childData, $childCol, $parentData, $parentCol, $desc) {
    if (-not $childData -or -not $parentData) { return }
    $parentKeys = $parentData | Select-Object -ExpandProperty $parentCol | Sort-Object -Unique
    $invalidCount = 0
    foreach ($row in $childData) {
        $val = $row.$childCol
        if (-not [string]::IsNullOrEmpty($val) -and $val -notin $parentKeys) {
            $invalidCount++
            # Write-Host "Invalid FK: $val"
        }
    }
    if ($invalidCount -gt 0) {
        Write-Host "[ERROR] $desc : $invalidCount invalid records found."
    } else {
        Write-Host "[OK] $desc : All keys valid."
    }
}

Check-FK $thanhVienHGD "MaHo" $hoGiaDinh "MaHo" "ThanhVienHoGiaDinh -> HoGiaDinh (MaHo)"
Check-FK $doiTuong "MaHo" $hoGiaDinh "MaHo" "DoiTuongAnSinh -> HoGiaDinh (MaHo)"
Check-FK $doiTuong "MaDiaBan" $diaBan "MaDiaBan" "DoiTuongAnSinh -> DiaBan (MaDiaBan)"
Check-FK $chiTra "MaDoiTuong" $doiTuong "MaDoiTuong" "ChiTraTroCap -> DoiTuongAnSinh (MaDoiTuong)"
Check-FK $chiTra "MaDotChiTra" $dotChiTra "MaDotChiTra" "ChiTraTroCap -> DotChiTra (MaDotChiTra)"
Check-FK $chinhSach "MaNhom" $nhomDT "MaNhom" "ChinhSachTroCap -> NhomDoiTuong (MaNhom)"
Check-FK $doiTuong "MaNhom" $nhomDT "MaNhom" "DoiTuongAnSinh -> NhomDoiTuong (MaNhom)"
Check-FK $users "MaRole" $roles "MaRole" "Users -> Roles (MaRole)"

Write-Host "`n--- CHECKING REQUIRED FIELDS & BUSINESS RULES ---"
$emptyCCCD = $thanhVienHGD | Where-Object { [string]::IsNullOrWhiteSpace($_.CCCD) }
Write-Host "ThanhVienHoGiaDinh without CCCD: $($emptyCCCD.Count)"

$shortCCCD = $thanhVienHGD | Where-Object { -not [string]::IsNullOrWhiteSpace($_.CCCD) -and $_.CCCD.Length -lt 12 }
Write-Host "ThanhVienHoGiaDinh with short CCCD (<12 chars): $($shortCCCD.Count)"

$emptyNgaySinhTV = $thanhVienHGD | Where-Object { [string]::IsNullOrWhiteSpace($_.NgaySinhISO) }
Write-Host "ThanhVienHoGiaDinh without NgaySinhISO: $($emptyNgaySinhTV.Count)"

$emptyNgaySinhDT = $doiTuong | Where-Object { [string]::IsNullOrWhiteSpace($_.NgaySinhISO) }
Write-Host "DoiTuongAnSinh without NgaySinhISO: $($emptyNgaySinhDT.Count)"

$invalidChiTra = $chiTra | Where-Object { [string]::IsNullOrWhiteSpace($_.SoTienTroCap) }
Write-Host "ChiTraTroCap without SoTienTroCap: $($invalidChiTra.Count)"

$chuHoCount = $thanhVienHGD | Where-Object { $_.QuanHeVoiChuHo -eq 'Chủ hộ' } | Group-Object MaHo | Where-Object Count -gt 1
if ($chuHoCount.Count -gt 0) {
    Write-Host "Households with multiple Chu Ho: $($chuHoCount.Count)"
} else {
    Write-Host "All households have at most 1 Chu Ho."
}

Write-Host "`n--- ERRORS IN Stg_LoiDuLieu.csv ---"
$loiDuLieu | Group-Object Bang | Select-Object Name, Count | Format-Table
