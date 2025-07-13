// Toggle sidebar on mobile
document.getElementById('sidebarToggle').addEventListener('click', function () {
    document.getElementById('sidebar').classList.toggle('active');
});

// Initialize tooltips
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
});

// Add active class to sidebar items
const sidebarItems = document.querySelectorAll('.sidebar-item');
sidebarItems.forEach(item => {
    item.addEventListener('click', function () {
        sidebarItems.forEach(i => i.classList.remove('active'));
        this.classList.add('active');
    });
});

// Sample data for new reports
const reportTemplates = [
    {
        title: "HVAC System Maintenance",
        date: "10/01/2023, 11:20",
        author: "KHALID",
        authorColor: "#ff9e00",
        content: "This report documents the quarterly maintenance performed on the HVAC system. All filters were replaced, coils were cleaned, and the system was tested for optimal performance. Air quality readings showed improvement after maintenance."
    },
    {
        title: "Electrical System Inspection",
        date: "08/01/2023, 15:45",
        author: "LAYLA",
        authorColor: "#9d4edd",
        content: "This report covers the annual electrical system inspection. All circuit breakers, panels, and connections were examined and tested. Two faulty outlets were replaced in the east wing. Overall system is in good condition with no major concerns."
    }
];

// Function to add new report
document.querySelector('.add-report-btn').addEventListener('click', function () {
    // Get a random report template
    const template = reportTemplates[Math.floor(Math.random() * reportTemplates.length)];

    // Create new report card
    const reportCard = document.createElement('div');
    reportCard.className = 'report-card';
    reportCard.innerHTML = `
        <div class="report-header">
            <div class="report-title">${template.title}</div>
            <div class="report-date">${template.date}</div>
            <div class="report-author">
                <div class="author-dot" style="background-color: ${template.authorColor};"></div>
                <span>${template.author}</span>
            </div>
        </div>
        <div class="report-content">
            ${template.content}
        </div>
    `;

    // Add animation
    reportCard.style.opacity = '0';
    reportCard.style.transform = 'translateY(20px)';
    reportCard.style.transition = 'all 0.3s ease';

    // Insert at the beginning
    const reportsContainer = document.querySelector('.reports-container');
    reportsContainer.insertBefore(reportCard, reportsContainer.firstChild);

    // Trigger animation
    setTimeout(() => {
        reportCard.style.opacity = '1';
        reportCard.style.transform = 'translateY(0)';
    }, 10);
});

