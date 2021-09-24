var _clientParameterHelper = {
    getDataApi: function(baseAddress, parameter = '', callback) {
        baseAddress = baseAddress || API_URL;
        $.ajax({
            type: 'get',
            url: `${baseAddress}/api/ClientParameters/${parameter}`,
            dataType: 'json',
            contentType: 'application/json',
            success: function(res) {
                callback(res)
            },
        });
    },
    getValueByCode: function(data = [], code = '') {
        const foundParameter = data.find(p => p.code === code);
        return typeof foundParameter !== 'undefined' ? foundParameter.value : '';
    },
};