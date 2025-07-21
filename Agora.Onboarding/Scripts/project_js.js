
$(window).on('load', function () {
    $('.confetti').css('display', 'flex');
    setTimeout(function (fast) {
        $('.confetti').css('display', 'none');
    }, 4000);


    $(".showcontent").css({ 'display': ' none' });
    // $(".showcontent").hide();
    $(".showpagebtn").on("click", function () {

        $(".showcontent").css({ 'display': ' block' });
        $(".showpage").css({ 'display': ' none' });

    });
});





$(document).ready(function () {
    var current_fs, next_fs, previous_fs; //fieldsets
    var opacity;
    var current = 1;
    var steps = $("fieldset").length;  
   let data= setProgressBar(current);
   
    $(".next").click(function () {       
        window.scrollTo(top);
    current_fs = $(this).parent();
        next_fs = $(this).parent().next();
    
    //Add Class Active
    $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");
    
    //show the next fieldset
    next_fs.show();
    //hide the current fieldset with style
    current_fs.animate({opacity: 0}, {
    step: function(now) {
    // for making fielset appear animation
    opacity = 1 - now;
    
    current_fs.css({
    'display': 'none',
    'position': 'relative'
    });
    next_fs.css({'opacity': opacity});
    },
    duration: 500
    });
    setProgressBar(++current);
    });
    
    $(".previous").click(function(){
    
    current_fs = $(this).parent();
    previous_fs = $(this).parent().prev();
    
    //Remove class active
    $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");
    
    //show the previous fieldset
    previous_fs.show();
    
    //hide the current fieldset with style
    current_fs.animate({opacity: 0}, {
    step: function(now) {
    // for making fielset appear animation
    opacity = 1 - now;
    
    current_fs.css({
    'display': 'none',
    'position': 'relative'
    });
    previous_fs.css({'opacity': opacity});
    },
    duration: 500
    });
    setProgressBar(--current);
    });
    
    function setProgressBar(curStep){
        var percent = parseFloat(100 / steps) * curStep;
       
    percent = percent.toFixed();
    $(".progress-bar")
    .css("width",percent+"%")
    }    
    $(".submit").click(function () {
        
    return false;
    })
    
   
    });




$(function () {
    $('.wrapme .card .card-header ul li').click(function () {
        $('.wrapme .card .card-header ul li.active').removeClass('active');
        $(this).addClass('active');
    });
});


$("#FileInput").on('change', function (e) {
    var labelVal = $(".title").text();
    var oldfileName = $(this).val();
    fileName = e.target.value.split('\\').pop();

    if (oldfileName == fileName) { return false; }
    var extension = fileName.split('.').pop();

    if ($.inArray(extension, ['jpg', 'jpeg', 'png']) >= 0) {
        $(".filelabel i").removeClass().addClass('fa fa-file-image-o');
        $(".filelabel i, .filelabel .title").css({ 'color': '#208440' });
        $(".filelabel").css({ 'border': ' 2px solid #208440' });
    }
    else if (extension == 'pdf') {
        $(".filelabel i").removeClass().addClass('fa fa-file-pdf-o');
        $(".filelabel i, .filelabel .title").css({ 'color': 'red' });
        $(".filelabel").css({ 'border': ' 2px solid red' });

    }
    else if (extension == 'doc' || extension == 'docx') {
        $(".filelabel i").removeClass().addClass('fa fa-file-word-o');
        $(".filelabel i, .filelabel .title").css({ 'color': '#2388df' });
        $(".filelabel").css({ 'border': ' 2px solid #2388df' });
    }
    else {
        $(".filelabel i").removeClass().addClass('fa fa-file-o');
        $(".filelabel i, .filelabel .title").css({ 'color': 'black' });
        $(".filelabel").css({ 'border': ' 2px solid black' });
    }

    if (fileName) {
        if (fileName.length > 10) {
            $(".filelabel .title").text(fileName.slice(0, 4) + '...' + extension);
        }
        else {
            $(".filelabel .title").text(fileName);
        }
    }
    else {
        $(".filelabel .title").text(labelVal);
    }
});



$(document).ready(
    
    function () {
        $("#welldoneclick").hide();
        $(".welldoneclick").click(function () {
            $("#welldoneclick").show();
        });

       
        if ($("#progressbar li").hasClass("hr")) {
            $("#progressbar li.hr.active").find("#progressbar li.hr.active:after").css('background', '#F89C2D;' );
        }

        else {
            $("#progressbar li.hr.active:after").css( 'width','50%;' );
        }

        $(".afterfullwidth").on("click", function () {
           
            $("#progressbar li.hr").addClass("aftercss");
           
        });

        $(".afterremove").on("click", function () {
            
            $("#progressbar li.hr").removeClass("aftercss");
            
        });

        //$(".firstpage").on("click", function () {

        //    $(".showpage").show();
        //    $(".showcontent").hide();
        //    window.scrollTo(top);

        //});
       

        
    });