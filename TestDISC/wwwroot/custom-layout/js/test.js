var infoGroup = 'info-group';
var testGroup = 'test-group';
var keyStore = 'form-register';

$(document).ready(function (e) {
    var storeForm = getCookie(keyStore);
    if (storeForm) {
        var form = JSON.parse(storeForm);

        var radios = $('input[name="UserCreate.titleid"]');
        if (radios.is(':checked') === false) {
            radios.filter('[value="' + form.titleid + '"]').attr('checked', true);
        }

        $('#UserCreate_fullname').val(form.fullname);
        $('#UserCreate_email').val(form.email);
        $('#UserCreate_phone').val(form.phone);
        $('#UserCreate_namecompany').val(form.namecompany);
    }
    OpenInfoGroup();
    DetectBanner();

    $(window).resize(function () {
        DetectBanner();
    });
});

function DetectBanner() {
    var width = $(document).width();
    if (width < 500) {
        $('.my-banner').attr("src", "/images/banner.png");
    } else {
        $('.my-banner').attr("src", "/images/banner-big.png");
    }
}

$("#btn-start").click(function (e) {
    e.preventDefault();

    if (!$('input[name="UserCreate.titleid"]:checked').val() ||
        !$('#UserCreate_fullname').val() || !$('#UserCreate_email').val() ||
        !$('#UserCreate_phone').val() || !$('#UserCreate_namecompany').val()) {
        alert('Vui lòng điền đầy đủ thông tin để bắt đầu.');
        return false;
    }

    if (!ValidateEmail($('#UserCreate_email').val())) {
        alert('Email không hợp lệ. Vui lòng thử lại.');
        return false;
    }

    if (!ValidatePhone($('#UserCreate_phone').val())) {
        alert('Số điện thoại không hợp lệ. Vui lòng thử lại.');
        return false;
    }

    var cookie = JSON.stringify({
        titleid: $('input[name="UserCreate.titleid"]:checked').val(),
        fullname: $('#UserCreate_fullname').val(),
        email: $('#UserCreate_email').val(),
        phone: $('#UserCreate_phone').val(),
        namecompany: $('#UserCreate_namecompany').val(),
    });

    setCookie(keyStore, cookie, 7);

    OpenTestGroup();

    setTimeout(() => {
        $(window).scrollTop(0);
    }, 100);

    return true;
});

$('#btn-return').click(function (e) {
    e.preventDefault();
    ChangeQuestion(-1);
});

$('#btn-next').click(function (e) {
    e.preventDefault();
    ChangeQuestion(1);
});

$('#btn-submit').click(function (e) {
    e.preventDefault();
    SaveForm();
});

function CheckValid(ind) {
    if (ind < 0) {
        return '';
    }

    var indexChoose = $('#QuestionGroup_ActiveQuestion').val();
    var mostValue = $('input[name="QuestionGroup.Questions[' + indexChoose +'].mostchoosenid"]:checked').val();
    var leastValue = $('input[name="QuestionGroup.Questions[' + indexChoose + '].leastchoosenid"]:checked').val();
    
    if (!mostValue || !leastValue) {
        return 'Vui lòng chọn đầy đủ câu trả lời.';
    }

    if (mostValue == leastValue) {
        return 'MOST và LEAST phải chọn khác nhau.';
    }

    return '';
}

function ChangeQuestion(ind) {
    var valid = CheckValid(ind);
    if (valid.trim().length > 0) {
        alert(valid.trim());
        return false;
    }

    var pe = parseInt($('#QuestionGroup_ActiveQuestion').val()) + parseInt(ind);
    if (pe < 0) {
        pe = 0;
    } else if (pe >= $('#lengthMax').val()) {
        pe = $('#lengthMax').val() - 1;
    }

    if (pe == ($('#lengthMax').val() - 1)) {
        $('#btn-next').css('display', 'none');
        $('#btn-submit').css('display', '');
    } else {
        $('#btn-next').css('display', '');
        $('#btn-submit').css('display', 'none');
    }

    $('#QuestionGroup_ActiveQuestion').val(pe);

    if (pe == 0) {
        $('#btn-return').css('display', 'none');
    } else {
        $('#btn-return').css('display', '');
    }

    $('div[name="question_block"]').css('display', 'none');
    $('div[name="question_block"]').eq(pe).css('display', '');

    return true;
}

function SaveForm() {
    var valid = CheckValid(1);
    if (valid.trim().length > 0) {
        alert(valid.trim());
        return false;
    }

    ShowLoading();
    $('#main-form').submit();
}

function OpenInfoGroup() {
    $('div[name="' + infoGroup + '"]').css('display', 'block');
    $('div[name="' + testGroup + '"]').css('display', 'none');
}

function OpenTestGroup() {
    $('div[name="' + infoGroup + '"]').css('display', 'none');
    $('div[name="' + testGroup + '"]').css('display', 'block');
}