import './PaymentsPage.css'
import { useEffect, useState } from 'react'
import AppLayout from '../layouts/AppLayout'

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
}

type PaymentPagedResponse = {
    data?: {
        items?: Payment[]
        page?: number
        pageSize?: number
        totalCount?: number
        totalPages?: number
        hasNextPage?: boolean
        hasPreviousPage?: boolean
    }
    success?: boolean
}

const formatMoney = (value: number) =>
    new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND',
    }).format(value)

const formatDate = (value?: string | null) => {
    if (!value) return 'Chưa cập nhật'

    const date = new Date(value)

    if (Number.isNaN(date.getTime())) return 'Chưa cập nhật'

    return new Intl.DateTimeFormat('vi-VN').format(date)
}

const translateMethod = (method: string) => {
    if (method.toLowerCase() === 'cash') return 'Tiền mặt'
    return method
}

const translateStatus = (status: string) => {
    const normalized = status.toLowerCase()

    if (normalized === 'paid') return 'Đã chi trả'
    if (normalized === 'pending') return 'Chờ chi trả'
    if (normalized === 'cancelled') return 'Đã hủy'
    if (normalized === 'failed') return 'Thất bại'

    return status
}

function PaymentsPage() {
    const [payments, setPayments] = useState<Payment[]>([])
    const [totalCount, setTotalCount] = useState(0)
    const [page, setPage] = useState(1)
    const [totalPages, setTotalPages] = useState(1)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState('')

    useEffect(() => {
        const accessToken = localStorage.getItem('accessToken')

        const loadPayments = async () => {
            setIsLoading(true)
            setError('')

            try {
                const response = await fetch(
                    `/api/v1/payments?page=${page}&pageSize=10`,
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )

                if (!response.ok) {
                    setError('Không thể tải danh sách chi trả trợ cấp.')
                    return
                }

                const body = (await response.json()) as PaymentPagedResponse

                setPayments(body.data?.items ?? [])
                setTotalCount(body.data?.totalCount ?? 0)
                setTotalPages(body.data?.totalPages ?? 1)
            } catch {
                setError('Không thể kết nối đến máy chủ.')
            } finally {
                setIsLoading(false)
            }
        }

        void loadPayments()
    }, [page])

    return (
        <AppLayout>
            <div className="payments-page">
                <div className="payments-heading">
                    <div>
                        <h1>Chi trả trợ cấp</h1>
                        <p>Quản lý và theo dõi các khoản chi trả trợ cấp tại xã Sông Lũy</p>
                    </div>

                    <button type="button" className="payments-add-button">
                        + Thêm chi trả
                    </button>
                </div>

                <div className="payments-summary">
                    <span>Tổng số khoản chi trả</span>
                    <strong>{totalCount}</strong>
                </div>

                <div className="payments-table-card">
                    {isLoading && (
                        <div className="payments-state">Đang tải dữ liệu...</div>
                    )}

                    {error && <div className="payments-state">{error}</div>}

                    {!isLoading && !error && (
                        <>
                            <div className="payments-table-wrapper">
                                <table className="payments-table">
                                    <thead>
                                        <tr>
                                            <th>Mã chi trả</th>
                                            <th>Số tiền</th>
                                            <th>Phương thức</th>
                                            <th>Trạng thái</th>
                                            <th>Ngày dự kiến</th>
                                            <th>Ngày thực chi</th>
                                            <th>Ghi chú</th>
                                            <th></th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        {payments.map((item) => (
                                            <tr key={item.id}>
                                                <td>
                                                    <strong>{item.paymentNumber}</strong>
                                                </td>
                                                <td>{formatMoney(item.amount)}</td>
                                                <td>{translateMethod(item.method)}</td>
                                                <td>
                                                    <span className="payments-status">
                                                        {translateStatus(item.status)}
                                                    </span>
                                                </td>
                                                <td>{formatDate(item.scheduledDate)}</td>
                                                <td>{formatDate(item.actualPaymentDate)}</td>
                                                <td>{item.notes || '—'}</td>
                                                <td>
                                                    <button type="button" className="payments-view-button">
                                                        Xem
                                                    </button>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>

                            <div className="payments-pagination">
                                <span>
                                    Trang {page} / {totalPages}
                                </span>

                                <div>
                                    <button
                                        type="button"
                                        disabled={page <= 1}
                                        onClick={() => setPage((current) => current - 1)}
                                    >
                                        Trước
                                    </button>

                                    <button
                                        type="button"
                                        disabled={page >= totalPages}
                                        onClick={() => setPage((current) => current + 1)}
                                    >
                                        Sau
                                    </button>
                                </div>
                            </div>
                        </>
                    )}
                </div>
            </div>
        </AppLayout>
    )
}

export default PaymentsPage