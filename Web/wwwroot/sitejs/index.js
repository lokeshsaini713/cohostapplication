(function () {
    document.addEventListener("DOMContentLoaded", function () {
        const altOverrides = {
            // Home banner services
            "software-dev.svg": "Software Development Services",
            "mobile-app-dev.svg": "Mobile App Development Services",
            "web-app-dev.svg": "Web Development Services",
            "cart-icon-white.svg": "E-Commerce Development Services",
            "hubspot-crm-white.svg": "Search Engine Optimization Services",

            // Driving success icons
            "elevate1.svg": "Cost Saving Software Solutions",
            "elevate2.svg": "Productivity Enhancement Solutions",
            "elevate3.svg": "Enterprise Security Solutions",

            // Services section
            "custom-soft-dev.webp": "Custom Software Development",
            "custom-mobile-app.webp": "Custom Mobile App Development",
            "web-dev.webp": "Professional Web Development",
            "custom-ecommerce.webp": "Custom E-Commerce Solutions",
            "custom-security.webp": "SEO and Digital Marketing Services",
            "ai-ml.webp": "API Development and Integration",
            "ai-ml-brain.webp": "API Integration Architecture",

            // Case studies
            "case-study-4.webp": "Website and Trading Application Development",
            "case-study-5.webp": "Web Application Development Project",
            "case-study-6.webp": "Website Upgrade and Maintenance Project",

            // Country / tech icons
            "india.webp": "India",
            "netlogo.webp": ".NET Technology Stack",
            "drupal-img.webp": "Drupal and React Technology Stack"
        };

        document.querySelectorAll("img").forEach(function (img) {
            const src = img.getAttribute("src");
            if (!src) return;

            const fileName = src.split("/").pop();

            // 1️⃣ Use predefined alt if available
            if (altOverrides[fileName]) {
                img.alt = altOverrides[fileName];
                return;
            }

            // 2️⃣ Auto-generate clean alt from filename (fallback)
            const cleanAlt = fileName
                .replace(/\.(webp|png|jpg|jpeg|svg)$/i, "")
                .replace(/[-_]/g, " ")
                .replace(/\b\w/g, c => c.toUpperCase());

            img.alt = cleanAlt;
        });
    });
})();
