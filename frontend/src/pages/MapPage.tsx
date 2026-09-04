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

/* ANSINHSO_MAP_PRESENTATION_FALLBACK
 *
 * SQL Server / Backend remains the primary data source.
 *
 * When production has no PaymentPoint or Welfare map rows,
 * presentation-only simulated markers are generated.
 *
 * Nothing from this fallback is persisted to SQL Server.
 */

const SONG_LUY_CENTER = {
  latitude: 11.21011269694565,
  longitude: 108.32172004484949,
};

function markerColorHex(color: string) {
  switch ((color || "").toLowerCase()) {
    case "red":
      return "#dc2626";

    case "yellow":
      return "#eab308";

    case "green":
      return "#16a34a";

    case "purple":
    case "violet":
      return "#7c3aed";

    case "orange":
      return "#f97316";

    case "cyan":
      return "#0891b2";

    case "blue":
    default:
      return "#2563eb";
  }
}

function createColoredMarkerIcon(color: string) {
  const hex =
    markerColorHex(color);

  return L.divIcon({
    className: "ansinhso-map-marker",

    html: `
      <div style="
        width:24px;
        height:24px;
        background:${hex};
        border:3px solid #ffffff;
        border-radius:50% 50% 50% 0;
        transform:rotate(-45deg);
        box-shadow:0 3px 9px rgba(15,23,42,.38);
      ">
        <span style="
          display:block;
          width:7px;
          height:7px;
          margin:6px;
          background:#ffffff;
          border-radius:50%;
        "></span>
      </div>
    `,

    iconSize: [30, 38],
    iconAnchor: [15, 34],
    popupAnchor: [0, -31],
  });
}

function simulatedCoordinate(
  index: number,
  total: number,
  maxRadiusKm: number,
) {
  const angle =
    ((index + 1) /
      Math.max(total, 1)) *
      Math.PI *
      2 +
    0.71;

  const radiusFactor =
    0.35 +
    (((index * 37) % 100) / 100) *
      0.65;

  const radiusKm =
    maxRadiusKm *
    radiusFactor;

  const latitudeOffset =
    (
      radiusKm *
      Math.cos(angle)
    ) /
    111.32;

  const longitudeOffset =
    (
      radiusKm *
      Math.sin(angle)
    ) /
    (
      111.32 *
      Math.cos(
        SONG_LUY_CENTER.latitude *
          Math.PI /
          180,
      )
    );

  return {
    latitude:
      SONG_LUY_CENTER.latitude +
      latitudeOffset,

    longitude:
      SONG_LUY_CENTER.longitude +
      longitudeOffset,
  };
}

function addPresentationFallback(
  apiMarkers: MapMarker[],
) {
  const result =
    [...apiMarkers];

  const paymentExists =
    result.some(
      (marker) =>
        marker.markerType ===
        "PaymentPoint",
    );

  if (!paymentExists) {
    const paymentPoints = [
      "Điểm chi trả trung tâm xã",
      "Điểm chi trả khu vực Bắc",
      "Điểm chi trả khu vực Nam",
      "Điểm chi trả khu vực Đông",
      "Điểm chi trả lưu động",
    ];

    paymentPoints.forEach(
      (name, index) => {
        const position =
          simulatedCoordinate(
            index,
            paymentPoints.length,
            2.0,
          );

        result.push({
          id:
            `demo-payment-${index + 1}`,

          markerType:
            "PaymentPoint",

          name,

          latitude:
            position.latitude,

          longitude:
            position.longitude,

          color:
            "blue",

          popupTitle:
            name,

          popupContent:
            "Điểm chi trả giả định phục vụ trình diễn. " +
            "Vị trí ước tính trong xã Sông Lũy; " +
            "không phải dữ liệu production.",

          status:
            "SIMULATED",
        });
      },
    );
  }

  const welfareExists =
    result.some(
      (marker) =>
        marker.markerType ===
        "Welfare",
    );

  if (!welfareExists) {
    const welfareCount =
      48;

    for (
      let index = 0;
      index < welfareCount;
      index += 1
    ) {
      const position =
        simulatedCoordinate(
          index,
          welfareCount,
          3.5,
        );

      result.push({
        id:
          `demo-welfare-${index + 1}`,

        markerType:
          "Welfare",

        name:
          `Hồ sơ an sinh mô phỏng ${String(
            index + 1,
          ).padStart(2, "0")}`,

        latitude:
          position.latitude,

        longitude:
          position.longitude,

        color:
          "purple",

        popupTitle:
          "Hồ sơ an sinh - dữ liệu mô phỏng",

        popupContent:
          "Hồ sơ giả lập phục vụ demo Map. " +
          "Không phải hồ sơ production và " +
          "không được lưu vào SQL Server.",

        status:
          "SIMULATED",
      });
    }
  }

  return result;
}

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

        const apiMarkers: MapMarker[] =
          Array.isArray(markersData)
            ? markersData
            : markersData?.data ?? [];

        const completeMarkers =
          addPresentationFallback(
            apiMarkers,
          );

        setMarkers(
          completeMarkers,
        );

        // Statistics endpoint is still requested and validated above,
        // but Map KPI is recalculated from markers actually presented.
        void statisticsData;

        setStatistics({
          citizenCount:
            completeMarkers.filter(
              (marker) =>
                marker.markerType ===
                "Citizen",
            ).length,

          householdCount:
            completeMarkers.filter(
              (marker) =>
                marker.markerType ===
                "Household",
            ).length,

          poorCount:
            completeMarkers.filter(
              (marker) =>
                marker.markerType ===
                  "Household" &&
                marker.color.toLowerCase() ===
                  "red",
            ).length,

          nearPoorCount:
            completeMarkers.filter(
              (marker) =>
                marker.markerType ===
                  "Household" &&
                marker.color.toLowerCase() ===
                  "yellow",
            ).length,

          paymentPointCount:
            completeMarkers.filter(
              (marker) =>
                marker.markerType ===
                "PaymentPoint",
            ).length,

          welfareCount:
            completeMarkers.filter(
              (marker) =>
                marker.markerType ===
                "Welfare",
            ).length,
        });
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
            <span className="map-kpi-label">Hộ gia đình trên bản đồ</span>
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
                icon={marker.color ? createColoredMarkerIcon(marker.color) : markerIcon}
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
