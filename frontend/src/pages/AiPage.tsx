import "./AiPage.css";
import { useEffect, useState } from "react";
import AppLayout from "../layouts/AppLayout";

type AiDashboardSummary = {
  totalRecommendations: number;
  highRisk: number;
  mediumRisk: number;
  lowRisk: number;
  poorHouseholds: number;
  duplicatedBenefits: number;
  elderlyWithoutSupport: number;
  missingIdentity: number;
  reviewed: number;
  pending: number;
  topTriggeredRules: Array<{
    ruleCode: string;
    count: number;
  }>;
};

type AiRecommendation = {
  id: string;
  recommendationNumber: string;
  targetType: number;
  targetId: string;
  category: number;
  score: number;
  confidence: number;
  title: string;
  summary: string;
  status: number;
  createdAt: string;
  reasons: Array<{
    ruleCode: string;
    factor: string;
    weight: number;
    message: string;
  }>;
};

const getTargetLabel = (value: number) => {
  if (value === 1) return "Người dân";
  if (value === 2) return "Hộ gia đình";
  if (value === 3) return "Hồ sơ an sinh";
  return "Khác";
};

const getStatusLabel = (value: number) => {
  if (value === 1) return "Chờ xử lý";
  if (value === 2) return "Đã xem xét";
  if (value === 3) return "Đã bỏ qua";
  return "Không xác định";
};