// Add interactivity when DOM is fully loaded
document.addEventListener('DOMContentLoaded', function () {
    // Highlight active menu item
    const menuItems = document.querySelectorAll('.menu-item');
    menuItems.forEach(item => {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            menuItems.forEach(i => i.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Add employee button functionality
    const addBtn = document.querySelector('.add-btn');
    addBtn.addEventListener('click', function () {
        alert('Add employee functionality would go here');
        // Here you could open a modal or redirect to a form page
    });

    // Comment button functionality
    const commentBtns = document.querySelectorAll('.actions button');
    commentBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            const employeeName = this.closest('.employee-card').querySelector('.employee-name').textContent;
            alert(`Open chat with ${employeeName}`);
            // Here you could open a chat window with the selected employee
        });
    });

    // Search functionality
    const searchInput = document.querySelector('.search-bar input');
    searchInput.addEventListener('keyup', function () {
        const searchText = this.value.toLowerCase();
        const employeeCards = document.querySelectorAll('.employee-card');

        employeeCards.forEach(card => {
            const employeeName = card.querySelector('.employee-name').textContent.toLowerCase();
            const employeePosition = card.querySelector('.employee-position').textContent.toLowerCase();

            if (employeeName.includes(searchText) || employeePosition.includes(searchText)) {
                card.style.display = 'flex';
            } else {
                card.style.display = 'none';
            }
        });
    });

    // Language selector functionality
    const languageSelector = document.querySelector('.language-selector');
    languageSelector.addEventListener('click', function () {
        alert('Language selection options would appear here');
        // Here you could show a dropdown with language options
    });

    // Notification functionality
    const notificationBtn = document.querySelector('.notification .btn');
    notificationBtn.addEventListener('click', function () {
        alert('Notifications panel would open here');
        // Here you could show a dropdown with notifications
    });

    // Mobile responsive menu toggle
    const menuToggle = document.querySelector('.logo-container .btn');
    const sidebar = document.querySelector('.sidebar');

    menuToggle.addEventListener('click', function () {
        sidebar.classList.toggle('expanded');

        if (window.innerWidth <= 992) {
            const sidebarWidth = getComputedStyle(document.documentElement).getPropertyValue('--sidebar-width');
            if (sidebar.style.width === sidebarWidth) {
                sidebar.style.width = '70px';
            } else {
                sidebar.style.width = 'var(--sidebar-width)';
            }
        }
    });
});
// Wait for the DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function () {
    // Initialize variables to store user data
    let userData = {
        fullName: 'Ahmed',
        profilePhoto: null, // Will store the image data URL
        email: '',
        phone: '',
        language: 'english',
        twoFactorAuth: 'disabled',
        notifications: 'all',
        dataSharing: 'enabled'
    };

    // Reference to form elements
    const profileForm = document.getElementById('profileForm');
    const fileInput = document.getElementById('fileInput');
    const profilePhotoPreview = document.getElementById('profilePhotoPreview');
    const fullNameInput = document.getElementById('fullName');
    const photoUserName = document.getElementById('photoUserName');
    const navbarUserName = document.getElementById('navbarUserName');
    const navbarAvatar = document.getElementById('navbarAvatar');

    // Add event listener for file input changes (profile photo upload)
    fileInput.addEventListener('change', function (event) {
        const file = event.target.files[0];
        if (file) {
            // Check if the file is an image
            if (!file.type.startsWith('image/')) {
                alert('Please select an image file');
                return;
            }

            // Check file size (limit to 5MB)
            if (file.size > 5 * 1024 * 1024) {
                alert('Image size should be less than 5MB');
                return;
            }

            // Create a FileReader to read the image
            const reader = new FileReader();
            reader.onload = function (e) {
                // Update the profile photo preview with the new image
                profilePhotoPreview.innerHTML = `<img src="${e.target.result}" alt="Profile Photo" class="w-100 h-100 rounded-circle">`;

                // Store the image data URL
                userData.profilePhoto = e.target.result;

                // Update the navbar avatar if there's a photo
                navbarAvatar.innerHTML = `<img src="${e.target.result}" alt="Profile" class="w-100 h-100 rounded-circle">`;
            };
            reader.readAsDataURL(file);
        }
    });

    // Add event listener for name input changes
    fullNameInput.addEventListener('input', function () {
        // Update the displayed name and stored value
        const newName = this.value.trim();
        if (newName) {
            photoUserName.textContent = newName;
            navbarUserName.textContent = newName;
            userData.fullName = newName;

            // If there's no profile photo, update the avatar with the first letter
            if (!userData.profilePhoto) {
                const firstLetter = newName.charAt(0).toUpperCase();
                navbarAvatar.textContent = firstLetter;
            }
        }
    });

    // Add event listener for form submission
    profileForm.addEventListener('submit', function (event) {
        event.preventDefault();

        // Update user data from form inputs
        userData.email = document.getElementById('email').value;
        userData.phone = document.getElementById('phoneNumber').value;
        userData.language = document.getElementById('language').value;
        userData.twoFactorAuth = document.getElementById('twoFactorAuth').value;
        userData.notifications = document.getElementById('notifications').value;
        userData.dataSharing = document.getElementById('dataSharing').value;

        // Save the user data (in a real application, this would be sent to a server)
        console.log('User data saved:', userData);

        // Show success message
        alert('Profile updated successfully!');

        // In a real application, you would send this data to your server
        // For example:
        // saveUserDataToServer(userData);
    });

    // Add event listeners for sidebar navigation
    document.querySelectorAll('.sidebar-item').forEach(item => {
        item.addEventListener('click', function () {
            // Remove active class from all items
            document.querySelectorAll('.sidebar-item').forEach(el => {
                el.classList.remove('active');
            });

            // Add active class to the clicked item
            this.classList.add('active');

            // In a real application, you would navigate to different pages
            const menuItem = this.querySelector('div').textContent.trim();
            console.log(`Menu item clicked: ${menuItem}`);
        });
    });

    // Function to save user data to server (mock implementation)
    function saveUserDataToServer(data) {
        // Simulate an API call with a timeout
        setTimeout(() => {
            console.log('Data sent to server:', data);
            // In a real application, you would use fetch or axios here
            // fetch('/api/update-profile', {
            //     method: 'POST',
            //     headers: {
            //         'Content-Type': 'application/json',
            //     },
            //     body: JSON.stringify(data),
            // })
        }, 1000);
    }
});
document.addEventListener('DOMContentLoaded', function () {
    const itemForm = document.getElementById('itemForm');
    const dropArea = document.getElementById('dropArea');
    const fileInput = document.getElementById('fileInput');
    const previewImage = document.getElementById('preview-image');
    let fileToUpload = null;

    // Open file dialog when clicking on the drop area
    dropArea.addEventListener('click', function () {
        fileInput.click();
    });

    // Handle drag and drop events
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, preventDefaults, false);
    });

    function preventDefaults(e) {
        e.preventDefault();
        e.stopPropagation();
    }

    ['dragenter', 'dragover'].forEach(eventName => {
        dropArea.addEventListener(eventName, highlight, false);
    });

    ['dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, unhighlight, false);
    });

    function highlight() {
        dropArea.style.borderColor = '#0d6efd';
        dropArea.style.backgroundColor = '#f0f7ff';
    }

    function unhighlight() {
        dropArea.style.borderColor = '#ddd';
        dropArea.style.backgroundColor = '#f9f9f9';
    }

    // Handle dropped files
    dropArea.addEventListener('drop', handleDrop, false);

    function handleDrop(e) {
        const dt = e.dataTransfer;
        const files = dt.files;
        handleFiles(files);
    }

    // Handle file selection from dialog
    fileInput.addEventListener('change', function () {
        handleFiles(this.files);
    });

    function handleFiles(files) {
        if (files.length > 0) {
            const file = files[0];

            // Validate file type and size
            const validTypes = ['image/jpeg', 'image/png', 'image/gif'];
            const maxSize = 5 * 1024 * 1024; // 5MB

            if (!validTypes.includes(file.type)) {
                alert('Please select a valid image file (JPG, PNG, or GIF)');
                return;
            }

            if (file.size > maxSize) {
                alert('File size exceeds 5MB limit');
                return;
            }

            fileToUpload = file;
            previewFile(file);
        }
    }

    function previewFile(file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            previewImage.src = e.target.result;
            previewImage.style.display = 'block';
            dropArea.querySelector('.upload-icon').style.display = 'none';
            dropArea.querySelector('.upload-text').textContent = 'Click to change image';
        }
        reader.readAsDataURL(file);
    }

    // Form submission
    itemForm.addEventListener('submit', function (event) {
        event.preventDefault();

        // Get form values
        const name = document.getElementById('name').value;
        const category = document.getElementById('category').value;
        const price = document.getElementById('price').value;
        const inStockCount = document.getElementById('inStockCount').value;

        // Create FormData object for file upload
        const formData = new FormData();
        formData.append('name', name);
        formData.append('category', category);
        formData.append('price', price);
        formData.append('inStockCount', inStockCount);

        if (fileToUpload) {
            formData.append('image', fileToUpload);
        }

        // Here you would typically send the formData to a server using fetch or XMLHttpRequest
        console.log('Item data prepared for upload:', {
            name,
            category,
            price,
            inStockCount,
            imageFile: fileToUpload ? fileToUpload.name : 'No image'
        });

        // For demo purposes, show success message and reset the form
        alert('Item added successfully!');
        itemForm.reset();
        previewImage.style.display = 'none';
        dropArea.querySelector('.upload-icon').style.display = 'block';
        dropArea.querySelector('.upload-text').innerHTML = 'Drag and drop or click to upload<br>(JPG, PNG, GIF, Max 5 MB)';
        fileToUpload = null;
    });
});
// Sidebar toggle functionality
document.getElementById('sidebarToggle').addEventListener('click', function () {
    document.querySelector('.sidebar').classList.toggle('collapsed');
    document.querySelector('.content').classList.toggle('expanded');
});

