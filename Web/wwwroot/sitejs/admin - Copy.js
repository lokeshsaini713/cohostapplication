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
        url: '/api/articles/latest',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            var html = '';

            $.each(data, function (index, a) {
                html += `
                            <div class="article-card">
                                <img src="${a.imagePath}" alt="${a.title}" />
                                <span class="badge">${a.category}</span>
                                <h4>${a.title}</h4>
                                <p>${a.shortDescription}</p>
                                <small>${a.date}</small>
                                <a href="/blog/${a.slug}" class="btn btn-info">
                                    Keep Reading →
                                </a>
                            </div>
                        `;
            });

            $('#latestArticles').html(html);
        },
        error: function (err) {
            console.error('Error loading articles', err);
        }
    });

});
