$('.clear-input').each(function () {
    let self = $(this);
    self.click(function () {
        //tìm 2 cấp
        self.parent().parent().find('input').each(function () {
            $(this).val('');
        });
        self.parent().parent().find('select').each(function () {
            $(this).selectpicker('val', '');
        });
        //tìm 1 cấp
        self.parent().find('input').each(function () {
            $(this).val('');
        });
        self.parent().find('input').each(function () {
            $(this).selectpicker('val', '');
        });
    });
});