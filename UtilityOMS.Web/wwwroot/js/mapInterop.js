// ====================================
// mapInterop.js - Leaflet Map Interop
// ====================================

window.mapInterop = {
    map: null,
    markers: {},

    // ✅ Initialize the map
    initMap: function (lat, lng, zoom) {
        // ✅ Destroy existing map if any
        if (this.map) {
            this.map.remove();
            this.map = null;
            this.markers = {};
        }

        // ✅ Make sure container exists
        const container = document.getElementById('outage-map');
        if (!container) return;

        // ✅ Clear any leftover Leaflet init flag
        delete container._leaflet_id;

        this.map = L.map('outage-map', {
            center: [lat, lng],
            zoom: zoom,
            zoomControl: true,
            attributionControl: true
        });

        // ✅ Free OpenStreetMap tiles
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors',
            maxZoom: 19
        }).addTo(this.map);

        // ✅ Force correct size after render
        setTimeout(() => {
            if (this.map) this.map.invalidateSize();
        }, 200);

        this.markers = {};
    },

    // ✅ Get pin color based on outage status
    getMarkerColor: function (status) {
        switch (status) {
            case 'Reported': return '#e74c3c'; // Red
            case 'InProgress': return '#f39c12'; // Orange
            case 'Resolved': return '#27ae60'; // Green
            default: return '#888888';
        }
    },

    // ✅ Create custom colored pin
    createIcon: function (status) {
        const color = this.getMarkerColor(status);
        return L.divIcon({
            className: '',
            html: `
                <div style="
                    position: relative;
                    width: 36px;
                    height: 36px;
                ">
                    <div style="
                        width: 36px;
                        height: 36px;
                        background: ${color};
                        border-radius: 50% 50% 50% 0;
                        transform: rotate(-45deg);
                        border: 3px solid white;
                        box-shadow: 0 3px 10px rgba(0,0,0,0.3);
                    "></div>
                    <div style="
                        position: absolute;
                        top: 7px;
                        left: 7px;
                        width: 16px;
                        height: 16px;
                        background: white;
                        border-radius: 50%;
                        transform: rotate(45deg);
                    "></div>
                </div>`,
            iconSize: [36, 36],
            iconAnchor: [18, 36],
            popupAnchor: [0, -36]
        });
    },

    // ✅ Build popup HTML
    buildPopup: function (outage) {
        const statusColor = this.getMarkerColor(outage.status);
        const resolved = outage.resolvedAt
            ? `<div style="color:#27ae60; font-size:0.78rem; margin-top:4px;">
                   ✅ Resolved: ${new Date(outage.resolvedAt).toLocaleString()}
               </div>`
            : '';
        return `
            <div style="font-family:Inter,sans-serif; min-width:220px; padding:4px;">
                <div style="display:flex; align-items:center; 
                            justify-content:space-between; margin-bottom:8px;">
                    <strong style="color:#1a1f3c; font-size:0.95rem;">
                        ${outage.title}
                    </strong>
                    <span style="background:${statusColor}22; color:${statusColor};
                                 padding:3px 10px; border-radius:20px; 
                                 font-size:0.72rem; font-weight:700;">
                        ${outage.status}
                    </span>
                </div>
                <div style="font-size:0.8rem; color:#666; margin-bottom:6px;">
                    ${outage.description || 'No description provided.'}
                </div>
                <hr style="margin:8px 0; border-color:#f0f0f0;" />
                <div style="font-size:0.78rem; color:#888;">
                    <div>⚡ <strong>Cause:</strong> ${outage.cause}</div>
                    <div>📍 <strong>Area:</strong> ${outage.affectedArea || 'N/A'}</div>
                    <div>👥 <strong>Affected:</strong> ${outage.affectedCustomers} customers</div>
                    <div>👷 <strong>Crew:</strong> ${outage.assignedCrewName || 'Unassigned'}</div>
                    <div>🕒 <strong>Reported:</strong> 
                        ${new Date(outage.reportedAt).toLocaleString()}
                    </div>
                    ${resolved}
                </div>
            </div>`;
    },

    // ✅ Add or update a marker on the map
    addOrUpdateMarker: function (outage) {
        if (!this.map) return;

        // Remove old marker if exists
        if (this.markers[outage.id]) {
            this.map.removeLayer(this.markers[outage.id]);
            delete this.markers[outage.id];
        }

        // Skip resolved outages with default coords
        if (outage.latitude === 0 && outage.longitude === 0) return;

        const marker = L.marker(
            [outage.latitude, outage.longitude],
            { icon: this.createIcon(outage.status) }
        ).addTo(this.map);

        marker.bindPopup(this.buildPopup(outage), {
            maxWidth: 280,
            className: 'custom-popup'
        });

        // Animate marker on first add
        marker.on('add', function () {
            const el = marker.getElement();
            if (el) {
                el.style.transition = 'transform 0.3s ease';
                el.style.transform = 'scale(0)';
                setTimeout(() => { el.style.transform = 'scale(1)'; }, 10);
            }
        });

        this.markers[outage.id] = marker;
    },

    // ✅ Remove a marker from the map
    removeMarker: function (outageId) {
        if (this.markers[outageId]) {
            this.map.removeLayer(this.markers[outageId]);
            delete this.markers[outageId];
        }
    },

    // ✅ Load all initial outages onto the map
    loadOutages: function (outages) {
        if (!this.map) return;

        // ✅ Force correct tile rendering
        this.map.invalidateSize();

        outages.forEach(o => this.addOrUpdateMarker(o));
    },

    // ✅ Fly to a specific outage on the map
    flyToOutage: function (lat, lng) {
        if (this.map) {
            this.map.flyTo([lat, lng], 14, {
                animate: true,
                duration: 1.2
            });
        }
    },

    // ✅ Fit map to show all markers
    fitAllMarkers: function () {
        if (!this.map) return;
        const keys = Object.keys(this.markers);
        if (keys.length === 0) return;
        const group = L.featureGroup(
            keys.map(k => this.markers[k])
        );
        this.map.fitBounds(group.getBounds().pad(0.2));
    },

    // ✅ Destroy map (cleanup)
    destroyMap: function () {
        if (this.map) {
            this.map.remove();
            this.map = null;
            this.markers = {};
        }
    }
};