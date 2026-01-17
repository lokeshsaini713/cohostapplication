$(document).ready(function () {

    // FORM SUBMIT
    $(document).on("submit", "#consultationForm", function (e) {
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

    // LOAD ARTICLES
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
                                <img src="${a.imagePath}" alt="${a.title}">
                            </a>
                        </div>
                        <div class="Articles_ArticalContant__hqctz">
                            <h3><a href="/blog/${a.slug}">${a.title}</a></h3>
                            <p>${a.shortDescription}</p>
                        </div>
                    </div>
                </div>`;
            });
            $('#articlesContainer').html(html);
        },
        error: function (xhr) {
            console.error('Articles load failed', xhr.responseText);
        }
    });

});
