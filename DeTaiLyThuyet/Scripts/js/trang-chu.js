veSP(layDS(false), "ds-le");
veSP(layDS(true), "ds-combo");

// Banner tự chuyển slide mỗi 3 giây
var slides = document.querySelectorAll(".slide");
var chams = document.querySelectorAll(".chamtron");
var vt = 0;
setInterval(function () {
    slides[vt].classList.remove("hien");
    chams[vt].classList.remove("on");
    vt = (vt + 1) % slides.length;
    slides[vt].classList.add("hien");
    chams[vt].classList.add("on");
}, 3000);

// Đồng hồ đếm ngược 1 giờ
var conLai = 3600;
function hai(n) { return n < 10 ? "0" + n : n; }
setInterval(function () {
    if (conLai > 0) conLai--;
    document.getElementById("gio").innerText = hai(Math.floor(conLai / 3600));
    document.getElementById("phut").innerText = hai(Math.floor(conLai % 3600 / 60));
    document.getElementById("giay").innerText = hai(conLai % 60);
}, 1000);