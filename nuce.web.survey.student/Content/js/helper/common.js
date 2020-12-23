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
}