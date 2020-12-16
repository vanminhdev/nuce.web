var COMMON_CONFIG = {
    getCurrentDomain: window.location.hostname,
    domain: {
        "api3.khmt.nuce.edu.vn": {
            mobile: "",
            email: "",
        },
        "ad.sv.nuce.edu.vn": {
            mobile: "02438697004",
            email: "ctsv@nuce.edu.vn",
        },
        default: {
            mobile: "02438697004",
            email: "ctsv@nuce.edu.vn",
        },
    },
    getDomainConfig: function() {
        var host = COMMON_CONFIG.getCurrentDomain;
        if (host in COMMON_CONFIG.domain) {
            return COMMON_CONFIG.domain[host];
        }
        return COMMON_CONFIG.domain.default;
    },
};