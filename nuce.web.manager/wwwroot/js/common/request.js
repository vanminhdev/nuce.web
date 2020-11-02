var methodDictionary = {
    POST: 'POST',
    PUT: 'PUT',
    GET: 'GET',
    POST: 'POST',
};
var refreshToken = function(callback) {
    $.ajax({
        type: 'POST',
        url: `${API_URL}/api/user/refreshToken`,
        success: function(res) {
            console.log('success: ',  res);
            callback();
        },
        error: function(err) {
            console.log('refresh-error: ', err);
            href = `/admin/login`;
            window.location.href = href;
        }
    });
};

var dataTableRequest = {
    create: function(api, method = 'POST', tableId = '', columns = [], callback) {
        var config = {
            processing: true,
            serverSide: true,
            searching: false,
            ordering: false,
            lengthChange: false,
            stateSave: true,
            pageLength: 1,
            language: dataTableConfig.language,
            columns,
            ajax: {
                url: API_URL + api,
                dataType: 'json',
                contentType: 'application/json',
                type: method,
                columns,
                data: function(d) {
                    return JSON.stringify(d);
                },
                error: function (jqXHR, exception) {
                    var msg = '';
                    if (jqXHR.status === 401) {
                        console.log('401: callback');
                        refreshToken(callback);
                    } else {
                        console.log('drop');
                        window.location.href = `admin/login`;
                    }
                },
            },
        };
        var result = $(`#${tableId}`).DataTable(config);
        return result;
    },
};