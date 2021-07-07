
$("rect").hover(
    (e) => {
        const y = e.clientY;
        const x = e.clientX;
        const thisDate = e.target.dataset.datename;
        let cnt = $(e.target).data().content ? $(e.target).data().content : null;
        if (!cnt) {
            cnt = "0 Решений в этот день";
        }
        $("#activityResultPopup").html(
            `<p style="top: ${y - 35}px; left: ${
            x - 56
            }px;" class="text-white bg-dark p-1 rounded border-3 position-absolute z100"><span class="text-secondary">${thisDate}</span>${
            cnt
            }</p>`
        );
    },
    () => {
        $("#activityResultPopup p").remove();
    }
);
