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
