$('#darkthene-checkbox').change(function () {
    if (this.checked)
    {
        $('body').addClass('dark-theme');
        $(".navbar").removeClass("navbar-default");
        $(".navbar").addClass("navbar-inverse");
    }
    else
    {
        $('body').removeClass('dark-theme');
        $(".navbar").removeClass("navbar-inverse");
        $(".navbar").addClass("navbar-default");


    }
});