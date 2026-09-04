import './WelfareCasesPage.css'
import { useEffect, useMemo, useState } from 'react'
import AppLayout from '../layouts/AppLayout'

type WelfareStatus =
    | 'Draft'
    | 'Submitted'
    | 'UnderReview'
    | 'Approved'
    | 'Rejected'
    | 'Cancelled'
    | 'Closed'

type WelfareCase = {
    id: string
    citizenId: string
    householdId?: string | null
    programId: string
    status: string
    notes?: string | null
    benefitAmount?: number | null
    effectiveFrom?: string | null
    effectiveTo?: string | null
    citizenSnapshot: {
        citizenNumber: string
        fullName: string
        dateOfBirth: string
        gender: string
        householdCode?: string | null
        address: string
        phone?: string | null
        createdAtSnapshot: string
    }
    simulated?: boolean
    programName?: string
}

type WelfareApiResponse = {
    data?: {
        items?: WelfareCase[]
        totalCount?: number
    }
}

type SourceMode =
    | 'real'
    | 'simulation-empty'
    | 'simulation-error'

const STORAGE_KEY =
    'ansinhso.demo.welfarecases.v1'

const PAGE_SIZE = 10

const statuses: WelfareStatus[] = [
    'Draft',
    'Submitted',
    'UnderReview',
    'Approved',
    'Rejected',
    'Cancelled',
    'Closed',
]

const programNames = [
    'Trợ cấp người cao tuổi',
    'Hỗ trợ hộ nghèo',
    'Hỗ trợ hộ cận nghèo',
    'Trợ cấp bảo trợ xã hội',
    'Hỗ trợ trẻ em có hoàn cảnh khó khăn',
    'Hỗ trợ người khuyết tật',
]

const familyNames = [
    'Nguyễn Văn An',
    'Trần Thị Bình',
    'Lê Văn Cường',
    'Phạm Thị Dung',
    'Huỳnh Văn Đạt',
    'Võ Thị Hạnh',
    'Đặng Văn Khải',
    'Bùi Thị Lan',
    'Phan Văn Minh',
    'Đỗ Thị Nga',
    'Nguyễn Văn Phúc',
    'Trần Thị Quỳnh',
]

const formatMoney = (
    value?: number | null,
) => {
    if (value == null)
        return 'Chưa cập nhật'

    return new Intl.NumberFormat(
        'vi-VN',
        {
            style: 'currency',
            currency: 'VND',
        },
    ).format(value)
}

const formatDate = (
    value?: string | null,
) => {
    if (!value)
        return 'Chưa cập nhật'

    const date = new Date(value)

    if (Number.isNaN(date.getTime()))
        return 'Chưa cập nhật'

    return new Intl.DateTimeFormat(
        'vi-VN',
    ).format(date)
}

const translateStatus = (
    status: string,
) => {
    const value =
        status.toLowerCase()

    if (value === 'draft')
        return 'Nháp'

    if (value === 'submitted')
        return 'Đã nộp'

    if (value === 'underreview')
        return 'Đang thẩm định'

    if (value === 'approved')
        return 'Đã duyệt'

    if (value === 'rejected')
        return 'Từ chối'

    if (value === 'cancelled')
        return 'Đã hủy'

    if (value === 'closed')
        return 'Đã đóng'

    return status
}

