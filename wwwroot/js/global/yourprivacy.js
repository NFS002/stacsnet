$(function() {
   var dialog = $('#privacydialog') 
   var link = $('#privacylink')

   dialog.dialog({
        autoOpen: false,
        width: 500,
        height:500,
        resizable: false,
        modal: true,
        buttons: {
            Ok: function() {
              $( this ).dialog( "close" );
            }
          }
      })

   link.on( 'click', () => dialog.dialog( 'open' ))

});