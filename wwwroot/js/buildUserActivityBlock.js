let d = new Date(
    2020,
    new Date(Date.now()).getMonth(),
    new Date(Date.now()).getDay() + 6
);

var options = {
    year: "numeric",
    month: "long",
    day: "numeric"
};

function formatDate(date) {
    var dd = date.getDate();
    if (dd < 10) dd = "0" + dd;

    var mm = date.getMonth() + 1;
    if (mm < 10) mm = "0" + mm;

    var yy = date.getFullYear() % 100;
    if (yy < 10) yy = "0" + yy;

    return dd + "." + mm + "." + yy;
}

$("rect").data("content", []);
for (let i = 0; i < 52; i++) {
    for (let q = 0; q < 7; q++) {
        $(".grider").append(`
              <svg>
                <rect x=${i * 10 + i * 5} y=${
            q * 10 + q * 5
            } data-date=${d
                .toLocaleString("ru", options)
                .replace(/ /g, "&nbsp;")} data-dateof=${formatDate(
                    d
                )} stroke="pink" stroke-linecap="round" stroke-width="0.5" rx="2" ry="2" height="10" width="10" fill="#e4e7eb"></rect>
              </svg>`);
        d.setDate(d.getDate() + 1);
    }
}

$("rect").hover(
    (e) => {
        const y = e.clientY;
        const x = e.clientX;
        const thisDate = e.target.dataset.date;
        let cnt = $(e.target).data().content ? $(e.target).data().content : null;
        if (!cnt) {
            cnt = ["0 Решений в этот день"];
        }
        $("#activityResultPopup").html(
            `<p style="top: ${y - 35}px; left: ${
            x - 56
            }px;" class="text-white bg-dark p-1 rounded border-3 position-absolute z100"><span class="text-secondary">${thisDate}</span>${cnt.map(
                (item) => " " + item
            )}</p>`
        );
    },
    () => {
        $("#activityResultPopup p").remove();
    }
);