export default function AiPage() {
  const [summary, setSummary] = useState<AiDashboardSummary | null>(null);
  const [recommendations, setRecommendations] = useState<AiRecommendation[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [targetFilter, setTargetFilter] = useState<"all" | 1 | 2>("all");
  const pageSize = 10;

  const filteredRecommendations =
    targetFilter === "all"
      ? recommendations
      : recommendations.filter((item) => item.targetType === targetFilter);

  const totalPages = Math.max(
    1,
    Math.ceil(filteredRecommendations.length / pageSize),
  );

  const startIndex = (currentPage - 1) * pageSize;
  const pagedRecommendations = filteredRecommendations.slice(
    startIndex,
    startIndex + pageSize,
  );

  useEffect(() => {
    const loadAiData = async () => {
      try {
        const token = localStorage.getItem("accessToken");
        const headers = token
          ? {
              Authorization: `Bearer ${token}`,
            }
          : undefined;

        const [summaryResponse, recommendationsResponse] = await Promise.all([
          fetch("/api/v1/ai/dashboard-summary", { headers }),
          fetch("/api/v1/ai/recommendations", { headers }),
        ]);

        if (!summaryResponse.ok) {
          throw new Error(`AI summary HTTP ${summaryResponse.status}`);
        }

        if (!recommendationsResponse.ok) {
          throw new Error(
            `AI recommendations HTTP ${recommendationsResponse.status}`,
          );
        }

        const summaryJson = await summaryResponse.json();
        const recommendationsJson = await recommendationsResponse.json();

        setSummary(summaryJson?.data?.value ?? null);
        setRecommendations(recommendationsJson?.data?.value ?? []);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Không thể tải dữ liệu AI.",
        );
      } finally {
        setLoading(false);
      }
    };

    void loadAiData();
  }, []);

  if (loading) {
    return (
      <AppLayout>
        <div className="ai-state">Đang tải dữ liệu AI...</div>
      </AppLayout>
    );
  }

  if (error) {
    return (
      <AppLayout>
        <div className="ai-state">Lỗi tải AI: {error}</div>
      </AppLayout>
    );
  }

  return (
    <AppLayout>
      <div className="ai-page">
        <div className="ai-heading">
          <div>
            <h1>AI & Phân tích</h1>
            <p>
              Hỗ trợ phát hiện và đề xuất các trường hợp an sinh cần ưu tiên.
            </p>
          </div>
        </div>

        <div className="ai-kpis">
          <div className="ai-kpi">
            <span className="ai-kpi-label">Tổng đề xuất</span>
            <strong className="ai-kpi-value">
              {summary?.totalRecommendations ?? 0}
            </strong>
          </div>

          <div className="ai-kpi">
            <span className="ai-kpi-label">Nguy cơ cao</span>
            <strong className="ai-kpi-value">
              {summary?.highRisk ?? 0}
            </strong>
          </div>

          <div className="ai-kpi">
            <span className="ai-kpi-label">Chờ xử lý</span>
            <strong className="ai-kpi-value">
              {summary?.pending ?? 0}
            </strong>
          </div>

          <div className="ai-kpi">
            <span className="ai-kpi-label">Đã xem xét</span>
            <strong className="ai-kpi-value">
              {summary?.reviewed ?? 0}
            </strong>
          </div>
        </div>

        <section className="ai-recommendation-panel">
          <div className="ai-panel-header">
            <div>
              <span className="ai-panel-eyebrow">AI DECISION CENTER</span>
              <h2 className="ai-panel-title">
                {"Trung t\u00e2m \u0111\u1ec1 xu\u1ea5t AI"}
              </h2>
              <p className="ai-panel-description">
                {"Danh s\u00e1ch c\u00e1c tr\u01b0\u1eddng h\u1ee3p \u0111\u01b0\u1ee3c h\u1ec7 th\u1ed1ng ph\u00e2n t\u00edch v\u00e0 x\u1ebfp h\u1ea1ng \u01b0u ti\u00ean."}
              </p>
            </div>
            <div className="ai-panel-total">
              <strong>{recommendations.length}</strong>
              <span>{"\u0110\u1ec1 xu\u1ea5t \u0111ang hi\u1ec3n th\u1ecb"}</span>
            </div>
          </div>

        <div className="ai-toolbar">
          <div className="ai-filter-group">
            <button
              type="button"
              className={`ai-filter-button ${targetFilter === "all" ? "active" : ""}`}
              onClick={() => {
                setTargetFilter("all");
                setCurrentPage(1);
              }}
            >
              Tất cả
            </button>
            <button
              type="button"
              className={`ai-filter-button ${targetFilter === 1 ? "active" : ""}`}
              onClick={() => {
                setTargetFilter(1);
                setCurrentPage(1);
              }}
            >
              Người dân
            </button>
            <button
              type="button"
              className={`ai-filter-button ${targetFilter === 2 ? "active" : ""}`}
              onClick={() => {
                setTargetFilter(2);
                setCurrentPage(1);
              }}
            >
              Hộ gia đình
            </button>
          </div>

          <div className="ai-count">
            Hiển thị {filteredRecommendations.length === 0 ? 0 : startIndex + 1}–
            {Math.min(startIndex + pageSize, filteredRecommendations.length)} /{" "}
            {filteredRecommendations.length} đề xuất
          </div>
        </div>

        <div className="ai-list">
          {filteredRecommendations.length === 0 ? (
            <div className="ai-empty">Chưa có đề xuất AI.</div>
          ) : (
            pagedRecommendations.map((item) => (
              <div key={item.id} className="ai-card">
                <div className="ai-card-main">
                  <h2 className="ai-card-title">{item.title}</h2>

                  <p className="ai-card-summary">{item.summary}</p>

                  <div className="ai-card-meta">
                    <span className="ai-badge">
                      {getTargetLabel(item.targetType)}
                    </span>

                    {item.confidence === 3 && (
                      <span className="ai-badge ai-badge-high">
                        Độ tin cậy cao
                      </span>
                    )}

                    <span
                      className={`ai-badge ${
                        item.status === 1
                          ? "ai-badge-pending"
                          : item.status === 2
                            ? "ai-badge-reviewed"
                            : ""
                      }`}
                    >
                      {getStatusLabel(item.status)}
                    </span>
                  </div>
                </div>

                <div className="ai-score-block">
                  <div className="ai-score">{item.score}</div>
                  <span className="ai-score-label">
                    {"\u0110i\u1ec3m \u01b0u ti\u00ean"}
                  </span>
                </div>
              </div>
            ))
          )}
        </div>

        {filteredRecommendations.length > 0 && (
          <div className="ai-pagination">
            <button
              type="button"
              className="ai-page-button"
              disabled={currentPage === 1}
              onClick={() => setCurrentPage((page) => Math.max(1, page - 1))}
            >
              Trước
            </button>

            <span className="ai-page-info">
              Trang {currentPage} / {totalPages}
            </span>

            <button
              type="button"
              className="ai-page-button"
              disabled={currentPage === totalPages}
              onClick={() =>
                setCurrentPage((page) => Math.min(totalPages, page + 1))
              }
            >
              Sau
            </button>
          </div>
        )}
        </section>
      </div>
    </AppLayout>
  );
}
