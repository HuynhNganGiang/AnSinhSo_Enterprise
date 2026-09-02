import { useEffect, useState } from 'react'
import AppLayout from '../layouts/AppLayout'
import './HouseholdsPage.css'

type Household = {
    id: string
    householdCode: string
    headCitizenName: string
    status: number
    memberCount: number
}

type HouseholdPagedResponse = {
    data?: {
        items?: Household[]
        page?: number
        pageSize?: number
        totalCount?: number
        totalPages?: number
        hasNextPage?: boolean
        hasPreviousPage?: boolean
    }
    success?: boolean
}

function HouseholdsPage() {
    const [households, setHouseholds] = useState<Household[]>([])
    const [totalCount, setTotalCount] = useState(0)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState('')
    const [page, setPage] = useState(1)
    const [totalPages, setTotalPages] = useState(1)

    useEffect(() => {
        const accessToken = localStorage.getItem('accessToken')

        const loadHouseholds = async () => {
            try {
                const response = await fetch(`/api/v1/households?page=${page}&pageSize=10`, {
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                    },
                })

                if (!response.ok) {
                    setError('Không thể tải danh sách hộ gia đình.')
                    return
                }

                const body = (await response.json()) as HouseholdPagedResponse

                setHouseholds(body.data?.items ?? [])
                setTotalCount(body.data?.totalCount ?? 0)
                setTotalPages(body.data?.totalPages ?? 1)
            } catch {
                setError('Không thể kết nối đến hệ thống.')
            } finally {
                setIsLoading(false)
            }
        }

        void loadHouseholds()
    }, [page])

    return (
        <AppLayout>
            <div className="households-sticky-top">
                <div className="households-heading">
                    <div>
                        <h1>Quản lý hộ gia đình</h1>
                        <p>Danh sách và thông tin các hộ gia đình tại xã Sông Lũy</p>
                    </div>

                    <button className="households-add-button" type="button">
                        <span>+</span>
                        Thêm hộ gia đình
                    </button>
                </div>

                <div className="households-toolbar">
                    <div className="households-search">
                        <span>⌕</span>
                        <input
                            type="search"
                            placeholder="Tìm theo mã hộ, tên chủ hộ..."
                            aria-label="Tìm kiếm hộ gia đình"
                        />
                    </div>

                    <select
                        className="households-filter"
                        aria-label="Lọc theo trạng thái"
                        defaultValue=""
                    >
                        <option value="">Tất cả trạng thái</option>
                        <option value="1">Đang hoạt động</option>
                        <option value="2">Ngừng hoạt động</option>
                    </select>
                </div>
            </div>

            {isLoading && <p>Đang tải dữ liệu...</p>}

            {error && <p>{error}</p>}

            {!isLoading && !error && (
                <>
                    <div className="households-table-card">
                        <div className="households-table-header">
                            <div>
                                <strong>Danh sách hộ gia đình</strong>
                                <span>
                                    Tổng cộng {totalCount.toLocaleString('vi-VN')} hộ
                                </span>
                            </div>
                        </div>

                        <div className="households-table-wrapper">
                            <table className="households-table">
                                <thead>
                                    <tr>
                                        <th>Mã hộ</th>
                                        <th>Chủ hộ</th>
                                        <th>Số thành viên</th>
                                        <th>Trạng thái</th>
                                        <th>Thao tác</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {households.map((household) => (
                                        <tr key={household.id}>
                                            <td>
                                                <strong>{household.householdCode}</strong>
                                            </td>

                                            <td>{household.headCitizenName}</td>

                                            <td>{household.memberCount}</td>

                                            <td>
                                                <span className="household-status">
                                                    {household.status === 1
                                                        ? 'Đang hoạt động'
                                                        : 'Ngừng hoạt động'}
                                                </span>
                                            </td>

                                            <td>
                                                <button
                                                    className="household-view-button"
                                                    type="button"
                                                >
                                                    Xem
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                        <div className="households-pagination">
                            <span>
                                Trang {page} / {totalPages}
                            </span>

                            <div>
                                <button
                                    type="button"
                                    disabled={page <= 1}
                                    onClick={() => setPage((current) => current - 1)}
                                >
                                    ← Trước
                                </button>

                                <button
                                    type="button"
                                    disabled={page >= totalPages}
                                    onClick={() => setPage((current) => current + 1)}
                                >
                                    Sau →
                                </button>
                            </div>
                        </div>
                    </div>
                </>
            )}
        </AppLayout >
    )
}

export default HouseholdsPage