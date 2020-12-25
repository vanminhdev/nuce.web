var _fileHelper = {
    loadImageOnChangeInput: function(event, imageId = '') {                
        const file = event.files[0];

        const reader = new FileReader();
        reader.onload = function(event) {
            $(`#${imageId}`).attr('src', event.target.result);
        }; 
        reader.readAsDataURL(file);
    },
    loadImageOnLink: function(imgLink = '', imageId = '') {
        $(`#${imageId}`).attr('src', imgLink);
    },
};