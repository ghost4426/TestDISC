function generatePDF() {
    ShowLoading();
    const element = document.getElementById("result-form");
    html2pdf()
        .from(element)
        .set({
            margin: [0, 80, 0, 0],
            filename: 'Result-DISC.pdf',
            image: { type: 'png', quality: 1.0 },
            pageBreak: { mode: ['css'], before: '#graph-page' },
            jsPDF: {
                unit: 'pt',
                format: 'A4',
                orientation: 'landscape',
            },
            html2canvas: {
                scale: 2,
                //windowWidth: element    ,
                //windowHeight: element.getBoundingClientRect().height,
                onclone: (element) => {
                    const svgElements = element.body.querySelectorAll('svg');
                    Array.from(svgElements).forEach((item) => {
                        item.setAttribute('width', (item.getBoundingClientRect().width - 14).toString());
                        item.setAttribute('height', item.getBoundingClientRect().height.toString());
                        item.style.width = null;
                        item.style.height = null;

                        //const bBox = item.getBBox();
                        //item.setAttribute("x", 0);
                        //item.setAttribute("y", 0);
                        //item.setAttribute("width", bBox.width);
                        //item.setAttribute("height", bBox.height);
                    });
                }
            }
        }).toPdf().get('pdf').save().then(() => {
            $('#loading').html('Tải kết quả DISC thành công.')
        });
}

$(document).ready(function () {
    ShowLoading();
    LoadAll();
    setTimeout(() => {
        generatePDF();
    }, 1000);
});