// Asset data - simulating data from a database
const assetData = [
    {
        name: "Apple Watch Series 4",
        location: "Saint-Joseph French High School",
        status: "Pending",
        date: "08/01/2025",
        assignedTo: "AHMED"
    },
    {
        name: "Microsoft Headquare",
        location: "Kargul Business Center",
        status: "In progress",
        date: "26/12/2024",
        assignedTo: "Sarah"
    },
    {
        name: "Camera",
        location: "Yabushan Business Center",
        status: "In progress",
        date: "26/12/2024",
        assignedTo: "Sarah"
    },
    {
        name: "Samsung A50",
        location: "Sirketl Train Station",
        status: "Complete",
        date: "14/12/2024",
        assignedTo: "MALAK"
    },
    {
        name: "Microsoft Headquare",
        location: "Sirketl Train Station",
        status: "Complete",
        date: "14/12/2024",
        assignedTo: "Jeanette Barker"
    }
];

// Function to create asset cards from data
function renderAssets() {
    const assetContainer = document.querySelector('.content');

    // Clear existing cards (except the title)
    const title = document.querySelector('.section-title');
    const assetCards = document.querySelectorAll('.asset-card');
    assetCards.forEach(card => card.remove());

    // Render assets from data
    assetData.forEach(asset => {
        const card = document.createElement('div');
        card.className = 'asset-card';

        let statusClass = '';
        if (asset.status === 'Pending') statusClass = 'status-pending';
        else if (asset.status === 'In progress') statusClass = 'status-progress';
        else if (asset.status === 'Complete') statusClass = 'status-complete';

        card.innerHTML = `
                    <div class="d-flex justify-content-between align-items-start">
                        <div class="asset-info">
                            <div class="asset-name">${asset.name}</div>
                            <div class="asset-location">${asset.location}</div>
                        </div>
                        <span class="status-badge ${statusClass}">${asset.status}</span>
                    </div>
                    <div class="asset-meta">
                        <span class="asset-meta-date">
                            <i class="far fa-calendar"></i> ${asset.date}
                        </span>
                        <span class="asset-meta-user">
                            <i class="far fa-user"></i> ${asset.assignedTo}
                        </span>
                    </div>
                `;

        // Insert after title
        title.insertAdjacentElement('afterend', card);
    });
}

