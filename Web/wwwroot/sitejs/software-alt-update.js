(function () {
    document.addEventListener("DOMContentLoaded", function () {

        const altMap = {
            /* Banner icons */
            "bannersoftware-main1.svg": "Agile Software Development Methodology",
            "bannersoftware-main2.svg": "CMMI Appraised Development Centers",
            "bannersoftware-main3.svg": "Scalable and Future Proof Software Solutions",
            "bannersoftware-main4.svg": "Transparent Software Pricing Models",
            "bannersoftware-main5.svg": "Global Software Delivery Capabilities",
            "bannersoftware-main6.svg": "Continuous Software Support and Maintenance",

            /* Software features */
            "software-feature1.svg": "SaaS Application Development",
            "software-feature2.svg": "Web Based Portal Development",
            "software-feature3.svg": "Custom B2B Software Development",
            "software-feature4.svg": "Cross Platform Mobile App Development",
            "software-feature5.svg": "Windows Application Development",
            "software-feature6.svg": "White Label Software Solutions",
            "software-feature.webp": "Software Development Feature Illustration",

            /* Why choose / types */
            "clear-communication.webp": "Web Development Services",
            "solutions-built.webp": "Mobile Application Development Services",
            "on-time-delivery.webp": "Custom Software Development Services",
            "direct-collaboration.webp": "Enterprise Software Development Services",

            /* Hire us section */
            "business3.svg": "Dedicated Software Development Team",
            "busines2.webp": "Why Choose CohostWeb Software Services",
            "busines2.svg": "CohostWeb Software Development Benefits",
            "business4.svg": "Explore Software Development Services",

            /* Services section */
            "service-craft.svg": "Custom Software Development Service Icon"
        };

        document.querySelectorAll("img").forEach(function (img) {
            const src = img.getAttribute("src");
            if (!src) return;

            const fileName = src.split("/").pop();

            if (altMap[fileName]) {
                img.alt = altMap[fileName];
            }
        });

    });
})();
