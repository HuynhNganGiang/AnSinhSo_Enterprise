import "./MapPage.css";
import { useEffect, useMemo, useState } from "react";
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import L from "leaflet";
import AppLayout from "../layouts/AppLayout";

type MapMarker = {
  id: string;
  markerType: string;
  name: string;
  latitude: number;
  longitude: number;
  color: string;
  popupTitle: string;
  popupContent: string;
  status: string;
};

type MapStatistics = {
  citizenCount: number;
  householdCount: number;
  poorCount: number;
  nearPoorCount: number;
  paymentPointCount: number;
  welfareCount: number;
};

type MarkerFilter = "all" | "Citizen" | "Household" | "PaymentPoint" | "Welfare";

const markerIcon = L.icon({
  iconUrl:
    "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-icon.png",
  iconRetinaUrl:
    "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-icon-2x.png",
  shadowUrl:
    "https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-shadow.png",
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

const filters: Array<{ value: MarkerFilter; label: string }> = [
  { value: "all", label: "Tất cả" },
  { value: "Citizen", label: "Người dân" },
  { value: "Household", label: "Hộ gia đình" },
  { value: "PaymentPoint", label: "Điểm chi trả" },
  { value: "Welfare", label: "An sinh" },
];

export default function MapPage() {
  const [markers, setMarkers] = useState<MapMarker[]>([]);
  const [statistics, setStatistics] = useState<MapStatistics | null>(null);
  const [activeFilter, setActiveFilter] = useState<MarkerFilter>("all");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadMapData = async () => {
      try {
        const token = localStorage.getItem("accessToken");
        const headers = token
          ? {
              Authorization: `Bearer ${token}`,
            }
          : undefined;

        const [markersResponse, statisticsResponse] = await Promise.all([
          fetch("/api/v1/map", { headers }),
          fetch("/api/v1/map/statistics", { headers }),
        ]);

        if (!markersResponse.ok) {
          throw new Error(`Map HTTP ${markersResponse.status}`);
        }

        if (!statisticsResponse.ok) {
          throw new Error(`Statistics HTTP ${statisticsResponse.status}`);
        }

        const markersData = await markersResponse.json();
        const statisticsData = await statisticsResponse.json();

        setMarkers(
          Array.isArray(markersData) ? markersData : markersData?.data ?? [],
        );

        setStatistics(statisticsData?.data ?? null);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "Không thể tải dữ liệu bản đồ.",
        );
      } finally {
        setLoading(false);
      }
    };

    void loadMapData();
  }, []);

  const validMarkers = useMemo(
    () =>
      markers.filter(
        (marker) =>
          Number.isFinite(marker.latitude) &&
          Number.isFinite(marker.longitude) &&
          marker.latitude >= -90 &&
          marker.latitude <= 90 &&
          marker.longitude >= -180 &&
          marker.longitude <= 180,
      ),
    [markers],
  );

  const filteredMarkers = useMemo(
    () =>
      activeFilter === "all"
        ? validMarkers
        : validMarkers.filter((marker) => marker.markerType === activeFilter),
    [activeFilter, validMarkers],
  );

  const center: [number, number] =
    filteredMarkers.length > 0
      ? [filteredMarkers[0].latitude, filteredMarkers[0].longitude]
      : [11.21011269694565, 108.32172004484949];

  if (loading) {
    return (
      <AppLayout>
        <div className="map-state">Đang tải dữ liệu bản đồ...</div>
      </AppLayout>
    );
  }

  if (error) {
    return (
      <AppLayout>
        <div className="map-state">Lỗi tải bản đồ: {error}</div>
      </AppLayout>
    );
  }

  return (
    <AppLayout>
      <div className="map-page">
        <div className="map-heading">
          <div>
            <h1>Bản đồ số</h1>
            <p>Quản lý và theo dõi dữ liệu an sinh theo vị trí tại xã Sông Lũy</p>
          </div>
        </div>

        <div className="map-kpis">
          <div className="map-kpi">
            <span className="map-kpi-label">Người dân có tọa độ</span>
            <strong className="map-kpi-value">
              {statistics?.citizenCount ?? 0}
            </strong>
          </div>

          <div className="map-kpi">
            <span className="map-kpi-label">Hộ gia đình có tọa độ</span>
            <strong className="map-kpi-value">
              {statistics?.householdCount ?? 0}
            </strong>
          </div>

          <div className="map-kpi">
            <span className="map-kpi-label">Điểm chi trả</span>
            <strong className="map-kpi-value">
              {statistics?.paymentPointCount ?? 0}
            </strong>
          </div>

          <div className="map-kpi">
            <span className="map-kpi-label">Hồ sơ an sinh</span>
            <strong className="map-kpi-value">
              {statistics?.welfareCount ?? 0}
            </strong>
          </div>
        </div>

        <div className="map-toolbar">
          <div className="map-filter-group">
            {filters.map((filter) => (
              <button
                key={filter.value}
                type="button"
                className={`map-filter-button ${
                  activeFilter === filter.value ? "active" : ""
                }`}
                onClick={() => setActiveFilter(filter.value)}
              >
                {filter.label}
              </button>
            ))}
          </div>

          <div className="map-count">
            Hiển thị {filteredMarkers.length} điểm bản đồ
          </div>
        </div>

        <div className="map-card">
          <MapContainer
            center={center}
            zoom={14}
            className="map-container"
          >
            <TileLayer
              attribution="&copy; OpenStreetMap contributors"
              url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />

            {filteredMarkers.map((marker) => (
              <Marker
                key={marker.id}
                position={[marker.latitude, marker.longitude]}
                icon={markerIcon}
              >
                <Popup>
                  <strong className="map-popup-title">
                    {marker.popupTitle || marker.name}
                  </strong>

                  <div className="map-popup-content">
                    {marker.popupContent}
                  </div>

                  <div className="map-status">
                    Trạng thái: {marker.status}
                  </div>
                </Popup>
              </Marker>
            ))}
          </MapContainer>
        </div>
      </div>
    </AppLayout>
  );
}
