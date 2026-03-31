var Func = function () {

   
    function Write_Celebrator() {
        jQuery('#li_Birthday').click(function () {
            $.ajax({
                type: "POST",
                traditional: true,
                url: "Home/Write_Celebrator_Json",
                success: function () {
                    alert('write')
                }
        })
    });
    }

    

    return {
        init: function () {
            Write_Celebrator();
            
        }
    }


}();