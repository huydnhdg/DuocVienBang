var contact = {
    init: function () {
        contact.registerEvent();
        contact.registerEvent60();
    },
    registerEvent: function () {
        $('#btnSend').off('click').on('click', function () {
            var username = $('#phone').val();
            var pass = $('#password').val();
            var commandcode = $('#commandcode').val();
            var category = $('#category').val();
            $('#loader').css('display', 'inline-block');

            $.ajax({
                url: '/Default/CallApi',
                type: 'POST',
                dataType: 'json',
                data: {
                    username: username,
                    pass: pass,
                    commandcode: commandcode,
                    category: category
                },
                success: function (response) {
                    if (response.success) {
                        $('#txtmess').css('display', 'inline-block');
                        $('#txtmess').html(response.message);
                        $('#loader').css('display', 'none');
                    }
                    contact.resetForm();
                }
            });
        });

    },
    registerEvent60: function () {
        $('#btnSend60').off('click').on('click', function () {
            var username = $('#phone60').val();
            var pass = $('#password60').val();
            var commandcode = $('#commandcode60').val();
            var category = $('#category60').val();
            $('#loader60').css('display', 'inline-block');

            $.ajax({
                url: '/Default/CallApi',
                type: 'POST',
                dataType: 'json',
                data: {
                    username: username,
                    pass: pass,
                    commandcode: commandcode,
                    category: category
                },
                success: function (response) {
                    if (response.success) {
                        $('#txtmess60').css('display', 'inline-block');
                        $('#txtmess60').html(response.message);
                        $('#loader60').css('display', 'none');
                    }
                    contact.resetForm();
                }
            });
        });
    },
    resetForm: function () {
        $('#form-contact')[0].reset();
    }
}
contact.init();