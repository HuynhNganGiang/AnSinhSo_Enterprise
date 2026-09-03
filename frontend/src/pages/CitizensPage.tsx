import './CitizensPage.css'
import { useEffect, useState } from 'react'
import AppLayout from '../layouts/AppLayout'

type Citizen = {
    id: string
    fullName: string
    citizenNumber: string
    birthDate: string
    gender: number
    phoneNumber: string
    address: string
    email: string
    status: number
}

type CitizenPagedResponse = {
    data?: {
        items?: Citizen[]
        page?: number
        pageSize?: number
        totalCount?: number
        totalPages?: number
        hasNextPage?: boolean
        hasPreviousPage?: boolean
    }
    success?: boolean
}

function CitizensPage() {
    const [citizens, setCitizens] = useState<Citizen[]>([])
    const [totalCount, setTotalCount] = useState(0)
    const [page, setPage] = useState(1)
    const [totalPages, setTotalPages] = useState(1)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState('')

    useEffect(() => {
        const accessToken = localStorage.getItem('accessToken')

        const loadCitizens = async () => {
            setIsLoading(true)
            setError('')

            try {
                const response = await fetch(
                    `/api/v1/citizens?page=${page}&pageSize=10`,
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )

                if (!response.ok) {
                    setError('Không thể tải danh sách người dân.')
                    return
                }

                const body = (await response.json()) as CitizenPagedResponse

                setCitizens(body.data?.items ?? [])
                setTotalCount(body.data?.totalCount ?? 0)
                setTotalPages(body.data?.totalPages ?? 1)
            } catch {
                setError('Không thể kết nối đến hệ thống.')
            } finally {
                setIsLoading(false)
            }
        }

        void loadCitizens()
    }, [page])

    const formatBirthDate = (birthDate?: string | null) => {
        if (!birthDate) {
            return 'Chưa cập nhật'
        }

        const date = new Date(birthDate)

        if (Number.isNaN(date.getTime())) {
            return 'Chưa cập nhật'
        }

        return new Intl.DateTimeFormat('vi-VN').format(date)
    }

    return (
        <AppLayout>
            <div className="citizens-page">
                <div className="citizens-heading">
                    <div>
                        <h1>Quản lý người dân</h1>
                        <p>Danh sách và thông tin người dân tại xã Sông Lũy</p>
                    </div>

                    <button className="citizens-add-button" type="button">
                        <span>+</span>
                        Thêm người dân
                    </button>
                </div>

                <div className="citizens-toolbar">
                    <div className="citizens-search">
                        <span>⌕</span>
                        <input
                            type="search"
                            placeholder="Tìm theo họ tên, CCCD..."
                            aria-label="Tìm kiếm người dân"
                        />
                    </div>

                    <select
                        className="citizens-filter"
                        aria-label="Lọc theo trạng thái"
                        defaultValue=""
                    >
                        <option value="">Tất cả trạng thái</option>
                        <option value="1">Đang hoạt động</option>
                        <option value="2">Ngừng hoạt động</option>
                    </select>
                </div>

                {isLoading && <p>Đang tải dữ liệu...</p>}
                {error && <p>{error}</p>}

                {!isLoading && !error && (
                    <div className="citizens-table-card">
                        <div className="citizens-table-header">
                            <div>
                                <strong>Danh sách người dân</strong>
                                <span>
                                    Tổng cộng {totalCount.toLocaleString('vi-VN')} người dân
                                </span>
                            </div>
                        </div>

                        <div className="citizens-table-wrapper">
                            <table className="citizens-table">
                                <thead>
                                    <tr>
                                        <th>Họ và tên</th>
                                        <th>CCCD</th>
                                        <th>Ngày sinh</th>
                                        <th>Giới tính</th>
                                        <th>Trạng thái</th>
                                        <th>Thao tác</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {citizens.map((citizen) => (
                                        <tr key={citizen.id}>
                                            <td>
                                                <span className="citizens-name">
                                                    {citizen.fullName}
                                                </span>
                                            </td>

                                            <td>{citizen.citizenNumber}</td>

                                            <td>{formatBirthDate(citizen.birthDate)}</td>

                                            <td>
                                                {citizen.gender === 1
                                                    ? 'Nam'
                                                    : citizen.gender === 2
                                                        ? 'Nữ'
                                                        : 'Khác'}
                                            </td>

                                            <td>
                                                <span
                                                    className={`citizen-status ${citizen.status === 1 ? 'active' : 'inactive'
                                                        }`}
                                                >
                                                    {citizen.status === 1
                                                        ? 'Đang hoạt động'
                                                        : 'Ngừng hoạt động'}
                                                </span>
                                            </td>

                                            <td>
                                                <button
                                                    className="citizen-view-button"
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

                        <div className="citizens-pagination">
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
                )}
            </div>
        </AppLayout>
    )
}

export default CitizensPage