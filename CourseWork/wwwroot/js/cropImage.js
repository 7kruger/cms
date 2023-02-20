$(document).on('click', '#upload-aphoto', function () {
    $('#selectedFile').trigger('click');
});

$('#selectedFile').change(function () {
    if (this.files[0] == undefined)
        return;
    $('#imageModalContainer').modal('show');
    let reader = new FileReader();
    reader.addEventListener("load", function () {
        window.src = reader.result;
        $('#selectedFile').val('');
    }, false);
    if (this.files[0]) {
        reader.readAsDataURL(this.files[0]);
    }
});

let croppi;
$('#imageModalContainer').on('shown.bs.modal', function () {
    let width = $('#crop-image-container')[0].offsetWidth - 20;

    $('#crop-image-container').height((width - 80) + 'px');
    croppi = $('#crop-image-container').croppie({
        viewport: {
            width: 300,
            height: 300
        },
    });
    $('.modal-body1').height($('#crop-image-container')[0].offsetHeight + 50 + 'px');
    croppi.croppie('bind', {
        url: window.src,
    }).then(function () {
        croppi.croppie('setZoom', 0);
    });
});
$('#imageModalContainer').on('hidden.bs.modal', function () {
    croppi.croppie('destroy');
});

$(document).on('click', '.save-modal', function (ev) {
    croppi.croppie('result', {
        type: 'base64',
        format: 'jpeg',
        size: 'original'
    }).then(function (res) {
        $('.modal').modal('hide');
        $('input[name=image]').val(res);
    });
});

//avatar
$(document).on('click', '#upload-avatar-photo', function () {
    $('#selectedAvatarFile').trigger('click');
});

$('#selectedAvatarFile').change(function () {
    if (this.files[0] == undefined)
        return;
    $('#avatarModalContainer').modal('show');
    let reader = new FileReader();
    reader.addEventListener("load", function () {
        window.src = reader.result;
        $('#selectedAvatarFile').val('');
    }, false);
    if (this.files[0]) {
        reader.readAsDataURL(this.files[0]);
    }
});

$('#avatarModalContainer').on('shown.bs.modal', function () {
    let width = $('#crop-image-container')[0].offsetWidth - 20;

    $('#crop-image-container').height((width - 80) + 'px');
    croppi = $('#crop-image-container').croppie({
        viewport: {
            width: 300,
            height: 300,
            type: 'circle'
        },
    });
    $('.modal-body1').height($('#crop-image-container')[0].offsetHeight + 50 + 'px');
    croppi.croppie('bind', {
        url: window.src,
    }).then(function () {
        croppi.croppie('setZoom', 0);
    });
});
$('#avatarModalContainer').on('hidden.bs.modal', function () {
    croppi.croppie('destroy');
});