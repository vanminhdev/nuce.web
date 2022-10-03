var COMMON_CONFIG = {
    currentDomain: window.location.hostname,
    domain: {
        "api3.khmt.nuce.edu.vn": {
            mobile: "0912987567",
            email: "ks.ktdb@nuce.edu.vn",
        },
        "ad.sv.nuce.edu.vn": {
            mobile: "02438697004",
            email: "ctsv@huce.edu.vn",
        },
        default: {
            mobile: "02438697004",
            email: "ctsv@huce.edu.vn",
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
    8: {
        jsTreeConfig: {
            a_attr: {
                href: '/frontendmanager/edit?type=CONTACT&code=&richText=false',
                class: 'alink',
            }
        },
    },
};

var IMAGE_MGMT = {
    KTDB: {
        KTDB_BANNER_HEADER: {
            code: 'KTDB_BANNER_HEADER',
            label: 'Logo phòng',
        },
        KTDB_BANNER_MAIN_1: {
            code: 'KTDB_BANNER_MAIN_1',
            label: 'Banner'
        },
        KTDB_SURVEY_STUDENT: {
            code: 'KTDB_SURVEY_STUDENT',
            label: 'Khảo sát sinh viên'
        },
        KTDB_SURVEY_GRADUATED: {
            code: 'KTDB_SURVEY_GRADUATED',
            label: 'Khảo sát cựu sinh viên',
        }, 
        KTDB_SURVEY_UNDERGRADUATE: {
            code: 'KTDB_SURVEY_UNDERGRADUATE',
            label: 'Khảo sát sinh viên trước tốt nghiệp',
        },
        KTDB_FAST_LINK_CTSV: {
            code: 'KTDB_FAST_LINK_CTSV',
            label: 'Liên kết nhanh CTSV',
        },
        KTDB_FAST_LINK_EDU: {
            code: 'KTDB_FAST_LINK_EDU',
            label: 'Liên kết nhanh trang đào tạo',
        },
    },
};