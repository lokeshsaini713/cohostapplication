//$(function () {
//    $.when(users.ResetStateSaveDataTable()).then(function () {
//        users.List();
//    });
//});
$("#consultationForm").submit(function (e) {
    e.preventDefault();

    if (!$(this).valid()) return;

    $.ajax({
        url: '/home/BookConsultation',
        type: 'POST',
        data: $(this).serialize(),
        success: function (data) {
            Swal.fire({
                icon: 'success',
                title: 'Thank You!',
                text: data.dis
            });

            $("#consultationForm")[0].reset();
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Something went wrong. Try again.'
            });
        }
    });
});
$(document).ready(function () {

    loadCaseStudies()
   
    $.ajax({
        url: '/latest',
        type: 'GET',
        success: function (data) {

            let html = '';

            $.each(data, function (i, a) {

                html += `
                <div class="col-md-4">
                    <div class="Articles_ArticalBox__iUGhA">

                        <div class="Articles_ArticalImg__ry2S2">
                            <a href="/blog/${a.slug}">
                                <img src="${a.imagePath}" alt="${a.title}" />
                            </a>
                        </div>

                        <div class="Articles_ArticalContant__hqctz">

                            <div class="Articles_TechArticalBlock__dr3fa d-flex align-items-center justify-content-between">
                                <div class="Articles_TechArticalDate__Q9EaZ d-flex align-items-center">
                                    <img src="./assets/calendar-icon.webp" alt="calendar-icon" />
                                    ${a.date}
                                </div>
                                <div class="Articles_TechArticalInfo__8dhr8">
                                    ${a.category}
                                </div>
                            </div>

                            <h3>
                                <a href="/blog/${a.slug}">
                                    ${a.title}
                                </a>
                            </h3>

                            <p>
                                ${a.shortDescription}
                            </p>

                            <a class="Articles_articleBtn__LJvRk btn-custom btn-small btn-arrow"
                               href="/blog/${a.slug}">
                                Keep Reading
                            </a>

                        </div>
                    </div>
                </div>`;
            });

            $('#articlesContainer').html(html);
        },
        error: function () {
            console.error('Failed to load articles');
        }
    });

});


  
function loadCaseStudies() {
    $.ajax({
        url: '/CaseStudy/GetCaseStudies',
        type: 'GET',
        success: function (data) {
            let html = '';

            $.each(data, function (index, item) {

                html += `
                    <div class="col-lg-4 col-md-6 col-sm-6 col-xs-6">
                        <a href="/case-study/${item.slug}">
                            <div class="HomeCaseStudy_CaseStudiesBox__cIG_H">
                                <div class="HomeCaseStudy_caseStudiesImg__53nAu">
                                    <img src="${item.imagePath}" alt="${item.category}" />

                                    <div class="HomeCaseStudy_caseStudiesContent__bD5FS">
                                        <h4>${item.title}</h4>
                                        <p>${item.shortDescription}</p>
                                    </div>

                                    <div class="HomeCaseStudy_caseDevFlag___fF08">
                                        

                                        <span>
                                          <img alt="Microsoft.Net 6" class="" src="./assets/netlogo.webp"/>
                                            <h6>${item.technology}</h6>
                                        </span>

                                        <span>
                                          <img alt="Angular" class="" src="./assets/angular-icon.webp"/>
                                            <h6>Angular 19</h6>
                                        </span>
                                        <span>
                                          <img alt="react" class="" src="./assets/react-icon.webp"/>
                                            <h6>React</h6>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </div>`;
            });

            $('#caseStudyRow').html(html);
        },
        error: function () {
            console.error("Case studies load failed");
        }
    });
}
