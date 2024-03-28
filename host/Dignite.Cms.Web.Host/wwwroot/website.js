(function () {
    abp.utils.createNamespace(window, 'dignite.website');

    /**
    * 网站的默认语言不是浏览器首选语言时，提示用户选择本地化语言
    */
    dignite.website.promptSelectLocaleLanguage = function () {
        /**
         * 获取浏览器的首选语言
         */
        let browserLanguage = navigator.language
            ? navigator.language
            : navigator.browserLanguage;

        var currentUrl = document.location.pathname + document.location.search;
        var changeLocalLanguageStorageName = "Dignite.ChangeLocalLanguage";
        
        if (document.location.pathname == "/"
            && browserLanguage.substring(0, 2) != abp.localization.currentCulture.twoLetterIsoLanguageName
            && !localStorage.hasOwnProperty(changeLocalLanguageStorageName)) {

            //获取站点信息，包含站点支持的区域列表
            dignite.cms.public.sites.sitePublic.findByHost(document.location.protocol + "//" + document.location.host)
                .then(function (site) {
                    try {
                        site.languages.forEach((siteLanguage) => {
                            //如果浏览器语言在站点区域中，则给予提示
                            if (siteLanguage.cultureName.toLowerCase() == browserLanguage.toLowerCase()
                                || siteLanguage.cultureName.substring(0, 2).toLowerCase() == browserLanguage.toLowerCase()) {
                                var browserCultureName = siteLanguage.cultureName;
                                var currentCultureName = abp.localization.currentCulture.cultureName;
                                var browserUiCultureName = null;

                                try {
                                    abp.localization.languages.forEach((abpLanguage) => {
                                        if (siteLanguage.cultureName.toLowerCase() === abpLanguage.cultureName.toLowerCase()) {
                                            browserUiCultureName = abpLanguage.uiCultureName;
                                            throw new Error('LoopInterrupt');//满足条件，跳出循环
                                        }
                                    });
                                } catch (e) {
                                    if (e.message !== "LoopInterrupt") throw e
                                }

                                //远程获取提示的UI
                                abp.ajax({
                                    dataType: "html",
                                    type: 'GET',
                                    url: '/common/LocaleModal?culture=' + browserCultureName + '&ui-culture=' + browserUiCultureName
                                }).then(function (result) {
                                    $(document.body).append( result);
                                    var localeModal = new bootstrap.Modal('#localeModal');
                                    var btnLocaleCultureChange = document.getElementById('btnLocaleCultureChange');
                                    var btnLocaleCultureCancel = document.getElementById('btnLocaleCultureCancel');

                                    //打开提示窗口
                                    localeModal.show();

                                    //用户如果点击了切换，则跳转到本地语言页面
                                    btnLocaleCultureChange.addEventListener('click', function () {
                                        localStorage.setItem(changeLocalLanguageStorageName, browserCultureName);
                                        window.location.href = browserCultureName + currentUrl;
                                    });

                                    //如果用户点击了取消，则不跳转
                                    btnLocaleCultureCancel.addEventListener('click', function () {
                                        localStorage.setItem(changeLocalLanguageStorageName, currentCultureName);
                                    });
                                });

                                throw new Error('LoopInterrupt');//满足条件，跳出循环
                            }
                        });
                    } catch (e) {
                        if (e.message !== "LoopInterrupt") throw e
                    }
                });
        }
    };
})();


/**
* 带有品牌LOGO的这个背景是放在首页顶部的，使用如下的逻辑
*/
(function () {
    //圆形背景元素
    let shapeBgElement = document.getElementById('shape-brand-bg');
    window.addEventListener('scroll', function () {
        if (shapeBgElement != null) {
            let scrolled = window.scrollY;

            // Calculate the border radius based on scroll position
            let borderRadius = Math.max(50 - scrolled / 10, 50); // 后面这个参数表示圆角值

            // Calculate the width based on scroll position
            let width = Math.max(400 + scrolled / 0.6, 0);

            // Calculate the height based on scroll position
            let height = Math.max(400 + scrolled / 0.6, 0);

            shapeBgElement.style.borderRadius = `${borderRadius}%`;
            shapeBgElement.style.width = `${width}px`;
            shapeBgElement.style.height = `${height}px`;
        }
    });
})();



/**
* 由上向下滚动页面指定位置时，图形背景的动画
*/
(function () {
    //圆形背景区域
    let shapeBackgroundAreaElements = document.getElementsByClassName('shape-background-area');
    window.addEventListener('scroll', function () {
        Array.prototype.forEach.call(shapeBackgroundAreaElements, function (element) {
            shapeBgElement = element.getElementsByClassName('shape-bg')[0];
            const rect = element.getBoundingClientRect();
            const distanceToBottom = window.innerHeight - rect.bottom;
            if (distanceToBottom > 0) {

                // Calculate the border radius based on scroll position
                let borderRadius = Math.max(50 - distanceToBottom / 10, 50); // 后面这个参数表示圆角值

                // Calculate the width based on scroll position
                let width = Math.max(0 + distanceToBottom / 0.5, 0);

                // Calculate the height based on scroll position
                let height = Math.max(0 + distanceToBottom / 0.5, 0);

                shapeBgElement.style.borderRadius = `${borderRadius}%`;
                shapeBgElement.style.width = `${width}px`;
                shapeBgElement.style.height = `${height}px`;
            }
        });
    });
})();