const createDemoCases =
    (): WelfareCase[] => {

        return Array.from(
            { length: 48 },
            (_, index) => {

                const number =
                    index + 1

                const status =
                    statuses[
                        index %
                        statuses.length
                    ]

                const month =
                    String(
                        7 +
                        (index % 3),
                    ).padStart(
                        2,
                        '0',
                    )

                const day =
                    String(
                        1 +
                        (index % 27),
                    ).padStart(
                        2,
                        '0',
                    )

                return {
                    id:
                        `demo-welfare-${String(number).padStart(3, '0')}`,

                    citizenId:
                        `demo-citizen-${String(number).padStart(3, '0')}`,

                    householdId:
                        `demo-household-${String(
                            1 +
                            (index % 30),
                        ).padStart(3, '0')}`,

                    programId:
                        `demo-program-${1 + (index % programNames.length)}`,

                    programName:
                        programNames[
                            index %
                            programNames.length
                        ],

                    status,

                    notes:
                        'Dữ liệu giả lập phục vụ trình diễn quy trình hồ sơ an sinh.',

                    benefitAmount:
                        500000 +
                        (
                            (index % 7) *
                            250000
                        ),

                    effectiveFrom:
                        `2026-${month}-${day}T00:00:00`,

                    effectiveTo:
                        '2026-12-31T00:00:00',

                    simulated:
                        true,

                    citizenSnapshot: {
                        citizenNumber:
                            `MOPHONG-${String(number).padStart(4, '0')}`,

                        fullName:
                            familyNames[
                                index %
                                familyNames.length
                            ],

                        dateOfBirth:
                            `${1950 + (index % 48)}-01-15T00:00:00`,

                        gender:
                            index % 2 === 0
                                ? 'Male'
                                : 'Female',

                        householdCode:
                            `HG-MP-${String(
                                1 +
                                (index % 30),
                            ).padStart(3, '0')}`,

                        address:
                            `Khu vực giả lập ${1 + (index % 8)}, xã Sông Lũy`,

                        phone:
                            null,

                        createdAtSnapshot:
                            '2026-09-04T00:00:00',
                    },
                }
            },
        )
    }

const loadStoredDemo =
    () => {

        try {
            const raw =
                localStorage.getItem(
                    STORAGE_KEY,
                )

            if (!raw)
                return createDemoCases()

            const parsed =
                JSON.parse(raw)

            if (!Array.isArray(parsed))
                return createDemoCases()

            return parsed as WelfareCase[]
        }
        catch {
            return createDemoCases()
        }
    }

const saveStoredDemo =
    (
        items: WelfareCase[],
    ) => {

        localStorage.setItem(
            STORAGE_KEY,
            JSON.stringify(items),
        )
    }

