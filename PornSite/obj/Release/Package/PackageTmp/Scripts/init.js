var width = $(window).width();
let deferredPrompt;
var firstVisit;
function lazyLoader() {
    let options = {
        root: null,
        rootMargin: '0px',
        threshold: 0.3
    };
    let observer = new IntersectionObserver(onChange, options);
    let images = document.querySelectorAll('video');
    images.forEach(img => observer.observe(img));
}
function onChange(changes, observer) {
    changes.forEach(change => {
        if (change.intersectionRatio > 0) {
            if (change.target.getAttribute("poster") == "../Content/img/video.jpg") {
                var img = new Image();
                img.src = change.target.getAttribute('data-img');
                img.onload = function () {
                    $(change.target).fadeOut(250, function () {
                        change.target.setAttribute("poster", img.src);
                        $(change.target).fadeIn(250);
                    });
                }
            }
        }
    });
}
function getRandomQuote(par) {
    var quotes;
    if (par) {
        quotes = [
            "“Sex is a part of nature. I go along with nature.” ;Marilyn Monroe",
            "“Software is like sex: it's better when it's free.” ;Linus Torvalds",
            "“My wife wants sex in the back of the car and she wants me to drive.” ;Rodney Dangerfield",
            "“Love is an ice cream sundae, with all the marvelous coverings. Sex is the cherry on top.” ;Jimmy Dean",
            "“If it wasn't for pick-pockets I'd have no sex life at all.” ;Rodney Dangerfield",
            "“I remember the first time I had sex - I kept the receipt.” ;Groucho Marx",
            "“I know a man who gave up smoking, drinking, sex, and rich food. He was healthy right up to the day he killed himself.” ;Johnny Carson",
            "“Sex without love is a meaningless experience, but as far as meaningless experiences go its pretty damn good.” ;Woody Allen",
            "“Women need a reason to have sex. Men just need a place.” ;Billy Crystal",
            "“I'm a heroine addict. I need to have sex with women who have saved someone's life.” ;Mitch Hedberg",
            "“The real story is that I had unprotected sex. That's that. That's easy.” ;Magic Johnson",
            "“Stay positive.” ;Charlie Sheen",
            "“There is no age limit on the enjoyment of sex. It keeps getting better.” ;Florence Henderson",
            "“Sex is a bad thing because it rumples the clothes.” ;Jackie Kennedy",
            "“The major civilizing force in the world is not religion, it is sex.” ;Hugh Hefner",
            "“I believe that sex is one of the most beautiful, natural, wholesome things that money can buy.” ;Steve Martin",
            "“Is sex dirty? Only when it's being done right.” ;Woody Allen",
            "“The function of muscle is to pull and not to push, except in the case of the genitals and the tongue.” ;Leonardo da Vinci",
            "“I'm such a good lover because I practice a lot on my own.” ;Woody Allen",
            "“Women fake orgasms and men fake finances.” ;Suze Orman",
            "“It's not true that I had nothing on. I had the radio on.” ;Marilyn Monroe",
            "“Fighting for peace is like screwing for virginity.” ;George Carlin",
            "“Sex is the most fun you can have without laughing.” ;Woody Allen"


        ];
    }
    else {
        quotes = [
            "“Dívejte sa na ňu. Prý masérka. Kurva je to! Vždyť já ju znám! Já s ňu žiju, ne?” ;Bohuš Stejskal",
            "“Irenko, nebuď hlupá! Já ťa mám rád aj bez peněz.” ;Bohuš Stejskal",
            "“Irenko, už jsi? Už jsi? Víte... ona musí... ona ještě není... však ona bude. Irenko, až budeš, řekni.” ;Buhuš Stejskal",
            "“Když máš strasti, tak si masti!” ;Bára",
            "“Kdyby bylo možné znovu se narodit, přál bych si, abych rostl v břiše Pamely Anderson.” ;Woody Allen",
            "“Spolehlivější než kondom je česnek, natáčky a legíny.” ;Jim Carrey",
            "“Dříve jsem skočil po všem, co se hýbalo. K tomu musím dodat, že některé dámy přitom spolupracovaly.” ;Eddie Murphy",
            "“Jsem jediný muž, který může říct, že byl v klučičí skupině Take That a také ve dvou Spice Girls.” (spal s několika zpěvačkami z této skupiny.) ;Robbie Williams",
            "“Rozdíl mezi onanií a sexem: Při tom druhém člověk pozná lidi.” ;John Cleese",
            "“Muži mají mozek a penis, ale jen tolik krve, že nemůžou obě věci fungovat současně.” ;Robin Williams",
            "“Za mého mládí byl vzduch čistější a sex špinavější.” ;Jack Nicholson",
            "“Milenka je jako láhev dobrého vína, manželka je jako dobrá láhev od vína.” ;Gerard Depardieu",
            "“Člověk nemůže spát se všemi ženami, ale měl by se o to alespoň pokusit.” ;Burt Reynolds",
            "“Za roli Batmana jsem měl vlastně dostat bolestné. Oblek byl tak úzký, že byly každý večer moje vajíčka úplně prázdná.” ;George Clooney",
            "“Sex je věc, při které člověk může vyměnit vztek za trochu příjemného času.” ;John Barrymore",
            "“Sexuální výuka ve školách může být dobrá věc, ale myslím si, že by děti neměly z tohoto předmětu dostávat domácí úkoly.” ;Bill Cosby",
            "“Být sexsymbolem je to, o čem každý sní. Tím se pomstí všem dívkám, které nás ve škole odmítaly.” ;Orlando Bloom",
            "“Jsem natolik fantastický milenec, protože na sobě pracuji každý den.” ;Woody Allen",
            "“Dříve jsem chtěl pětkrát denně sex. Bylo to deprimující. Dneska už to nechci - a to je také deprimující.” ;Ben Affleck",
            "“Sex je velká zábava, kterou si člověk může užívat, aniž by se musel smát.” ;Steve Martin",
            "“Bisexualita zdvojnásobuje šanci na rande.” ;Woody Allen",
            "“Věrní muži žijí o deset let déle. Ale co z toho mají?” ;Will Smith",
            "“Viagru si beru jen když v mé posteli leží více než jedna žena.” ;Jack Nicholson",
            "“Homosexuální manželství by mělo být uzavřeno mezi mužem a ženou.” ;Arnold Schwarzenegger",
            "“Láska je skvělá nemoc, při které musíme hned ve dvou do postele.” ;Robert Lembke",
            "“Však vašu piču jsem taky neviděl a věřím, že ju máte!” ;Bohuš Stejskal",
        ];
    }
    var ran = Math.floor((Math.random() * quotes.length));
    return quotes[ran];

};
var iOS = !!navigator.platform && /iPad|iPhone|iPod/.test(navigator.platform);
var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
$(document).ready(function () {
    var match = document.cookie.match(new RegExp('(^| )' + 'Language' + '=([^;]+)'));
    if (match) {
        if (!match[2].includes("cs")) {
            document.getElementsByTagName("title")[0].innerHTML = "Watch porn safely! - fapping.guru";
            if (width > 1149) {
                var quote = getRandomQuote(true).split(";");
                $("#quote").html(quote[0] + "<br>" + "- " + "<strong>" + quote[1] + "</strong>");
            }
        }
        else if (width > 1149) {
            var quote = getRandomQuote(false).split(";");
            $("#quote").html(quote[0] + "<br>" + "- " + "<strong>" + quote[1] + "</strong>");
        }
    }
    if (document.cookie.match(/^(.*;)?\s*History\s*=\s*[^;]+(.*)?$/)) {
        $("#cookies").css('display', 'none');
    }
    else {
        firstVisit = true;
    }
    if (!window.navigator.userAgent.indexOf("Edge") > -1) {
        navigator.serviceWorker && navigator.serviceWorker.register('./sw.js').then(function (registration) {
        });
    }
    window.addEventListener('beforeinstallprompt', (e) => {
        e.preventDefault();
        if (firstVisit) {
            $("#pwa").css("display", "flex");
            deferredPrompt = e;
        }
    });
    if (navigator.userAgent.indexOf("Firefox") != -1 && width > 960) {
        $("#firefox-error").css("display", "block");
    }

    if (!isChrome) {
        iOS = true;
    }
    $(document).on("click", "#confirm", function () {
        if (width > 760) {
            var x = ($(".img-category"));
            for (var i = 0; i < 4; i++) {
                x[i].src = x[i].getAttribute("data-src");
            }
        }
        if (width > 1149) {
            document.getElementById("excel").src = "../Content/img/excel.png";
        }
        $("body").removeClass("modal-on");
        $('#modal-confirmation').css('display', 'none');
        $('main').addClass('animated fadeInUp');
        if (!iOS && isChrome && width < 960) {
            $(".tip").css("display", "block");
        }
        setTimeout(function () {
            $("main").removeClass("animated fadeInUp");
            lazyLoader();
        }, 501);
    });
});