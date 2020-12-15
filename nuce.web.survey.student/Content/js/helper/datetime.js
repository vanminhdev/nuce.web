var datetimeHelper = {
    timeToString: function(value) {
        const actualMonth = `${value}`;
        const prefix = actualMonth.length === 1 ? '0' : '';
        return `${prefix}${actualMonth}`;
    },

    convertDateTime: function(date = '') {
        var entryDateTime = new Date(date);
        return `${this.timeToString(entryDateTime.getDate())}/${this.timeToString(entryDateTime.getMonth() + 1)}/${entryDateTime.getFullYear()}`;
    },
};