// If you want to test the data rendering functionality
// Uncomment this line to render assets from the JavaScript data
// renderAssets();

// Search functionality
document.querySelector('.search-container input').addEventListener('input', function (e) {
    const searchTerm = e.target.value.toLowerCase();
    const assetCards = document.querySelectorAll('.asset-card');

    assetCards.forEach(card => {
        const assetName = card.querySelector('.asset-name').textContent.toLowerCase();
        const assetLocation = card.querySelector('.asset-location').textContent.toLowerCase();
        const assignedTo = card.querySelector('.asset-meta-user').textContent.toLowerCase();

        if (assetName.includes(searchTerm) ||
            assetLocation.includes(searchTerm) ||
            assignedTo.includes(searchTerm)) {
            card.style.display = 'block';
        } else {
            card.style.display = 'none';
        }
    });
});

<script>
    document.querySelectorAll('.sidebar-item[data-link]').forEach(item => {
        item.addEventListener('click', function () {
            const url = this.getAttribute('data-link');
            if (url) {
                window.location.href = url;
            }
        });
    });
</script>
document.addEventListener('DOMContentLoaded', function () {
    const dropArea = document.getElementById('dropArea');
    const fileInput = document.getElementById('fileInput');
    const previewImage = document.getElementById('preview-image');
    const imageError = document.getElementById('image-error');
    const form = document.getElementById('itemForm');

    // Open file dialog when clicking on the drop area
    dropArea.addEventListener('click', () => {
        fileInput.click();
    });

    // Prevent default behavior for drag events
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, preventDefaults, false);
    });

    function preventDefaults(e) {
        e.preventDefault();
        e.stopPropagation();
    }

    // Highlight drop area when dragging over it
    ['dragenter', 'dragover'].forEach(eventName => {
        dropArea.addEventListener(eventName, highlight, false);
    });

    ['dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, unhighlight, false);
    });

    function highlight() {
        dropArea.classList.add('highlight');
    }

    function unhighlight() {
        dropArea.classList.remove('highlight');
    }

    // Handle dropped files
    dropArea.addEventListener('drop', handleDrop, false);

    function handleDrop(e) {
        const dt = e.dataTransfer;
        const files = dt.files;

        if (files.length > 0) {
            handleFiles(files);
        }
    }

    // Handle file input change
    fileInput.addEventListener('change', function () {
        handleFiles(this.files);
    });

    function handleFiles(files) {
        const file = files[0];

        // Check file type
        const validTypes = ['image/jpeg', 'image/png', 'image/gif'];
        if (!validTypes.includes(file.type)) {
            showError('Please upload a valid image file (JPG, PNG, or GIF)');
            return;
        }

        // Check file size (max 5MB)
        if (file.size > 5 * 1024 * 1024) {
            showError('File size exceeds 5MB limit');
            return;
        }

        // Preview the image
        const reader = new FileReader();
        reader.onload = function (e) {
            previewImage.src = e.target.result;
            previewImage.style.display = 'block';
            imageError.style.display = 'none';
            // Hide the icon and text when image is displayed
            document.querySelector('.upload-icon').style.display = 'none';
            document.querySelector('.upload-text').style.display = 'none';
        }
        reader.readAsDataURL(file);
    }

    function showError(message) {
        imageError.textContent = message;
        imageError.style.display = 'block';
        previewImage.style.display = 'none';
        // Show the icon and text when there's an error
        document.querySelector('.upload-icon').style.display = 'block';
        document.querySelector('.upload-text').style.display = 'block';
    }

    // Form submission (you can customize this based on your backend needs)
    form.addEventListener('submit', function (e) {
        e.preventDefault();

        // Check if image is uploaded
        if (!fileInput.files.length) {
            showError('Please upload a product image');
            return;
        }

        // Here you would typically submit the form data
        // For example, using FormData and fetch:
        const formData = new FormData(form);

        // Example fetch request (uncomment and modify as needed)
        /*
        fetch('your-backend-endpoint', {
            method: 'POST',
            body: formData
        })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
            // Handle success (e.g., show success message, reset form)
            form.reset();
            previewImage.style.display = 'none';
            document.querySelector('.upload-icon').style.display = 'block';
            document.querySelector('.upload-text').style.display = 'block';
        })
        .catch(error => {
            console.error('Error:', error);
            // Handle error
        });
        */

        // For demo purposes, just log the form data
        console.log('Form submitted!');
        for (let [key, value] of formData.entries()) {
            console.log(key, value);
        }

        // Comment out the alert for production use
        alert('Product added successfully!');

        // Reset form
        form.reset();
        previewImage.style.display = 'none';
        document.querySelector('.upload-icon').style.display = 'block';
        document.querySelector('.upload-text').style.display = 'block';
    });
});


  // مركز محافظة القاهرة
  const cairoCenter = [30.0444, 31.2357]; 

  // إنشاء الخريطة
  const map = L.map('map').setView(cairoCenter, 12); // zoom 12 يناسب المدينة

  // إضافة طبقة خريطة OpenStreetMap
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap contributors',
    maxZoom: 18,
    minZoom: 10
  }).addTo(map);

  // بيانات الأصول (الأسماء + إحداثياتهم داخل المحافظة)
  const assets = [
    { name: "Vehicle Fleet", coords: [30.0500, 31.2330] },
    { name: "IT Equipment", coords: [30.0400, 31.2400] },
    { name: "Office Supplies", coords: [30.0460, 31.2300] },
    { name: "HR Department", coords: [30.0420, 31.2370] },
    { name: "Maintenance Tools", coords: [30.0485, 31.2450] },
  ];

  // إضافة كل أصل كنقطة على الخريطة مع نافذة منبثقة باسم الأصل
  assets.forEach(asset => {
    L.marker(asset.coords).addTo(map)
      .bindPopup(<b>${asset.name}</b>)
      .openPopup();
  });

