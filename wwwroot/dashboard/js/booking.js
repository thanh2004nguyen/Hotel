function scrollToSection(className) {
    document.querySelector(className).scrollIntoView({ behavior: 'smooth' });
}
function showOverlay() {
    document.getElementById('imageOverlay').style.display = 'block';
    document.body.classList.add("no-scroll");
}

function closeOverlay() {
    document.getElementById('imageOverlay').style.display = 'none';
}
const apiKey = 'R9vP5FKHeER463TiCptblQnYNeYI5fIu';

// Khởi tạo bản đồ
const map = tt.map({
    key: apiKey,
    container: 'mapLocation',
    center: [106.6758, 10.7548], // Tọa độ (longitude, latitude)
    zoom: 12
});


// Thêm một marker tại vị trí mặc định
const marker = new tt.Marker()
    .setLngLat([106.6758, 10.7548]) // Tọa độ của marker
    .addTo(map);
const name = document.getElementById('room-name-marker').innerHTML;
const starts = document.getElementById('room-starts-marker').innerHTML;
// Thêm thông tin cho marker
const popupContent = `
        <div style="width: 100px; height: 50px;"> 
          <h6>${name}</h6>
          <p>${starts}</p> 
        </div>
    `;
const popup = new tt.Popup({ offset: 15 })
    .setHTML(popupContent);
marker.setPopup(popup);
popup.addTo(map);

const categories = [
    { name: 'restaurant', icon: 'fas fa-utensils' },
    { name: 'park', icon: 'fas fa-tree' },
    { name: 'shopping', icon: 'fas fa-shopping-cart' },
    { name: 'entertainment', icon: 'fas fa-film' }
];
const baseUrl = 'https://api.tomtom.com/search/2/categorySearch/';
const lat = 10.7548;
const lon = 106.6758;
const radius = 10000;
let allLocations = []; // Thêm biến để lưu trữ tất cả các địa điểm

function fetchNearbyLocations() {
    const promises = categories.map(category => {
        const url = `${baseUrl}${category.name}.json?key=${apiKey}&lat=${lat}&lon=${lon}&radius=${radius}`;
        return fetch(url)
            .then(response => response.json())
            .then(data => {
                allLocations.push({ category: category.name, results: data.results }); // Lưu trữ tất cả địa điểm
                return {
                    category: category.name,
                    icon: category.icon,
                    results: data.results.slice(0, 3),
                };
            })
            .catch(error => {
                console.error(`Error fetching ${category.name}:`, error);
                return { category: category.name, results: [] };
            });
    });

    Promise.all(promises).then(results => {
        displayLocations(results);
    });
}

function displayLocations(results) {
    const locationsList = document.getElementById('locations-list');
    locationsList.innerHTML = '';

    results.forEach(({ category, icon, results }) => {
        if (results.length > 0) {
            const categoryHeader = document.createElement('h4');
            categoryHeader.innerHTML = `<i class="${icon}"></i> ${category.charAt(0).toUpperCase() + category.slice(1)}`;
            locationsList.appendChild(categoryHeader);

            results.forEach(result => {
                const poi = result.poi;
                const distance = Math.floor(result.dist);
                const locationHTML = `
                    <div class="location d-flex">
                        <h6 class="poi-name" onclick="showOnMap(${result.position.lat}, ${result.position.lon}, ' ${poi.name}')">
                            <span style="" class="dot"></span>• ${poi.name}
                        </h6>
                        <p>${distance} m</p>
                    </div>
                `;
                locationsList.innerHTML += locationHTML;
            });
        }
    });
}

// Hiển thị tất cả địa điểm khi nhấn nút
document.getElementById('show-all-btn').addEventListener('click', () => {
    displayAllLocations();
});

// Hàm hiển thị tất cả địa điểm
function displayAllLocations() {
    const allLocationsList = document.getElementById('all-locations-list');
    allLocationsList.innerHTML = ''; // Xóa nội dung hiện tại

    allLocations.forEach(({ category, results }) => {
        if (results.length > 0) {
            const categoryHeader = document.createElement('h4');
            categoryHeader.className = 'category-header';
            categoryHeader.textContent = category.charAt(0).toUpperCase() + category.slice(1);
            allLocationsList.appendChild(categoryHeader);

            results.forEach(result => {
                const poi = result.poi;
                const distance = Math.floor(result.dist);
                const locationHTML = `
                    <div class="location d-flex">
                       <h6 class="poi-name" onclick="showOnMap(${result.position.lat}, ${result.position.lon}, '${poi.name}')">
                          <span style="display: inline-block; max-width: 150px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">。${poi.name}</span>
                      </h6>
                      <p style="text-align:center">${distance} m</p>

                    </div>
                `;
                allLocationsList.innerHTML += locationHTML;
            });
        }
    });

    // Hiển thị modal
    document.getElementById('all-locations-modal').style.display = "block";
}

// Đóng modal khi nhấn vào nút đóng
document.querySelector('.close').addEventListener('click', () => {
    document.getElementById('all-locations-modal').style.display = "none";
});

// Đóng modal khi nhấn ra ngoài nội dung
window.onclick = function (event) {
    const modal = document.getElementById('all-locations-modal');
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

window.onload = fetchNearbyLocations;

function showOnMap(lat, lon, name) {
    const confirmMessage = `Bạn muốn chuyển tới Google Maps với địa điểm: ${name}?`;
    if (confirm(confirmMessage)) {
        const mapUrl = `https://www.google.com/maps/search/?api=1&query=${lat},${lon}`;
        window.open(mapUrl, '_blank');
    }
}


window.onscroll = function () {
    const infoBar = document.getElementById("infoBar");
    if (window.pageYOffset > 600) {
        // Khi cuộn xuống quá 400px, thêm class 'fixed' để cố định info-bar
        infoBar.classList.add("fixed-booking");
    } else {
        // Khi cuộn ngược lại, bỏ class 'fixed'
        infoBar.classList.remove("fixed-booking");
    }
};

//show more description 
let isExpanded = false; // Biến boolean để theo dõi trạng thái

function showMoreDescription() {
    const descriptionElement = document.getElementById('description');
    const showMoreText = document.getElementById('showMoreText');
    const showmore = document.getElementById('showmore');
    // Đảo ngược trạng thái mỗi lần nhấn
    isExpanded = !isExpanded;

    // Nếu là trạng thái mở rộng, hiển thị toàn bộ văn bản
    if (isExpanded) {
        descriptionElement.classList.remove('limit-three-words');
        showMoreText.style.display = "inline";
        showmore.innerText = "Ẩn bớt"
    } else {
        descriptionElement.classList.add('limit-three-words');
        showMoreText.style.display = "none";
        showmore.innerText = "Xem thêm"
    }
}
