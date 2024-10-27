$(document).ready(function () {
    $('#uploadBtn').on('click', function () {
        var formData = new FormData();
        var fileInput = $('#fileInput')[0].files[0];

        if (fileInput) {
            formData.append('file', fileInput);

            $.ajax({
                url: $("#meterUploadUrl").val(),
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.successCount !== undefined && response.failedCount !== undefined) {
                        $('#successLabel').text('Successful Number: ' + response.successCount + '\n Failed Number: ' + response.failedCount);
                    } else {
                        $('#successLabel').text('Upload failed');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#successLabel').text('Error occurred: ' + jqXHR.responseText);
                }
            });
        } else {
            $('#successLabel').text('Please select a file.');
        }
    });
});