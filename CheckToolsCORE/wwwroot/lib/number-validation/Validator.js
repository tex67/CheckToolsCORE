
    $.validator.methods.number = function (value, element) {
       
        return this.optional(element) || /^-?(?:\d+|\d{1, 3}(?:\.\d{3})+)?(?:,\d+)?$/.test(value);
    
};
$.validator.methods.date = function (value, element) {
   
    return this.optional(element) || moment(value, "DD/MM/YYYY", true).isValid();
};