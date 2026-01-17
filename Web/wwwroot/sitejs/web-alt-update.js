(function () {
    document.addEventListener("DOMContentLoaded", function () {

        const seoAltMap = {
            /* Banner icons */
            "bannersoftware-main1.svg": "Agile Software Development Services",
            "bannersoftware-main2.svg": "CMMI Certified Software Development Company",
            "bannersoftware-main3.svg": "Scalable Custom Software Solutions",
            "bannersoftware-main4.svg": "Affordable Software Development Pricing",
            "bannersoftware-main5.svg": "Global Software Development Company",
            "bannersoftware-main6.svg": "Software Maintenance and Support Services",

            /* Software feature section */
            "software-feature1.svg": "SaaS Application Development Company",
            "software-feature2.svg": "Web Portal Development Services",
            "software-feature3.svg": "Custom B2B Software Development",
            "software-feature4.svg": "Cross Platform Mobile App Development",
            "software-feature5.svg": "Windows Application Development Services",
            "software-feature6.svg": "White Label Software Development",
            "software-feature.webp": "Custom Software Development Features",

            /* Software types / why choose */
            "clear-communication.webp": "Web Development Services Company",
            "solutions-built.webp": "Mobile App Development Company",
            "on-time-delivery.webp": "Custom Software Development Company",
            "direct-collaboration.webp": "Enterprise Software Development Services",

            /* Hire us section */
            "business3.svg": "Dedicated Software Development Team",
            "busines2.webp": "Why Choose CohostWeb Software Development",
            "busines2.svg": "Software Development Benefits",
            "business4.svg": "Contact Software Development Company",

            /* Services cards */
            "service-craft.svg": "Custom Software Development Services"
        };

        document.querySelectorAll("img").forEach(function (img) {
            const src = img.getAttribute("src");
            if (!src) return;

            const fileName = src.split("/").pop();

            // 🔹 Remove existing alt
            img.removeAttribute("alt");

            // 🔹 Apply SEO optimized alt if mapped
            if (seoAltMap[fileName]) {
                img.setAttribute("alt", seoAltMap[fileName]);
            }
        });

    });
})();
