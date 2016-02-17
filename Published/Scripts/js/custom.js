$(".or-label").click(function (e) {
    //e.preventDefault();
    var label ="#"+ $(this).attr("id");
    var dInput = $(label +" .is_selected").attr("id");
    if ($("#" + dInput).attr("checked")) {
        $(label).toggleClass("or-label-true");
    } else {
        $(label).removeClass("or-label-true");
    }
   
    
});