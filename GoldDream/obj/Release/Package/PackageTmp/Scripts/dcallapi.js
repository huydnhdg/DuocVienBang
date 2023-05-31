var contact = {
    init: function () {
        contact.registerEvent();
    },
    registerEvent: function () {
        $('#dbtnSend').off('click').on('click', function () {
            var username = $('#dphone').val();
            var pass = $('#dpassword').val();
            var maqua = $('#dmaqua').val();
            var commandcode = $('#dcommandcode').val();
            var category = $('#dcategory').val();
            $('#dloader').css('display', 'inline-block');

            $.ajax({
                url: '/Default/CallApi',
                type: 'POST',
                dataType: 'json',
                data: {
                    username: username,
                    pass: pass,                    
                    commandcode: commandcode,
                    category: category,
                    maqua: maqua
                },
                success: function (response) {
                    //console.log(response);
                    if (response.success) {
                        $('#dtxtmess').css('display', 'inline-block');
                        $('#dtxtmess').html(response.message);
                        $('#dloader').css('display', 'none');
                    }
                    contact.resetForm();
                },
            });
        });
        $('#dbtnSend60').off('click').on('click', function () {
            var username = $('#dphone60').val();
            var pass = $('#dpassword60').val();
            var maqua = $('#dmaqua60').val();
            var commandcode = $('#dcommandcode60').val();
            var category = $('#dcategory60').val();
            $('#dloader60').css('display', 'inline-block');

            $.ajax({
                url: '/Default/CallApi',
                type: 'POST',
                dataType: 'json',
                data: {
                    username: username,
                    pass: pass,
                    commandcode: commandcode,
                    category: category,
                    maqua: maqua
                },
                success: function (response) {
                    //console.log(response);
                    if (response.success) {
                        $('#dtxtmess60').css('display', 'inline-block');
                        $('#dtxtmess60').html(response.message);
                        $('#dloader60').css('display', 'none');
                    }
                    contact.resetForm();
                },
            });
        });
    },
    resetForm: function () {
        $('#dform-contact')[0].reset();
        $('#dform-contact60')[0].reset();
    }
}
contact.init();