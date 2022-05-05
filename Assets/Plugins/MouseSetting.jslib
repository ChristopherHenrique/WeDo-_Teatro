mergeInto(LibraryManager.library, {

  SetPointerCursor: function (pointerType) {
    var a = Pointer_stringify(pointerType);
    if (window.yourFunctionName)
    {
        window.yourFunctionName(a);
    }
  },

  PrintMessage: function (message) {
    console.log(message);
  },

  OpenFrame: function (value) {
    var a = Pointer_stringify(value);
    if (window.openFrame)
    {
        window.openFrame(a);
    }
  },
  CloseFrame: function (value) {
    var a = Pointer_stringify(value);
    if (window.closeFrame)
    {
        window.closeFrame(a);
    }
  },
  CheckIsMobile: function() {
    return /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
  },

  OpenURL: function(url) {
    var a = Pointer_stringify(url);
    window.open(a, "_blank");
  },

  OpenLink: function(url) {
    var a = Pointer_stringify(url);
    window.openLink(a);
  },

  OpenInput: function() {
    window.openInput();
  },
  CloseInput: function() {
    window.closeInput();
  },

  openWindow: function(link)
  {
  	var url = Pointer_stringify(link);
      document.onmouseup = function()
      {
      	window.open(url);
      	document.onmouseup = null;
      }
  },

  checkSSH: function()
  {
    return window.location.protocol === "https:";
  }

});