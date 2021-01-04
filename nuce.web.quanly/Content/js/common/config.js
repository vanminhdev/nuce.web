var COMMON_CONFIG = {
    currentDomain: window.location.hostname,
    domain: {
        "api3.khmt.nuce.edu.vn": {
            mobile: "0912987567",
            email: "ks.ktdb@nuce.edu.vn",
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
        var host = COMMON_CONFIG.currentDomain;
        if (host in COMMON_CONFIG.domain) {
            return COMMON_CONFIG.domain[host];
        }
        return COMMON_CONFIG.domain.default;
    },
};

var SPECIAL_CATEGORY = {
    /* Liên hệ */
    7: {
        jsTreeConfig: {
            a_attr: {
                href: '/frontendmanager/edit?type=CONTACT&code=&richText=false',
                class: 'alink',
            }
        },
    },
};