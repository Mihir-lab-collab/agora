// JScript File

//@ shahin@themorningoutline.com provided this UNDER GNU GPL 
//@ For more inforamtion visit www.themorningoutline.com

var initProgressPanel=false;
var prgCounter=0;
var strLoadingMessage ='Loading...';
   


 function initLoader(strSplash)
 {
 document.body.scroll='no';
                document.body.style.overflow='hidden';
    if (strSplash) strLoadingMessage =strSplash;
    var myNewObj= document.getElementById('divContainer');
    if (!myNewObj )
    {
    strID='divContainer';
    strClass='divContainer';
           document.body.style.overflow='hidden';
           myNewObj =  createNewDiv( strID,strClass);
           myNewObj.style.height=window.screen.height;
           myNewObj.style.width=window.screen.width;
           document.body.appendChild(myNewObj);
     }
   
//     var myNewObj= document.getElementById('divLoadingStat');
//     if (!myNewObj )
//     {
//            strID='divLoadingStat';  
//            strClass='divLoadingStat';
//            myNewObj =  createNewDiv( strID,strClass);        
//            var mytext=document.createTextNode(strLoadingMessage);
//            myNewObj.appendChild(mytext);
//            document.getElementById('divContainer').appendChild(myNewObj);
//       
//        }
  
   var myNewObj= document.getElementById('divLoaderBack');
    if (!myNewObj )
    {
            strID='divLoaderBack';
            strClass='divLoaderBack';

            myNewObj =  createNewDiv( strID,strClass);
            document.getElementById('divContainer').appendChild(myNewObj);
     }
     var myNewObj= document.getElementById('divLoaderImage');
    if (!myNewObj )
    {
            strID='divLoaderImage';
            strClass='divLoaderImage';

            myNewObj =  createNewDiv( strID,strClass);
            document.getElementById('divLoaderBack').appendChild(myNewObj);
     }
      var myNewObj= document.getElementById('divLoaderText');
    if (!myNewObj )
    {
            strID='divLoaderText';
            strClass='divLoaderText';

            myNewObj =  createNewDiv( strID,strClass);
            document.getElementById('divLoaderBack').appendChild(myNewObj);
     }
         var imgLoader=document.getElementById('imgLoader')
if (!imgLoader)
        {
                
            imgLoader =  createImage();
            
            document.getElementById('divLoaderImage').appendChild(imgLoader);
     
        }
        var mytext1=document.createTextNode('Loading please wait....');
        document.getElementById('divLoaderText').appendChild(mytext1);
        var myNewObj= document.getElementById('divLoaderProgress');
     if (!myNewObj )
        {
     
            strID='divLoaderProgress';
            strClass='divLoaderProgress'
            myNewObj =  createNewDiv( strID,strClass);
            document.getElementById('divLoaderBack').appendChild(myNewObj);
     
        }
        initProgressPanel=true;
          
     }
       
       
       function setProgress(intPrc,strMessage)
  {
  
       if (!initProgressPanel) initLoader('Loading...');
      if (strMessage)  strLoadingMessage=strMessage;
      if(!intPrc) return
      
      var mytext=document.createTextNode( strLoadingMessage+' ' + prgCounter +'%');
      
//      var lodStat= document.getElementById('divLoadingStat');
//          lodStat.removeChild(lodStat.lastChild );
//          lodStat.appendChild(mytext);
          prgCounter++;
          prgDiv= document.getElementById('divLoaderProgress');
          
          prgDiv.style.display='none';
           prgDiv.style.width=prgCounter*1+'px';
       if (prgCounter<=intPrc) 
           {
                document.body.scroll='no';
                document.body.style.overflow='hidden';
                self.setTimeout( 'setProgress('+intPrc+')',0.1);
                //completed();
            }
            else if(prgCounter>100)
            {
            document.body.scroll='yes';
            document.body.style.overflow='visible';
                 completed();
            }
 
   }

function completed()
{
  document.body.removeChild(document.getElementById('divContainer'));
}
        function createNewDiv()
        {
            newDiv = document.createElement('div');
            newDiv.id=strID;
            var styleCollection = newDiv.style;
            newDiv.className=strClass;
            return newDiv;

        }
   function resetProgress()
     {
            prgCounter=0;
             strLoadingMessage ='Loading...';
     }
function createImage()
        {
        
            newImg = document.createElement('img');
            
            newImg.id="imgLoader2";
           // var styleCollection = newDiv.style;
           // newDiv.className=strClass;
           newImg.src="../../images/indicator_blueWheel.gif";
          newImg.style.height="32px";
          newImg.style.width="32px";
            return newImg;

        }

-->