function WelfareCasesPage() {

    const [
        welfareCases,
        setWelfareCases,
    ] =
        useState<WelfareCase[]>([])

    const [
        sourceMode,
        setSourceMode,
    ] =
        useState<SourceMode>(
            'simulation-empty',
        )

    const [
        loading,
        setLoading,
    ] =
        useState(true)

    const [
        apiMessage,
        setApiMessage,
    ] =
        useState('')

    const [
        keyword,
        setKeyword,
    ] =
        useState('')

    const [
        statusFilter,
        setStatusFilter,
    ] =
        useState('all')

    const [
        page,
        setPage,
    ] =
        useState(1)

    const [
        selected,
        setSelected,
    ] =
        useState<WelfareCase | null>(
            null,
        )

    const [
        creating,
        setCreating,
    ] =
        useState(false)

    const [
        form,
        setForm,
    ] =
        useState({
            fullName: '',
            citizenNumber: '',
            householdCode: '',
            address: '',
            programName:
                programNames[0],
            benefitAmount:
                '1000000',
            status:
                'Draft',
            effectiveFrom:
                '2026-09-04',
            effectiveTo:
                '2026-12-31',
            notes:
                'Hồ sơ giả lập phục vụ demo.',
        })

    useEffect(
        () => {

            const load =
                async () => {

                    setLoading(true)

                    const token =
                        localStorage.getItem(
                            'accessToken',
                        )

                    try {

                        const response =
                            await fetch(
                                '/api/v1/welfarecases?page=1&pageSize=200',
                                {
                                    headers: token
                                        ? {
                                            Authorization:
                                                `Bearer ${token}`,
                                        }
                                        : undefined,
                                },
                            )

                        if (!response.ok)
                            throw new Error(
                                `HTTP ${response.status}`,
                            )

                        const body =
                            (await response.json()) as WelfareApiResponse

                        const realItems =
                            body.data?.items ??
                            []

                        const total =
                            body.data?.totalCount ??
                            realItems.length

                        if (
                            total > 0 &&
                            realItems.length > 0
                        ) {

                            setWelfareCases(
                                realItems,
                            )

                            setSourceMode(
                                'real',
                            )

                            setApiMessage(
                                'Đang hiển thị dữ liệu thật từ AnSinhSoRealDb.',
                            )
                        }
                        else {

                            const demo =
                                loadStoredDemo()

                            setWelfareCases(
                                demo,
                            )

                            setSourceMode(
                                'simulation-empty',
                            )

                            setApiMessage(
                                'Production chưa có hồ sơ an sinh. Hệ thống đang dùng dữ liệu giả lập phục vụ demo.',
                            )
                        }
                    }
                    catch {

                        const demo =
                            loadStoredDemo()

                        setWelfareCases(
                            demo,
                        )

                        setSourceMode(
                            'simulation-error',
                        )

                        setApiMessage(
                            'API hiện không khả dụng. Hệ thống chuyển sang dữ liệu giả lập cục bộ.',
                        )
                    }
                    finally {
                        setLoading(false)
                    }
                }

            void load()
        },
        [],
    )

    const simulation =
        sourceMode !== 'real'

    const filtered =
        useMemo(
            () => {

                const q =
                    keyword
                        .trim()
                        .toLowerCase()

                return welfareCases.filter(
                    (item) => {

                        const matchStatus =
                            statusFilter ===
                                'all' ||
                            item.status ===
                                statusFilter

                        const matchKeyword =
                            !q ||
                            item.citizenSnapshot
                                .fullName
                                .toLowerCase()
                                .includes(q) ||
                            item.citizenSnapshot
                                .citizenNumber
                                .toLowerCase()
                                .includes(q) ||
                            (
                                item.citizenSnapshot
                                    .householdCode ??
                                ''
                            )
                                .toLowerCase()
                                .includes(q) ||
                            (
                                item.programName ??
                                ''
                            )
                                .toLowerCase()
                                .includes(q)

                        return (
                            matchStatus &&
                            matchKeyword
                        )
                    },
                )
            },
            [
                welfareCases,
                keyword,
                statusFilter,
            ],
        )

    const totalPages =
        Math.max(
            1,
            Math.ceil(
                filtered.length /
                PAGE_SIZE,
            ),
        )

    const visible =
        filtered.slice(
            (page - 1) *
                PAGE_SIZE,
            page *
                PAGE_SIZE,
        )

    const approved =
        welfareCases.filter(
            (item) =>
                item.status ===
                'Approved' ||
                item.status ===
                'Closed',
        ).length

    const reviewing =
        welfareCases.filter(
            (item) =>
                item.status ===
                    'Submitted' ||
                item.status ===
                    'UnderReview',
        ).length

    const saveDemo =
        (
            next: WelfareCase[],
        ) => {

            setWelfareCases(next)
            saveStoredDemo(next)
        }

    const openCreate =
        () => {

            setSelected(null)

            setForm({
                fullName: '',
                citizenNumber: '',
                householdCode: '',
                address: '',
                programName:
                    programNames[0],
                benefitAmount:
                    '1000000',
                status:
                    'Draft',
                effectiveFrom:
                    '2026-09-04',
                effectiveTo:
                    '2026-12-31',
                notes:
                    'Hồ sơ giả lập phục vụ demo.',
            })

            setCreating(true)
        }

    const openEdit =
        (
            item: WelfareCase,
        ) => {

            setCreating(false)
            setSelected(item)

            setForm({
                fullName:
                    item.citizenSnapshot
                        .fullName,

                citizenNumber:
                    item.citizenSnapshot
                        .citizenNumber,

                householdCode:
                    item.citizenSnapshot
                        .householdCode ??
                    '',

                address:
                    item.citizenSnapshot
                        .address,

                programName:
                    item.programName ??
                    'Chương trình thực tế',

                benefitAmount:
                    String(
                        item.benefitAmount ??
                        0,
                    ),

                status:
                    item.status,

                effectiveFrom:
                    item.effectiveFrom
                        ?.slice(0, 10) ??
                    '',

                effectiveTo:
                    item.effectiveTo
                        ?.slice(0, 10) ??
                    '',

                notes:
                    item.notes ??
                    '',
            })
        }

    const closeModal =
        () => {
            setSelected(null)
            setCreating(false)
        }

    const submitDemo =
        () => {

            if (!simulation)
                return

            if (
                !form.fullName.trim()
            )
                return

            if (creating) {

                const id =
                    `demo-welfare-custom-${Date.now()}`

                const item: WelfareCase =
                {
                    id,
                    citizenId:
                        `demo-citizen-custom-${Date.now()}`,

                    householdId:
                        null,

                    programId:
                        'demo-program-custom',

                    programName:
                        form.programName,

                    status:
                        form.status,

                    notes:
                        form.notes,

                    benefitAmount:
                        Number(
                            form.benefitAmount,
                        ) || 0,

                    effectiveFrom:
                        form.effectiveFrom
                            ? `${form.effectiveFrom}T00:00:00`
                            : null,

                    effectiveTo:
                        form.effectiveTo
                            ? `${form.effectiveTo}T00:00:00`
                            : null,

                    simulated:
                        true,

                    citizenSnapshot: {
                        citizenNumber:
                            form.citizenNumber ||
                            `MOPHONG-${Date.now()}`,

                        fullName:
                            form.fullName,

                        dateOfBirth:
                            '1960-01-01T00:00:00',

                        gender:
                            'Other',

                        householdCode:
                            form.householdCode,

                        address:
                            form.address,

                        phone:
                            null,

                        createdAtSnapshot:
                            new Date()
                                .toISOString(),
                    },
                }

                saveDemo(
                    [
                        item,
                        ...welfareCases,
                    ],
                )
            }
            else if (selected) {

                const next =
                    welfareCases.map(
                        (item) => {

                            if (
                                item.id !==
                                selected.id
                            )
                                return item

                            return {
                                ...item,

                                programName:
                                    form.programName,

                                status:
                                    form.status,

                                notes:
                                    form.notes,

                                benefitAmount:
                                    Number(
                                        form.benefitAmount,
                                    ) || 0,

                                effectiveFrom:
                                    form.effectiveFrom
                                        ? `${form.effectiveFrom}T00:00:00`
                                        : null,

                                effectiveTo:
                                    form.effectiveTo
                                        ? `${form.effectiveTo}T00:00:00`
                                        : null,

                                citizenSnapshot: {
                                    ...item.citizenSnapshot,

                                    fullName:
                                        form.fullName,

                                    citizenNumber:
                                        form.citizenNumber,

                                    householdCode:
                                        form.householdCode,

                                    address:
                                        form.address,
                                },
                            }
                        },
                    )

                saveDemo(next)
            }

            closeModal()
        }

    const removeDemo =
        (
            item: WelfareCase,
        ) => {

            if (
                !simulation ||
                !item.simulated
            )
                return

            if (
                !window.confirm(
                    'Xóa hồ sơ giả lập này?',
                )
            )
                return

            saveDemo(
                welfareCases.filter(
                    (x) =>
                        x.id !==
                        item.id,
                ),
            )
        }

    if (loading) {
        return (
            <AppLayout>
                <div className="wfs-state">
                    Đang tải dữ liệu an sinh...
                </div>
            </AppLayout>
        )
    }

    return (
        <AppLayout>
            <style>{`
                .wfs-page{padding:24px;display:grid;gap:18px;color:#0f172a}
                .wfs-head{display:flex;align-items:flex-start;justify-content:space-between;gap:16px}
                .wfs-head h1{margin:0 0 6px;font-size:25px}
                .wfs-head p{margin:0;color:#64748b;font-size:13px}
                .wfs-badge{padding:8px 12px;border-radius:999px;background:#eef2ff;color:#4338ca;font-size:11px;font-weight:800}
                .wfs-badge.demo{background:#fff7ed;color:#c2410c}
                .wfs-notice{padding:13px 15px;border:1px solid #e2e8f0;border-radius:12px;background:#fff;font-size:12px;color:#475569}
                .wfs-cards{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:12px}
                .wfs-card{background:#fff;border:1px solid #e2e8f0;border-radius:13px;padding:15px}
                .wfs-card span{font-size:11px;color:#64748b}
                .wfs-card strong{display:block;margin-top:6px;font-size:25px}
                .wfs-tools{display:flex;gap:10px;flex-wrap:wrap}
                .wfs-tools input,.wfs-tools select,.wfs-modal input,.wfs-modal select,.wfs-modal textarea{border:1px solid #dbe3ee;border-radius:9px;padding:9px 11px;font:inherit;font-size:12px;background:white}
                .wfs-tools input{flex:1 1 280px}
                .wfs-button{border:0;border-radius:9px;padding:9px 13px;cursor:pointer;font-size:11px;font-weight:750;background:#2563eb;color:white}
                .wfs-button.secondary{background:#f1f5f9;color:#334155}
                .wfs-button.danger{background:#fee2e2;color:#b91c1c}
                .wfs-table-wrap{overflow:auto;background:#fff;border:1px solid #e2e8f0;border-radius:13px}
                .wfs-table{width:100%;border-collapse:collapse;min-width:1050px}
                .wfs-table th,.wfs-table td{padding:11px 12px;border-bottom:1px solid #edf2f7;text-align:left;font-size:11px;vertical-align:top}
                .wfs-table th{background:#f8fafc;color:#475569;font-weight:800}
                .wfs-status{display:inline-flex;padding:4px 8px;border-radius:999px;background:#eef2ff;color:#4338ca;font-weight:750}
                .wfs-demo-tag{display:inline-flex;margin-top:4px;padding:3px 6px;border-radius:6px;background:#fff7ed;color:#c2410c;font-size:9px;font-weight:800}
                .wfs-actions{display:flex;gap:6px}
                .wfs-pages{display:flex;align-items:center;justify-content:flex-end;gap:9px}
                .wfs-overlay{position:fixed;inset:0;background:rgba(15,23,42,.45);display:grid;place-items:center;padding:20px;z-index:1000}
                .wfs-modal{width:min(720px,100%);max-height:90vh;overflow:auto;background:white;border-radius:16px;padding:20px;box-shadow:0 24px 70px rgba(15,23,42,.28)}
                .wfs-modal h2{margin:0 0 15px}
                .wfs-grid{display:grid;grid-template-columns:1fr 1fr;gap:12px}
                .wfs-field{display:grid;gap:5px}
                .wfs-field label{font-size:10px;font-weight:800;color:#64748b}
                .wfs-field.full{grid-column:1/-1}
                .wfs-modal textarea{min-height:75px;resize:vertical}
                .wfs-modal-actions{display:flex;justify-content:flex-end;gap:8px;margin-top:16px}
                @media(max-width:800px){.wfs-cards,.wfs-grid{grid-template-columns:1fr}.wfs-head{flex-direction:column}}
            `}</style>

            <div className="wfs-page">

                <section className="wfs-head">
                    <div>
                        <h1>
                            An sinh xã hội
                        </h1>

                        <p>
                            Quản lý hồ sơ, trạng thái xét duyệt
                            và mức hỗ trợ an sinh.
                        </p>
                    </div>

                    <span
                        className={
                            `wfs-badge ${
                                simulation
                                    ? 'demo'
                                    : ''
                            }`
                        }
                    >
                        {simulation
                            ? 'DỮ LIỆU GIẢ LẬP'
                            : 'ANSINHSOREALDB'}
                    </span>
                </section>

                <div className="wfs-notice">
                    {apiMessage}
                    {simulation &&
                        ' Các thao tác thêm/sửa/xóa chỉ lưu trong trình duyệt và không ghi vào database production.'}
                </div>

                <section className="wfs-cards">
                    <article className="wfs-card">
                        <span>
                            Tổng hồ sơ đang hiển thị
                        </span>
                        <strong>
                            {welfareCases.length}
                        </strong>
                    </article>

                    <article className="wfs-card">
                        <span>
                            Đang xử lý / thẩm định
                        </span>
                        <strong>
                            {reviewing}
                        </strong>
                    </article>

                    <article className="wfs-card">
                        <span>
                            Đã duyệt / đã đóng
                        </span>
                        <strong>
                            {approved}
                        </strong>
                    </article>
                </section>

                <section className="wfs-tools">
                    <input
                        value={keyword}
                        placeholder="Tìm họ tên, mã hồ sơ, mã hộ, chương trình..."
                        onChange={(event) => {
                            setKeyword(
                                event.target.value,
                            )
                            setPage(1)
                        }}
                    />

                    <select
                        value={statusFilter}
                        onChange={(event) => {
                            setStatusFilter(
                                event.target.value,
                            )
                            setPage(1)
                        }}
                    >
                        <option value="all">
                            Tất cả trạng thái
                        </option>

                        {statuses.map(
                            (status) => (
                                <option
                                    key={status}
                                    value={status}
                                >
                                    {translateStatus(
                                        status,
                                    )}
                                </option>
                            ),
                        )}
                    </select>

                    {simulation && (
                        <button
                            type="button"
                            className="wfs-button"
                            onClick={openCreate}
                        >
                            + Thêm hồ sơ giả lập
                        </button>
                    )}
                </section>

                <div className="wfs-table-wrap">
                    <table className="wfs-table">
                        <thead>
                            <tr>
                                <th>STT</th>
                                <th>Người dân</th>
                                <th>Mã định danh</th>
                                <th>Chương trình</th>
                                <th>Mức hỗ trợ</th>
                                <th>Trạng thái</th>
                                <th>Hiệu lực</th>
                                <th>Thao tác</th>
                            </tr>
                        </thead>

                        <tbody>
                            {visible.map(
                                (
                                    item,
                                    index,
                                ) => (
                                    <tr key={item.id}>
                                        <td>
                                            {(page - 1) *
                                                PAGE_SIZE +
                                                index +
                                                1}
                                        </td>

                                        <td>
                                            <strong>
                                                {
                                                    item
                                                        .citizenSnapshot
                                                        .fullName
                                                }
                                            </strong>

                                            {item.simulated && (
                                                <div className="wfs-demo-tag">
                                                    MÔ PHỎNG
                                                </div>
                                            )}
                                        </td>

                                        <td>
                                            {
                                                item
                                                    .citizenSnapshot
                                                    .citizenNumber
                                            }
                                        </td>

                                        <td>
                                            {item.programName ??
                                                item.programId}
                                        </td>

                                        <td>
                                            {formatMoney(
                                                item.benefitAmount,
                                            )}
                                        </td>

                                        <td>
                                            <span className="wfs-status">
                                                {translateStatus(
                                                    item.status,
                                                )}
                                            </span>
                                        </td>

                                        <td>
                                            {formatDate(
                                                item.effectiveFrom,
                                            )}
                                            <br />
                                            →
                                            {' '}
                                            {formatDate(
                                                item.effectiveTo,
                                            )}
                                        </td>

                                        <td>
                                            <div className="wfs-actions">
                                                <button
                                                    type="button"
                                                    className="wfs-button secondary"
                                                    onClick={() =>
                                                        openEdit(
                                                            item,
                                                        )
                                                    }
                                                >
                                                    {simulation
                                                        ? 'Xem / sửa'
                                                        : 'Xem'}
                                                </button>

                                                {simulation &&
                                                    item.simulated && (
                                                        <button
                                                            type="button"
                                                            className="wfs-button danger"
                                                            onClick={() =>
                                                                removeDemo(
                                                                    item,
                                                                )
                                                            }
                                                        >
                                                            Xóa
                                                        </button>
                                                    )}
                                            </div>
                                        </td>
                                    </tr>
                                ),
                            )}

                            {visible.length === 0 && (
                                <tr>
                                    <td colSpan={8}>
                                        Không có hồ sơ phù hợp.
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>

                <div className="wfs-pages">
                    <button
                        type="button"
                        className="wfs-button secondary"
                        disabled={page <= 1}
                        onClick={() =>
                            setPage(
                                Math.max(
                                    1,
                                    page - 1,
                                ),
                            )
                        }
                    >
                        ← Trước
                    </button>

                    <span>
                        Trang {page}/{totalPages}
                        {' · '}
                        {filtered.length} hồ sơ
                    </span>

                    <button
                        type="button"
                        className="wfs-button secondary"
                        disabled={
                            page >= totalPages
                        }
                        onClick={() =>
                            setPage(
                                Math.min(
                                    totalPages,
                                    page + 1,
                                ),
                            )
                        }
                    >
                        Sau →
                    </button>
                </div>

                {(creating || selected) && (
                    <div className="wfs-overlay">
                        <div className="wfs-modal">
                            <h2>
                                {creating
                                    ? 'Thêm hồ sơ an sinh giả lập'
                                    : simulation
                                        ? 'Xem / chỉnh sửa hồ sơ'
                                        : 'Chi tiết hồ sơ'}
                            </h2>

                            <div className="wfs-grid">

                                <div className="wfs-field">
                                    <label>
                                        HỌ VÀ TÊN
                                    </label>
                                    <input
                                        disabled={!simulation}
                                        value={form.fullName}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                fullName:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="wfs-field">
                                    <label>
                                        MÃ ĐỊNH DANH
                                    </label>
                                    <input
                                        disabled={!simulation}
                                        value={form.citizenNumber}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                citizenNumber:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="wfs-field">
                                    <label>
                                        MÃ HỘ
                                    </label>
                                    <input
                                        disabled={!simulation}
                                        value={form.householdCode}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                householdCode:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="wfs-field">
                                    <label>
                                        CHƯƠNG TRÌNH
                                    </label>
                                    <select
                                        disabled={!simulation}
                                        value={form.programName}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                programName:
                                                    e.target.value,
                                            })
                                        }
                                    >
                                        {programNames.map(
                                            (name) => (
                                                <option
                                                    key={name}
                                                    value={name}
                                                >
                                                    {name}
                                                </option>
                                            ),
                                        )}
                                    </select>
                                </div>

                                <div className="wfs-field full">
                                    <label>
                                        ĐỊA CHỈ
                                    </label>
                                    <input
                                        disabled={!simulation}
                                        value={form.address}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                address:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="wfs-field">
                                    <label>
                                        MỨC HỖ TRỢ
                                    </label>
                                    <input
                                        disabled={!simulation}
                                        type="number"
                                        min="0"
                                        value={form.benefitAmount}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                benefitAmount:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="wfs-field">
                                    <label>
                                        TRẠNG THÁI
                                    </label>
                                    <select
                                        disabled={!simulation}
                                        value={form.status}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                status:
                                                    e.target.value,
                                            })
                                        }
                                    >
                                        {statuses.map(
                                            (status) => (
                                                <option
                                                    key={status}
                                                    value={status}
                                                >
                                                    {translateStatus(
                                                        status,
                                                    )}
                                                </option>
                                            ),
                                        )}
                                    </select>
                                </div>

                                <div className="wfs-field">
                                    <label>
                                        TỪ NGÀY
                                    </label>
                                    <input
                                        disabled={!simulation}
                                        type="date"
                                        value={form.effectiveFrom}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                effectiveFrom:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="wfs-field">
                                    <label>
                                        ĐẾN NGÀY
                                    </label>
                                    <input
                                        disabled={!simulation}
                                        type="date"
                                        value={form.effectiveTo}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                effectiveTo:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="wfs-field full">
                                    <label>
                                        GHI CHÚ
                                    </label>
                                    <textarea
                                        disabled={!simulation}
                                        value={form.notes}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                notes:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>
                            </div>

                            <div className="wfs-modal-actions">
                                <button
                                    type="button"
                                    className="wfs-button secondary"
                                    onClick={closeModal}
                                >
                                    Đóng
                                </button>

                                {simulation && (
                                    <button
                                        type="button"
                                        className="wfs-button"
                                        onClick={submitDemo}
                                    >
                                        Lưu giả lập
                                    </button>
                                )}
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </AppLayout>
    )
}

export default WelfareCasesPage