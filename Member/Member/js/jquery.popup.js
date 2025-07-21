$(document).ready(function() {
	$('.open').click(function() {
		$('#popup').slideDown('fast');

 
  var top = $('#popup').offset().top - parseFloat($('#popup').css('marginTop').replace(/auto/, 0));
  $(window).scroll(function (event) 
  {
    // what the y position of the scroll is
    var y = $(this).scrollTop();
  
    // whether that's below the form
    if (y >= top) 
	{
      // if so, ad the fixed class
      $('#popup').addClass('fixed');
    } else 
	{
      // otherwise remove it
      $('#popup').removeClass('fixed');
    }
	

	});
	
  });
  
});


$(document).ready(function() {
	$('.close').click(function() {
		$('#popup').slideUp('fast');

 
  var top = $('#popup').offset().top - parseFloat($('#popup').css('marginTop').replace(/auto/, 0));
  $(window).scroll(function (event) 
  {
    // what the y position of the scroll is
    var y = $(this).scrollTop();
  
    // whether that's below the form
    if (y >= top) 
	{
      // if so, ad the fixed class
      $('#popup').addClass('fixed');
    } else 
	{
      // otherwise remove it
      $('#popup').removeClass('fixed');
    }
	

	});
	
  });
  
});


$(document).ready(function () {  
  var top = $('#left_menu_slide').offset().top - parseFloat($('#left_menu_slide').css('marginTop').replace(/auto/, 0));
  $(window).scroll(function (event) {
    // what the y position of the scroll is
    var y = $(this).scrollTop();
  
    // whether that's below the form
    if (y >= top) {
      // if so, ad the fixed class
      $('#left_menu_slide').addClass('fixed');
	   $('#left_menu_slide_b').addClass('fixed');
    } else {
      // otherwise remove it
      $('#left_menu_slide').removeClass('fixed');
	  $('#left_menu_slide_b').removeClass('fixed');
    }
  });
});

$(document).ready(function() {
	$('#left_menu_slide').click(function() {
		$('#left_panel').slideUp('fast');
  	    $('#wrapper').addClass('bg-postion');
		$('#main').css( "padding-left","20px" );
		$('#left_menu_slide_wrap').css( "left","0" );
		$('#left_menu_slide').css( "display","none" );
		$('#left_menu_slide_b').css( "display","block" );

  });
  
});
$(document).ready(function() {
	$('#left_menu_slide_b').click(function() {
		$('#left_panel').slideDown('fast');
		 $('#wrapper').removeClass('bg-postion');
		$('#main').css( "padding-left","230px " );
		$('#left_menu_slide_wrap').css( "left","210px" );
		$('#left_menu_slide').css( "display","block" );
		$('#left_menu_slide_b').css( "display","none" );

  });
  
});

$(document).ready(function() {
	$('#full_view').click(function() {
		$('#table_wrap').css( "position","absolute" );
		$('#table_wrap').css( "left","0" );
		$('#table_wrap').css( "top","0" );
		$('#table_wrap').css( "width","100%" );
		$('html').css( "overflow-y","auto" );
  });
  
});