/**
 * 结合animate.css,使用wow执行元素出现在屏幕中时启动动画
 */
(function () {
    var wow = new WOW({
        animateClass: 'animate__animated'
    });
    wow.init();
})();

/**
 * 使用swiper，自动向左滚动元素
 * 比较自动滚动品牌LOGO
 */
(function () {
    new Swiper('.auto-horizontal-scrolling', {
        centeredSlides: true,
        loop: true,
        allowTouchMove: true,
        autoplay: {
            delay: 0,
            disableOnInteraction: false
        },
        speed: 5000,
        grabCursor: false,
        mousewheelControl: false,
        keyboardControl: false,
        breakpoints: {
            360: {
                slidesPerView: 2,
            },
            640: {
                slidesPerView: 3.5
            },
            960: {
                slidesPerView: 4.5
            },
            1200: {
                slidesPerView: 6
            },
        }
    });
})();

/**
 * 星空背景动画
 */
(function () {
    /** 这个背景动画使用animate v2.2.0制作
    * 该JS库的最新版本已到V3.X，经测试无法使用
    * 关于V2.2.0的使用方法，请参照https://www.npmjs.com/package/animejs/v/2.2.0
    */
    var maxElements = 200;
    var duration = 7000;
    var toAnimate = [];
    var w = window.innerWidth > window.innerHeight ? window.innerWidth : window.innerHeight;
    var colors = ['#FF324A', '#31FFA6', '#206EFF', '#FFFF99'];

    //星空动画的区域
    var container = document.getElementById('starry-sky-container')
    if (container != null) {
        var createElements = (function () {
            var fragment = document.createDocumentFragment();
            for (var i = 0; i < maxElements; i++) {
                var el = document.createElement('div');
                el.classList.add('starry-sky-background')
                el.style.background = colors[anime.random(0, 3)];
                el.style.transform = 'rotate(' + anime.random(-360, 360) + 'deg)';
                toAnimate.push(el);
                fragment.appendChild(el);
            }
            container.appendChild(fragment);
        })();

        var animate = function (el) {
            anime({
                targets: el,
                rotate: anime.getValue(el, 'rotate'),
                translateX: [0, w / 2],
                translateY: [0, w / 2],
                scale: [0, 2],
                delay: anime.random(0, duration),
                duration: duration,
                easing: "linear",
                loop: true
            });
        }

        toAnimate.forEach(animate);
    }
})();

/**
 * 向下滚动页面时，隐藏指示图标，向上滚动时再次显示
 */
(function () {
    var scrollDownIconElement = document.getElementById('scroll-down-icon');
    if (scrollDownIconElement != null) {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 500) {
                scrollDownIconElement.classList.add('d-none');
            }
            else {
                scrollDownIconElement.classList.remove('d-none');
            }
        });
    }
})();

/**
 * 解析ckeditor中引入的视频
 */
$(function () {
    // Select all <figure> elements with class "media"
    var figureElements = document.querySelectorAll('figure.media');
    var mediaEmbedProviders = [
        {
            name: 'ixigua',
            url: /^https:\/\/www\.ixigua\.com\/(\d+)(\?logTag=[\w\d]+)?/,
            html: match => {
                return `<iframe src='https://www.ixigua.com/iframe/${match[1]}?autoplay=0' title="Ixigua video player" allowFullScreen></iframe>`;
            }
        },
        {
            name: 'youtube',
            url: /https:\/\/www\.youtube\.com\/watch\?v=([^"']+)?/,
            html: match => {
                return `<iframe src="https://www.youtube.com/embed/${match[1]}" title="YouTube video player" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen></iframe>`;
            }
        }
    ];


    // Loop through each <figure> element
    figureElements.forEach(function (figure) {
        var oembedElement = figure.firstChild;

        // 提取匹配结果
        mediaEmbedProviders.forEach(function (provider) {
            var match = oembedElement.getAttribute('url').match(provider.url);
            if (match && match.length > 1) {
                var videoContainer = document.createElement('div');
                videoContainer.classList.add('ratio', 'ratio-16x9');
                videoContainer.innerHTML = provider.html(match);
                figure.appendChild(videoContainer);
            }
        });
    });
});

$(function () {
    var masonryRow = document.querySelector('.row[data-masonry]'); // Targeting the Masonry container
    if (masonryRow) {
        imagesLoaded(masonryRow, function () {
            // Initialize Masonry after all images have loaded
            new Masonry(masonryRow, {
                itemSelector: '.col, .col-md-6, .col-lg-4, .col-xl-3', // Targeting the grid items
                percentPosition: true // Since you have 'percentPosition' in your data attribute
            });
        });
    }
});

(function ($) {
    var l = abp.localization.getResource("CmsKit");
    $(".contact-form").on('submit', '', function (e) {
        e.preventDefault();
        var $form = $(this);
        if ($form.valid()) {
            var formAsObject = $form.serializeFormToObject();
            volo.cmsKit.public.contact.contactPublic.sendMessage(
                {
                    contactName: formAsObject.input.contactName,
                    name: formAsObject.input.name,
                    subject: formAsObject.input.subject,
                    email: formAsObject.input.emailAddress,
                    message: formAsObject.input.message,
                    recaptchaToken: formAsObject.input.recaptchaToken
                }
            ).then(function () {
                abp.message.success(l("ContactSuccess"))
            })
        }
    });
})(jQuery);