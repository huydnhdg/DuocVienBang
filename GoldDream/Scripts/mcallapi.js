var contact = {
    init: function () {
        contact.registerEvent();
    },
    registerEvent: function () {
        $('#mbtnSend').off('click').on('click', function () {
            var username = $('#mphone').val();
            var pass = $('#mpassword').val();
            var commandcode = $('#mcommandcode').val();
            var category = $('#mcategory').val();
            $('#mloader').css('display', 'inline-block');

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
                        $('#mtxtmess').css('display', 'inline-block');
                        $('#mtxtmess').html(response.message);
                        $('#mloader').css('display', 'none');
                    }
                    contact.resetForm();
                },
            });
        });
        $('#mbtnSend60').off('click').on('click', function () {
            var username = $('#mphone60').val();
            var pass = $('#mpassword60').val();
            var commandcode = $('#mcommandcode60').val();
            var category = $('#mcategory60').val();
            $('#mloader60').css('display', 'inline-block');

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
                        $('#mtxtmess60').css('display', 'inline-block');
                        $('#mtxtmess60').html(response.message);
                        $('#mloader60').css('display', 'none');
                    }
                    contact.resetForm();
                },
            });
        });
    },
    resetForm: function () {
        $('#mform-contact')[0].reset();
        $('#mform-contact60')[0].reset();
    }
}
contact.init();