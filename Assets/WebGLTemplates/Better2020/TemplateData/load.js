var buildUrl;
      if(/iPhone|iPad|iPod|Android/i.test(navigator.userAgent))
      {
        buildUrl = "Build-mobile";
      }
      else
      {
        buildUrl = "Build-desktop";
      }
      const hideFullScreenButton = "{{{ HIDE_FULL_SCREEN_BUTTON }}}";
      const loaderUrl = buildUrl + "/{{{ LOADER_FILENAME }}}";
      const config = {
        dataUrl: buildUrl + "/{{{ DATA_FILENAME }}}",
        frameworkUrl: buildUrl + "/{{{ FRAMEWORK_FILENAME }}}",
        codeUrl: buildUrl + "/{{{ CODE_FILENAME }}}",
#if MEMORY_FILENAME
        memoryUrl: buildUrl + "/{{{ MEMORY_FILENAME }}}",
#endif
#if SYMBOLS_FILENAME
        symbolsUrl: buildUrl + "/{{{ SYMBOLS_FILENAME }}}",
#endif
        streamingAssetsUrl: "StreamingAssets",
        companyName: "{{{ COMPANY_NAME }}}",
        productName: "{{{ PRODUCT_NAME }}}",
        productVersion: "{{{ PRODUCT_VERSION }}}",
      };

      const container = document.querySelector("#unity-container");
      const canvas = document.querySelector("#unity-canvas");
      const loadingCover = document.querySelector("#loading-cover");
      const progressBarEmpty = document.querySelector("#unity-progress-bar-empty");
      const progressBarFull = document.querySelector("#unity-progress-bar-full");
      const fullscreenButton = document.querySelector("#unity-fullscreen-button");
      const spinner = document.querySelector('.spinner');

      const canFullscreen = (function() {
        for (const key of [
            'exitFullscreen',
            'webkitExitFullscreen',
            'webkitCancelFullScreen',
            'mozCancelFullScreen',
            'msExitFullscreen',
          ]) {
          if (key in document) {
            return true;
          }
        }
        return false;
      }());

      if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
        container.className = "unity-mobile";
        config.devicePixelRatio = 1;
      }
      loadingCover.style.display = "";
#if BACKGROUND_FILENAME
      canvas.style.background = "url('" + buildUrl + "/{{{ BACKGROUND_FILENAME.replace(/'/g, '%27') }}}') center / cover";
