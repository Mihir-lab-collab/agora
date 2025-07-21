$(document).ready(function() {
	
	var footerHeight = $('.footer').height();  
 	$('.pannel').css({'padding-bottom':logoHeight});
		   
	$('.main').css({'padding-bottom':(footerHeight-logoHeight)});
	$('.footer').css({'margin-top':(logoHeight-footerHeight)});
	 
	$('.footer').css({'height':(footerHeight-logoHeight)});
	  		
	var leftHeight = $('.left-panel').height();
	var RightHeight= $('.centerbg').height();
	
	if(RightHeight>leftHeight)
	    $('.left-panel').css({'height':RightHeight});
	else
		$('.centerbg').css({'height':leftHeight});	
	
});


