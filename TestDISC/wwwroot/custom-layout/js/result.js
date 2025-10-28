function DrawLine(startId, endId, typeGraph, color = `#16aaff`, isDot = false, index = 0) {

    var b1 = document.getElementById(startId).getBoundingClientRect();
    var b2 = document.getElementById(endId).getBoundingClientRect();
    var offsetSVG = document.getElementById('full-' + typeGraph).getBoundingClientRect();

    var newLine = document.createElementNS('http://www.w3.org/2000/svg', 'line');

    newLine.setAttribute('id', 'line_' + startId + '_' + endId);
    newLine.setAttribute('x1', b1.left - offsetSVG.left + b1.width / 2);
    newLine.setAttribute('y1', b1.top - offsetSVG.top + b1.height / 2);
    newLine.setAttribute('x2', b2.left - offsetSVG.left + b2.width / 2);
    newLine.setAttribute('y2', b2.top - offsetSVG.top + b2.height / 2);
    newLine.setAttribute('style', `stroke: ${color}; stroke-width: 2;`);

    if (isDot) {
        if (index === 1) {
            var cc = document.createElementNS('http://www.w3.org/2000/svg', "circle")
            cc.setAttribute("cx", b1.left - offsetSVG.left + b1.width / 2)
            cc.setAttribute("cy", b1.top - offsetSVG.top + b1.height / 2)
            cc.setAttribute("r", 5)
            cc.setAttribute("fill", `${color}`)

            document.getElementById("full-" + typeGraph).append(cc);
        }

        var cc = document.createElementNS('http://www.w3.org/2000/svg', "circle")
        cc.setAttribute("cx", b2.left - offsetSVG.left + b2.width / 2)
        cc.setAttribute("cy", b2.top - offsetSVG.top + b2.height / 2)
        cc.setAttribute("r", 5)
        cc.setAttribute("fill", `${color}`)

        document.getElementById("full-" + typeGraph).append(cc);
    }

    document.getElementById("full-" + typeGraph).append(newLine);
}

$(document).ready(function () {
    if ($('#page').val() !== 'PDF') {
        LoadAll();
        DetectBanner();

        //$(window).resize(function () {
        //    DetectBanner();
        //});
    }
});

$('#btn-export').click(function (e) {
    e.preventDefault();
    window.open("/Home/FormPDF?useranswerid=" + $('#useranswerid').val(), "_blank", "toolbar=no,scrollbars=yes,resizable=yes,width=400,height=500");
});

function DetectBanner() {
    var width = $(document).width();
    if (width < 500) {
        $('.my-banner').attr("src", "/images/banner.png");
    } else {
        $('.my-banner').attr("src", "/images/banner-big.png");
    }
}