#endif
      var UnityGame;
      const script = document.createElement("script");
      script.src = loaderUrl;
      script.onload = () => {
        createUnityInstance(canvas, config, (progress) => {
          spinner.style.display = "none";
          progressBarEmpty.style.display = "";
          progressBarFull.style.width = `${Math.round(111 * progress)}%`;

          setInterval( function (){
            $('#pct').text(progressBarFull.style.width);
          },10);
          
        }).then((unityInstance) => {
          UnityGame = unityInstance;
          loadingCover.style.display = "none";
          if (canFullscreen) {
            if (!hideFullScreenButton) {
              fullscreenButton.style.display = "";
            }
            fullscreenButton.onclick = () => {
              unityInstance.SetFullscreen(1);
            };
          }
        }).catch((message) => {
          alert(message);
        });
      };
      document.body.appendChild(script);
      window.yourFunctionName = function (cursor) {
        console.log(cursor);
        $('#unity-canvas').css('cursor', cursor);
      };

      window.openLink = function(url) {
        console.log(url);
        var popup = window.open(url);
        if(!popup || popup.closed || typeof popup.closed=='undefined') 
        { 
          //POPUP BLOCKED
          UnityGame.SendMessage('GameManager', 'PopupBlocked');
        }
      }

      window.openFrame = function(frame) {
        console.log($('#' + frame));
        $('#iframe-' + frame).css("display", 'inherit');

        gsap.to($('#iframe-' + frame), { duration: 0.4, opacity: 1, onComplete: function ()  {
        }});
        gsap.to($('#iframe-' + frame), { duration: 0.4, scale: 1, onComplete: function ()  {
          $('#btn-' + frame).css("visibility", 'visible');
        }});
      }
      window.closeFrame = function(frame) {
        console.log(frame);
        UnityGame.SendMessage('GameManager', 'FrameControlClose', frame);
        $('#btn-' + frame).css("visibility", 'hidden'); 

        gsap.to($('#iframe-' + frame), { duration: 0.4, opacity: 0, onComplete: function ()  {
        }});
        gsap.to($('#iframe-' + frame), { duration: 0.4, scale: 0.6, onComplete: function ()  {
          $('#iframe-' + frame).css("display", 'none');
          if(frame == 'bilheteria'){
            $("#iframe-bilheteria").attr("src", "https://www.sympla.com.br/agenda-eventos/6td27HvSf4dbseZUhZEQksagxe7iFiUliFhGQEQwHXs?theme=light");
          }
          else if (frame == 'palco') {
            $("#iframe-palco").attr("src", "https://www.wedoentretenimento.com/wedohdstgdtsfwry13");
          }
          else {
            $("#iframe-loja").attr("src", "https://www.wedoentretenimento.com/loja");
          }
        }});
      }

      window.sendTicket = function() {
        var ticket = $('#inputTicket').val();
        closeInput("true");
        UnityGame.SendMessage('GameManager', 'RetrieveTicket', ticket);
      }

      window.openInput = function() {
        $('#inputBox').css("display", 'grid');
        $('#inputTicket').val("");

        gsap.to($('#inputBox'), { duration: 0.4, opacity: 1, onComplete: function ()  {
        }});
        gsap.to($('#inputBox'), { duration: 0.4, scale: 1, onComplete: function ()  {
          $('#btn-input').css("visibility", 'visible');
        }});
      }

      window.closeInput = function(ticketSent) {
        UnityGame.SendMessage('GameManager', 'CloseInputWindow', ticketSent);
        $('#btn-input').css("visibility", 'hidden');
        gsap.to($('#inputBox'), { duration: 0.4, opacity: 0, onComplete: function ()  {
        }});
        gsap.to($('#inputBox'), { duration: 0.4, scale: 0.6, onComplete: function ()  {
          $('#inputBox').css("display", 'none');
        }});
      }

      //removendo a load bar
      $('#unity-progress-bar-empty').css('visibility','hidden');
      $('#unity-progress-bar-full').css('visibility','hidden');
      $('.spinner').css('visibility','hidden');

      //cor do background do loading
      $('#loading-cover').css('background-color','white');

      //loading balls
      $('#loading-cover').css('background-color','white');

      gsap.to($('#logo'), { duration: 1, opacity: 1, onComplete: function (){}});
      gsap.to($('#logo'), { duration: 1, scale: 1, onComplete: function (){}});

      loadingBalls();
      function loadingBalls() {
        console.log("ldb");
        gsap.to($('#ball-1'), { duration: 0.3, opacity: 0.2, onComplete: function ()  {
          gsap.to($('#ball-2'), { duration: 0.3, opacity: 0.2, onComplete: function ()  {
            gsap.to($('#ball-3'), { duration: 0.3, opacity: 0.2, onComplete: function ()  {
              gsap.to($('#ball-4'), { duration: 0.3, opacity: 0.2, onComplete: function ()  {
                gsap.to($('#ball-5'), { duration: 0.3, opacity: 0.2, onComplete: function ()  {
                  gsap.to($('#ball-6'), { duration: 0.3, opacity: 0.2, onComplete: function ()  {
                    gsap.to($('#ball-7'), { duration: 0.3, opacity: 0.2, onComplete: function ()  {
                      gsap.to($('#ball-1'), { duration: 0.3, opacity: 1, onComplete: function ()  {
                        gsap.to($('#ball-2'), { duration: 0.3, opacity: 1, onComplete: function ()  {
                          gsap.to($('#ball-3'), { duration: 0.3, opacity: 1, onComplete: function ()  {
                            gsap.to($('#ball-4'), { duration: 0.3, opacity: 1, onComplete: function ()  {
                              gsap.to($('#ball-5'), { duration: 0.3, opacity: 1, onComplete: function ()  {
                                gsap.to($('#ball-6'), { duration: 0.3, opacity: 1, onComplete: function ()  {
                                  gsap.to($('#ball-7'), { duration: 0.3, opacity: 1, onComplete: function ()  {
                                    loadingBalls();
                                  }});
                                }});
                              }});
                            }});
                          }});
                        }});
                      }});
                    }});
                  }});
                }});
              }});
            }});
          }});
        }});
      }