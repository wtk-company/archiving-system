$(document).ready(function () {

    $(".collapsible").on("click", function (e) {

        e.preventDefault();

        var this1 = $(this);
        var data = { pid: $(this).attr("pid") };

        var isloaded = $(this1).attr('data-loaded');
        if(isloaded=="false")
        {
            $(this1).addClass("loadingP");
            $(this1).removeClass("collapse");
            $.ajax({
                url: "/Departments/GetSubDepartment",
                type: "GET",
                dataType: "json",
                success: function (d) {

                    if(d.length>0)
                    {
                        var $ul = $("<ul></ul>");
                        $.each(d, function (i,ele) {

                            $ul.append(
                                $("<li></li>").append(
                                "<span class='collapse collapsible' data-loaded='false' pid='"+ele.Id+"'>&nbsp;</span>"+
                                "<span><a href='#'>"+ele.Name+"</a></span>"
                                )

                                )

                        });


                        $(this1).parent().append($ul);
                        $(this1).addClass("collapse");
                        $(this1).toggleClass("collapse expand");

                        $(this1).closest("li").children("ul").slideDown();
                    }
                else
                    {
                        $(this1).css({"display":"inline-block","width":"15px"});

                    }
                    $(this1).attr("data-loaded", true);
                },
                error: function () {

                    alert('error');
                }


            });

        }


        else
        {
            $(this1).toggleClass("collapse expand");
            $(this1).closest("li").children("ul").slideToggle();

        }
    });

});