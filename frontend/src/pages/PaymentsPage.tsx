import './PaymentsPage.css'
import { useEffect, useMemo, useState } from 'react'
import AppLayout from '../layouts/AppLayout'

type PaymentStatus =
    | 'Draft'
    | 'Pending'
    | 'Approved'
    | 'Processing'
    | 'Paid'
    | 'Failed'
    | 'Cancelled'

type Payment = {
    id: string
    paymentNumber: string
    citizenId: string
    householdId?: string | null
    welfareCaseId: string
    amount: number
    scheduledDate: string
    actualPaymentDate?: string | null
    method: string
    status: string
    notes?: string | null
    simulated?: boolean
    beneficiaryName?: string
    paymentPointName?: string
}

type PaymentApiResponse = {
    data?: {
        items?: Payment[]
        totalCount?: number
    }
}

type SourceMode =
    | 'real'
    | 'simulation-empty'
    | 'simulation-error'

const STORAGE_KEY =
    'ansinhso.demo.payments.v1'

const PAGE_SIZE =
    10

const paymentStatuses: PaymentStatus[] =
[
    'Draft',
    'Pending',
    'Approved',
    'Processing',
    'Paid',
    'Failed',
    'Cancelled',
]

const demoNames = [
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

const demoPaymentPoints = [
    'Điểm chi trả trung tâm xã',
    'Điểm chi trả khu vực Bắc',
    'Điểm chi trả khu vực Nam',
    'Điểm chi trả khu vực Đông',
    'Điểm chi trả lưu động',
]

const formatMoney =
    (value: number) =>
        new Intl.NumberFormat(
            'vi-VN',
            {
                style: 'currency',
                currency: 'VND',
            },
        ).format(value)

const formatDate =
    (
        value?: string | null,
    ) => {

        if (!value)
            return 'Chưa cập nhật'

        const date =
            new Date(value)

        if (
            Number.isNaN(
                date.getTime(),
            )
        )
            return 'Chưa cập nhật'

        return new Intl.DateTimeFormat(
            'vi-VN',
        ).format(date)
    }

const translateMethod =
    (method: string) => {

        if (
            method.toLowerCase() ===
            'cash'
        )
            return 'Tiền mặt'

        if (
            method.toLowerCase() ===
            'banktransfer'
        )
            return 'Chuyển khoản'

        return method
    }

const translateStatus =
    (
        status: string,
    ) => {

        const value =
            status.toLowerCase()

        if (value === 'draft')
            return 'Nháp'

        if (value === 'pending')
            return 'Chờ chi trả'

        if (value === 'approved')
            return 'Đã duyệt'

        if (value === 'processing')
            return 'Đang xử lý'

        if (value === 'paid')
            return 'Đã chi trả'

        if (value === 'failed')
            return 'Thất bại'

        if (value === 'cancelled')
            return 'Đã hủy'

        return status
    }

const createDemoPayments =
    (): Payment[] => {

        return Array.from(
            { length: 36 },
            (_, index) => {

                const number =
                    index + 1

                const status =
                    paymentStatuses[
                        index %
                        paymentStatuses.length
                    ]

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
                        `demo-payment-${String(number).padStart(3, '0')}`,

                    paymentNumber:
                        `CT-MP-2026-${String(number).padStart(4, '0')}`,

                    citizenId:
                        `demo-citizen-${String(number).padStart(3, '0')}`,

                    householdId:
                        `demo-household-${String(
                            1 +
                            (index % 30),
                        ).padStart(3, '0')}`,

                    welfareCaseId:
                        `demo-welfare-${String(number).padStart(3, '0')}`,

                    amount:
                        500000 +
                        (
                            (index % 7) *
                            250000
                        ),

                    scheduledDate:
                        `2026-09-${day}T00:00:00`,

                    actualPaymentDate:
                        status === 'Paid'
                            ? `2026-09-${day}T09:00:00`
                            : null,

                    method:
                        index % 4 === 0
                            ? 'BankTransfer'
                            : 'Cash',

                    status,

                    notes:
                        'Lượt chi trả giả lập phục vụ trình diễn.',

                    simulated:
                        true,

                    beneficiaryName:
                        demoNames[
                            index %
                            demoNames.length
                        ],

                    paymentPointName:
                        demoPaymentPoints[
                            index %
                            demoPaymentPoints.length
                        ],
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
                return createDemoPayments()

            const parsed =
                JSON.parse(raw)

            if (!Array.isArray(parsed))
                return createDemoPayments()

            return parsed as Payment[]
        }
        catch {
            return createDemoPayments()
        }
    }

const saveStoredDemo =
    (
        items: Payment[],
    ) => {

        localStorage.setItem(
            STORAGE_KEY,
            JSON.stringify(items),
        )
    }

function PaymentsPage() {

    const [
        payments,
        setPayments,
    ] =
        useState<Payment[]>([])

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
        useState<Payment | null>(
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
            paymentNumber: '',
            beneficiaryName: '',
            amount:
                '1000000',
            method:
                'Cash',
            status:
                'Pending',
            scheduledDate:
                '2026-09-04',
            actualPaymentDate:
                '',
            paymentPointName:
                demoPaymentPoints[0],
            notes:
                'Lượt chi trả giả lập phục vụ demo.',
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
                                '/api/v1/payments?page=1&pageSize=200',
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
                            (await response.json()) as PaymentApiResponse

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

                            setPayments(
                                realItems,
                            )

                            setSourceMode(
                                'real',
                            )

                            setApiMessage(
                                'Đang hiển thị dữ liệu chi trả thật từ AnSinhSoRealDb.',
                            )
                        }
                        else {

                            const demo =
                                loadStoredDemo()

                            setPayments(
                                demo,
                            )

                            setSourceMode(
                                'simulation-empty',
                            )

                            setApiMessage(
                                'Production chưa có lượt chi trả. Hệ thống đang dùng dữ liệu giả lập phục vụ demo.',
                            )
                        }
                    }
                    catch {

                        const demo =
                            loadStoredDemo()

                        setPayments(
                            demo,
                        )

                        setSourceMode(
                            'simulation-error',
                        )

                        setApiMessage(
                            'API hiện không khả dụng. Hệ thống chuyển sang dữ liệu chi trả giả lập cục bộ.',
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

                return payments.filter(
                    (item) => {

                        const statusOk =
                            statusFilter ===
                                'all' ||
                            item.status ===
                                statusFilter

                        const keywordOk =
                            !q ||
                            item.paymentNumber
                                .toLowerCase()
                                .includes(q) ||
                            (
                                item.beneficiaryName ??
                                ''
                            )
                                .toLowerCase()
                                .includes(q) ||
                            item.citizenId
                                .toLowerCase()
                                .includes(q) ||
                            (
                                item.paymentPointName ??
                                ''
                            )
                                .toLowerCase()
                                .includes(q)

                        return (
                            statusOk &&
                            keywordOk
                        )
                    },
                )
            },
            [
                payments,
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

    const paid =
        payments.filter(
            (item) =>
                item.status ===
                'Paid',
        )

    const pending =
        payments.filter(
            (item) =>
                item.status ===
                    'Pending' ||
                item.status ===
                    'Approved' ||
                item.status ===
                    'Processing',
        ).length

    const paidAmount =
        paid.reduce(
            (
                total,
                item,
            ) =>
                total +
                item.amount,
            0,
        )

    const saveDemo =
        (
            next: Payment[],
        ) => {

            setPayments(next)
            saveStoredDemo(next)
        }

    const openCreate =
        () => {

            setSelected(null)
            setCreating(true)

            setForm({
                paymentNumber:
                    `CT-MP-2026-${String(
                        payments.length +
                        1,
                    ).padStart(4, '0')}`,

                beneficiaryName:
                    '',

                amount:
                    '1000000',

                method:
                    'Cash',

                status:
                    'Pending',

                scheduledDate:
                    '2026-09-04',

                actualPaymentDate:
                    '',

                paymentPointName:
                    demoPaymentPoints[0],

                notes:
                    'Lượt chi trả giả lập phục vụ demo.',
            })
        }

    const openEdit =
        (
            item: Payment,
        ) => {

            setCreating(false)
            setSelected(item)

            setForm({
                paymentNumber:
                    item.paymentNumber,

                beneficiaryName:
                    item.beneficiaryName ??
                    '',

                amount:
                    String(
                        item.amount,
                    ),

                method:
                    item.method,

                status:
                    item.status,

                scheduledDate:
                    item.scheduledDate
                        ?.slice(0, 10) ??
                    '',

                actualPaymentDate:
                    item.actualPaymentDate
                        ?.slice(0, 10) ??
                    '',

                paymentPointName:
                    item.paymentPointName ??
                    'Điểm chi trả thực tế',

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
                !form.paymentNumber.trim() ||
                !form.beneficiaryName.trim()
            )
                return

            if (creating) {

                const stamp =
                    Date.now()

                const item: Payment =
                {
                    id:
                        `demo-payment-custom-${stamp}`,

                    paymentNumber:
                        form.paymentNumber,

                    citizenId:
                        `demo-citizen-custom-${stamp}`,

                    householdId:
                        null,

                    welfareCaseId:
                        `demo-welfare-custom-${stamp}`,

                    amount:
                        Number(
                            form.amount,
                        ) || 0,

                    scheduledDate:
                        form.scheduledDate
                            ? `${form.scheduledDate}T00:00:00`
                            : new Date()
                                .toISOString(),

                    actualPaymentDate:
                        form.actualPaymentDate
                            ? `${form.actualPaymentDate}T00:00:00`
                            : null,

                    method:
                        form.method,

                    status:
                        form.status,

                    notes:
                        form.notes,

                    simulated:
                        true,

                    beneficiaryName:
                        form.beneficiaryName,

                    paymentPointName:
                        form.paymentPointName,
                }

                saveDemo(
                    [
                        item,
                        ...payments,
                    ],
                )
            }
            else if (selected) {

                const next =
                    payments.map(
                        (item) => {

                            if (
                                item.id !==
                                selected.id
                            )
                                return item

                            return {
                                ...item,

                                paymentNumber:
                                    form.paymentNumber,

                                beneficiaryName:
                                    form.beneficiaryName,

                                amount:
                                    Number(
                                        form.amount,
                                    ) || 0,

                                method:
                                    form.method,

                                status:
                                    form.status,

                                scheduledDate:
                                    form.scheduledDate
                                        ? `${form.scheduledDate}T00:00:00`
                                        : item.scheduledDate,

                                actualPaymentDate:
                                    form.actualPaymentDate
                                        ? `${form.actualPaymentDate}T00:00:00`
                                        : null,

                                paymentPointName:
                                    form.paymentPointName,

                                notes:
                                    form.notes,
                            }
                        },
                    )

                saveDemo(next)
            }

            closeModal()
        }

    const removeDemo =
        (
            item: Payment,
        ) => {

            if (
                !simulation ||
                !item.simulated
            )
                return

            if (
                !window.confirm(
                    'Xóa lượt chi trả giả lập này?',
                )
            )
                return

            saveDemo(
                payments.filter(
                    (x) =>
                        x.id !==
                        item.id,
                ),
            )
        }

    if (loading) {
        return (
            <AppLayout>
                <div className="pmt-state">
                    Đang tải dữ liệu chi trả...
                </div>
            </AppLayout>
        )
    }

    return (
        <AppLayout>
            <style>{`
                .pmt-page{padding:24px;display:grid;gap:18px;color:#0f172a}
                .pmt-head{display:flex;align-items:flex-start;justify-content:space-between;gap:16px}
                .pmt-head h1{margin:0 0 6px;font-size:25px}
                .pmt-head p{margin:0;color:#64748b;font-size:13px}
                .pmt-badge{padding:8px 12px;border-radius:999px;background:#ecfdf5;color:#047857;font-size:11px;font-weight:800}
                .pmt-badge.demo{background:#fff7ed;color:#c2410c}
                .pmt-notice{padding:13px 15px;border:1px solid #e2e8f0;border-radius:12px;background:#fff;font-size:12px;color:#475569}
                .pmt-cards{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:12px}
                .pmt-card{background:#fff;border:1px solid #e2e8f0;border-radius:13px;padding:15px}
                .pmt-card span{font-size:11px;color:#64748b}
                .pmt-card strong{display:block;margin-top:6px;font-size:25px}
                .pmt-tools{display:flex;gap:10px;flex-wrap:wrap}
                .pmt-tools input,.pmt-tools select,.pmt-modal input,.pmt-modal select,.pmt-modal textarea{border:1px solid #dbe3ee;border-radius:9px;padding:9px 11px;font:inherit;font-size:12px;background:white}
                .pmt-tools input{flex:1 1 280px}
                .pmt-button{border:0;border-radius:9px;padding:9px 13px;cursor:pointer;font-size:11px;font-weight:750;background:#2563eb;color:white}
                .pmt-button.secondary{background:#f1f5f9;color:#334155}
                .pmt-button.danger{background:#fee2e2;color:#b91c1c}
                .pmt-table-wrap{overflow:auto;background:#fff;border:1px solid #e2e8f0;border-radius:13px}
                .pmt-table{width:100%;border-collapse:collapse;min-width:1120px}
                .pmt-table th,.pmt-table td{padding:11px 12px;border-bottom:1px solid #edf2f7;text-align:left;font-size:11px;vertical-align:top}
                .pmt-table th{background:#f8fafc;color:#475569;font-weight:800}
                .pmt-status{display:inline-flex;padding:4px 8px;border-radius:999px;background:#ecfdf5;color:#047857;font-weight:750}
                .pmt-demo-tag{display:inline-flex;margin-top:4px;padding:3px 6px;border-radius:6px;background:#fff7ed;color:#c2410c;font-size:9px;font-weight:800}
                .pmt-actions{display:flex;gap:6px}
                .pmt-pages{display:flex;align-items:center;justify-content:flex-end;gap:9px}
                .pmt-overlay{position:fixed;inset:0;background:rgba(15,23,42,.45);display:grid;place-items:center;padding:20px;z-index:1000}
                .pmt-modal{width:min(720px,100%);max-height:90vh;overflow:auto;background:white;border-radius:16px;padding:20px;box-shadow:0 24px 70px rgba(15,23,42,.28)}
                .pmt-modal h2{margin:0 0 15px}
                .pmt-grid{display:grid;grid-template-columns:1fr 1fr;gap:12px}
                .pmt-field{display:grid;gap:5px}
                .pmt-field label{font-size:10px;font-weight:800;color:#64748b}
                .pmt-field.full{grid-column:1/-1}
                .pmt-modal textarea{min-height:75px;resize:vertical}
                .pmt-modal-actions{display:flex;justify-content:flex-end;gap:8px;margin-top:16px}
                @media(max-width:800px){.pmt-cards,.pmt-grid{grid-template-columns:1fr}.pmt-head{flex-direction:column}}
            `}</style>

            <div className="pmt-page">

                <section className="pmt-head">
                    <div>
                        <h1>
                            Chi trả trợ cấp
                        </h1>

                        <p>
                            Theo dõi kế hoạch, trạng thái
                            và kết quả chi trả.
                        </p>
                    </div>

                    <span
                        className={
                            `pmt-badge ${
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

                <div className="pmt-notice">
                    {apiMessage}
                    {simulation &&
                        ' Thêm/sửa/xóa chỉ lưu trong trình duyệt và không ghi vào database production.'}
                </div>

                <section className="pmt-cards">
                    <article className="pmt-card">
                        <span>
                            Tổng lượt chi trả
                        </span>
                        <strong>
                            {payments.length}
                        </strong>
                    </article>

                    <article className="pmt-card">
                        <span>
                            Đang chờ / xử lý
                        </span>
                        <strong>
                            {pending}
                        </strong>
                    </article>

                    <article className="pmt-card">
                        <span>
                            Tổng tiền đã chi
                        </span>
                        <strong
                            style={{
                                fontSize:
                                    '18px',
                            }}
                        >
                            {formatMoney(
                                paidAmount,
                            )}
                        </strong>
                    </article>
                </section>

                <section className="pmt-tools">
                    <input
                        value={keyword}
                        placeholder="Tìm mã chi trả, người nhận, điểm chi trả..."
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

                        {paymentStatuses.map(
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
                            className="pmt-button"
                            onClick={openCreate}
                        >
                            + Thêm lượt chi giả lập
                        </button>
                    )}
                </section>

                <div className="pmt-table-wrap">
                    <table className="pmt-table">
                        <thead>
                            <tr>
                                <th>STT</th>
                                <th>Mã chi trả</th>
                                <th>Người nhận</th>
                                <th>Số tiền</th>
                                <th>Hình thức</th>
                                <th>Điểm chi trả</th>
                                <th>Trạng thái</th>
                                <th>Dự kiến</th>
                                <th>Thực chi</th>
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
                                                    item.paymentNumber
                                                }
                                            </strong>

                                            {item.simulated && (
                                                <div className="pmt-demo-tag">
                                                    MÔ PHỎNG
                                                </div>
                                            )}
                                        </td>

                                        <td>
                                            {item.beneficiaryName ??
                                                item.citizenId}
                                        </td>

                                        <td>
                                            {formatMoney(
                                                item.amount,
                                            )}
                                        </td>

                                        <td>
                                            {translateMethod(
                                                item.method,
                                            )}
                                        </td>

                                        <td>
                                            {item.paymentPointName ??
                                                '—'}
                                        </td>

                                        <td>
                                            <span className="pmt-status">
                                                {translateStatus(
                                                    item.status,
                                                )}
                                            </span>
                                        </td>

                                        <td>
                                            {formatDate(
                                                item.scheduledDate,
                                            )}
                                        </td>

                                        <td>
                                            {formatDate(
                                                item.actualPaymentDate,
                                            )}
                                        </td>

                                        <td>
                                            <div className="pmt-actions">
                                                <button
                                                    type="button"
                                                    className="pmt-button secondary"
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
                                                            className="pmt-button danger"
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
                                    <td colSpan={10}>
                                        Không có lượt chi trả phù hợp.
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>

                <div className="pmt-pages">
                    <button
                        type="button"
                        className="pmt-button secondary"
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
                        {filtered.length} lượt
                    </span>

                    <button
                        type="button"
                        className="pmt-button secondary"
                        disabled={
                            page >=
                            totalPages
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
                    <div className="pmt-overlay">
                        <div className="pmt-modal">
                            <h2>
                                {creating
                                    ? 'Thêm lượt chi trả giả lập'
                                    : simulation
                                        ? 'Xem / chỉnh sửa chi trả'
                                        : 'Chi tiết chi trả'}
                            </h2>

                            <div className="pmt-grid">

                                <div className="pmt-field">
                                    <label>
                                        MÃ CHI TRẢ
                                    </label>

                                    <input
                                        disabled={!simulation}
                                        value={form.paymentNumber}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                paymentNumber:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="pmt-field">
                                    <label>
                                        NGƯỜI NHẬN
                                    </label>

                                    <input
                                        disabled={!simulation}
                                        value={form.beneficiaryName}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                beneficiaryName:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="pmt-field">
                                    <label>
                                        SỐ TIỀN
                                    </label>

                                    <input
                                        disabled={!simulation}
                                        type="number"
                                        min="0"
                                        value={form.amount}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                amount:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="pmt-field">
                                    <label>
                                        HÌNH THỨC
                                    </label>

                                    <select
                                        disabled={!simulation}
                                        value={form.method}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                method:
                                                    e.target.value,
                                            })
                                        }
                                    >
                                        <option value="Cash">
                                            Tiền mặt
                                        </option>

                                        <option value="BankTransfer">
                                            Chuyển khoản
                                        </option>
                                    </select>
                                </div>

                                <div className="pmt-field">
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
                                        {paymentStatuses.map(
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

                                <div className="pmt-field">
                                    <label>
                                        ĐIỂM CHI TRẢ
                                    </label>

                                    <select
                                        disabled={!simulation}
                                        value={form.paymentPointName}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                paymentPointName:
                                                    e.target.value,
                                            })
                                        }
                                    >
                                        {demoPaymentPoints.map(
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

                                <div className="pmt-field">
                                    <label>
                                        NGÀY DỰ KIẾN
                                    </label>

                                    <input
                                        disabled={!simulation}
                                        type="date"
                                        value={form.scheduledDate}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                scheduledDate:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="pmt-field">
                                    <label>
                                        NGÀY THỰC CHI
                                    </label>

                                    <input
                                        disabled={!simulation}
                                        type="date"
                                        value={form.actualPaymentDate}
                                        onChange={(e) =>
                                            setForm({
                                                ...form,
                                                actualPaymentDate:
                                                    e.target.value,
                                            })
                                        }
                                    />
                                </div>

                                <div className="pmt-field full">
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

                            <div className="pmt-modal-actions">
                                <button
                                    type="button"
                                    className="pmt-button secondary"
                                    onClick={closeModal}
                                >
                                    Đóng
                                </button>

                                {simulation && (
                                    <button
                                        type="button"
                                        className="pmt-button"
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

export default PaymentsPage