var commonHelper = {
    toggleElementOnScroll: function(elementId = 'scrollTop', pivot = 100, fadeTime = 400) {
        $(window).scroll(function(){
            if ($(this).scrollTop() > pivot) {
		        $(`#${elementId}`).fadeIn(fadeTime);
	        } else {
		        $(`#${elementId}`).fadeOut(fadeTime);
	        }
        });
    },
    smoothScrollByAnchor: function(anchorId = 'scrollTop', duration = 800) {
        $(`#${anchorId}`).click(function() {
            const targetId = $(this).attr('href');
            const scrollTop = targetId !== null ? $(targetId).offset().top : 0;
            $('html, body').animate({
                scrollTop: scrollTop,
            }, duration);
        });
    },
    isFunction: function(functionToCheck) {
        return functionToCheck && {}.toString.call(functionToCheck) === '[object Function]';
    },
};

var _autoLayoutImage = {
    /* {id: {} || ...} */
    config: {
        'banner-header': 'KTDB_BANNER_HEADER',
        'banner-certificate': 'KTDB_BANNER_MAIN_1',
        'survey-undergraduate-img': 'KTDB_SURVEY_UNDERGRADUATE',
        'survey-student-img': 'KTDB_SURVEY_STUDENT',
        'survey-graduated-img': 'KTDB_SURVEY_GRADUATED',
        'link-ktdb-ctsv-img': 'KTDB_FAST_LINK_CTSV',
        'link-ktdb-daotao-img': 'KTDB_FAST_LINK_EDU',
    },
    load: function() {
        Object.keys(_autoLayoutImage.config).forEach(elementId => {
            if ($(`#${elementId}`) !== null) {
                $(`#${elementId}`).prop({
                    src: `${API_URL}/api/UserFile/image/${_autoLayoutImage.config[elementId]}`,
                });
            }
        });
    },
};