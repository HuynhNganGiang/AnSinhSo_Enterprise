import './WelfareCasesPage.css'
import { useEffect, useState } from 'react'
import AppLayout from '../layouts/AppLayout'

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
}

type WelfareCasePagedResponse = {
    data?: {
        items?: WelfareCase[]
        page?: number
        pageSize?: number
        totalCount?: number
        totalPages?: number
        hasNextPage?: boolean
        hasPreviousPage?: boolean
    }
    success?: boolean
}

const formatMoney = (value?: number | null) => {
    if (value == null) return 'Chưa cập nhật'

    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND',
    }).format(value)
}

const formatDate = (value?: string | null) => {
    if (!value) return 'Chưa cập nhật'

    const date = new Date(value)

    if (Number.isNaN(date.getTime())) return 'Chưa cập nhật'

    return new Intl.DateTimeFormat('vi-VN').format(date)
}

const translateStatus = (status: string) => {
    const normalized = status.toLowerCase()

    if (normalized === 'draft') return 'Nháp'
    if (normalized === 'submitted') return 'Đã nộp'
    if (normalized === 'approved') return 'Đã duyệt'
    if (normalized === 'rejected') return 'Từ chối'
    if (normalized === 'cancelled') return 'Đã hủy'
    if (normalized === 'closed') return 'Đã đóng'

    return status
}

function WelfareCasesPage() {
    const [welfareCases, setWelfareCases] = useState<WelfareCase[]>([])
    const [totalCount, setTotalCount] = useState(0)
    const [page, setPage] = useState(1)
    const [totalPages, setTotalPages] = useState(1)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState('')

    useEffect(() => {
        const accessToken = localStorage.getItem('accessToken')

        const loadWelfareCases = async () => {
            setIsLoading(true)
            setError('')

            try {
                const response = await fetch(
                    `/api/v1/welfarecases?page=${page}&pageSize=10`,
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )

                if (!response.ok) {
                    setError('Không thể tải danh sách hồ sơ an sinh.')
                    return
                }

                const body = (await response.json()) as WelfareCasePagedResponse

                setWelfareCases(body.data?.items ?? [])
                setTotalCount(body.data?.totalCount ?? 0)
                setTotalPages(body.data?.totalPages ?? 1)
            } catch {
                setError('Không thể kết nối đến máy chủ.')
            } finally {
                setIsLoading(false)
            }
        }

        void loadWelfareCases()
    }, [page])

    return (
        <AppLayout>
            <div className="welfare-page">
                <div className="welfare-heading">
                    <div>
                        <h1>An sinh xã hội</h1>
                        <p>Quản lý hồ sơ và chính sách an sinh tại xã Sông Lũy</p>
                    </div>

                    <button type="button" className="welfare-add-button">
                        + Thêm hồ sơ
                    </button>
                </div>

                <div className="welfare-summary">
                    <span>Tổng số hồ sơ</span>
                    <strong>{totalCount}</strong>
                </div>

                <div className="welfare-table-card">
                    {isLoading && (
                        <div className="welfare-state">Đang tải dữ liệu...</div>
                    )}

                    {error && <div className="welfare-state">{error}</div>}

                    {!isLoading && !error && (
                        <>
                            <div className="welfare-table-wrapper">
                                <table className="welfare-table">
                                    <thead>
                                        <tr>
                                            <th>Người dân</th>
                                            <th>CCCD</th>
                                            <th>Mức trợ cấp</th>
                                            <th>Trạng thái</th>
                                            <th>Hiệu lực từ</th>
                                            <th>Hiệu lực đến</th>
                                            <th></th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        {welfareCases.map((item) => (
                                            <tr key={item.id}>
                                                <td>
                                                    <strong>{item.citizenSnapshot.fullName}</strong>
                                                </td>
                                                <td>{item.citizenSnapshot.citizenNumber}</td>
                                                <td>{formatMoney(item.benefitAmount)}</td>
                                                <td>
                                                    <span className="welfare-status">
                                                        {translateStatus(item.status)}
                                                    </span>
                                                </td>
                                                <td>{formatDate(item.effectiveFrom)}</td>
                                                <td>{formatDate(item.effectiveTo)}</td>
                                                <td>
                                                    <button type="button" className="welfare-view-button">
                                                        Xem
                                                    </button>
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>

                            <div className="welfare-pagination">
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

export default WelfareCasesPage