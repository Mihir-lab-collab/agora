
(function(jQuery){
     jQuery.fn.extend({  
         accordion: function() {       
            return this.each(function() {
            	
            	var $ul = $(this);
            	
				if($ul.data('accordiated'))
					return false;
													
				$.each($ul.find('ul, li>div'), function(){
					$(this).data('accordiated', true);
					$(this).hide();
				});
				
				$.each($ul.find('a'), function(){
					$(this).click(function(e){
						activate(this);
						return void(0);
					});
				});
				
				var active = (location.pathname) ? $(this).find("a[href='.." + location.pathname + "']")[0] : '';
				//var active = (location.hash) ? $(this).find('a[href=' + location.hash + ']')[0] : '';
               // console.log(active);
				
				//var active = (location.pathname) ? $(this).find("a[href='" + location.pathname.split("/")[2] + "']")[0] : '';


				if(active){
				    //activate(active, 'toggle');
				    $(active).addClass('selected');				
				    $(active).parents().addClass('active');
				    $(active).parents().show();
				   //$(active).parents().parents().addClass('active');
				}
				
				function activate(el,effect){
					$(el).parents('li').toggleClass('active').siblings().removeClass('active').children('ul, div').slideUp('fast');
					$(el).siblings('ul, div')[(effect || 'slideToggle')]((!effect)?'fast':null);
				}
				
            });
        } 
    }); 
})(jQuery);

		// <![CDATA[	

		$(document).ready(function () {
		    $('ul').accordion();
		    var referrer = $('.left_menu').find('li.active ul');
		    referrer.show();
		});
				
		// ]]>