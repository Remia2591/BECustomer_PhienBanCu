// ====== DỮ LIỆU SẢN PHẨM ======
var SAN_PHAM = [
    { ma: 1, ten: "Sổ tay bìa da A5", hinh: "1.jpg", gia: 65000, loai: "so", mota: "Bìa da, khổ A5, 200 trang giấy kem." },
    { ma: 2, ten: "Sổ lò xo kẻ ngang", hinh: "2.jpg", gia: 25000, loai: "so", mota: "Gáy lò xo, 80 trang kẻ ngang, dễ lật." },
    { ma: 3, ten: "Sổ planner", hinh: "3.jpg", gia: 80000, loai: "so", mota: "Sổ lên kế hoạch theo tuần, tháng, có trang mục tiêu." },
    { ma: 4, ten: "Bút bi xanh", hinh: "4.jpg", gia: 5000, loai: "but", mota: "Mực xanh, viết êm, ngòi 0.5 mm." },
    { ma: 5, ten: "Bút gel đen", hinh: "5.jpg", gia: 8000, loai: "but", mota: "Mực gel đen, nét đều, khô nhanh." },
    { ma: 6, ten: "Bộ bút highlight nhiều màu", hinh: "6.jpg", gia: 45000, loai: "but", mota: "Bộ bút dạ quang nhiều màu để đánh dấu ghi chú." },
    { ma: 7, ten: "Hộp bút vải", hinh: "7.jpg", gia: 35000, loai: "dungcu", mota: "Hộp bút vải có khóa kéo, đựng được nhiều bút." },
    { ma: 8, ten: "Thước kẻ và Ê-ke", hinh: "8.jpg", gia: 20000, loai: "dungcu", mota: "Bộ thước kẻ và ê-ke nhựa trong." },
    { ma: 9, ten: "Kéo và băng keo trang trí", hinh: "9.jpg", gia: 30000, loai: "dungcu", mota: "Kéo nhỏ và băng keo họa tiết để trang trí sổ." },
    { ma: 10, ten: "Móc khóa gấu bông", hinh: "10.jpg", gia: 25000, loai: "qua", mota: "Móc khóa gấu bông nhỏ xinh, treo túi hoặc chìa khóa." },
    { ma: 11, ten: "Hộp quà mini", hinh: "11.jpg", gia: 40000, loai: "qua", mota: "Hộp quà mini, dùng để gói quà tặng." },
    { ma: 12, ten: "Cốc sứ in hình", hinh: "12.jpg", gia: 55000, loai: "qua", mota: "Cốc sứ in hình dễ thương, dung tích khoảng 300 ml." },
    { ma: 101, ten: "Combo Back to School", hinh: "101.jpg", gia: 150000, loai: "combo", mota: "Combo đồ dùng học tập cho năm học mới: sổ, bút, hộp bút, thước." },
    { ma: 102, ten: "Combo Văn phòng", hinh: "102.jpg", gia: 180000, loai: "combo", mota: "Combo cho bàn làm việc: sổ planner, bút, kéo, băng keo." },
    { ma: 103, ten: "Combo Quà tặng", hinh: "103.jpg", gia: 200000, loai: "combo", mota: "Combo quà tặng: hộp quà, cốc sứ, móc khóa gấu bông." },
    { ma: 104, ten: "Combo Sinh viên", hinh: "104.jpg", gia: 120000, loai: "combo", mota: "Combo cho sinh viên: sổ lò xo, bút bi, bút gel, bút highlight." }
];

// ====== HÀM TIỆN ÍCH ======
function dinhDang(n) { return n.toLocaleString("vi-VN") + " đ"; }

function timSP(ma) {
    for (var i = 0; i < SAN_PHAM.length; i++) {
        if (SAN_PHAM[i].ma == ma) return SAN_PHAM[i];
    }
    return null;
}

function layDS(laCombo) {
    var kq = [];
    for (var i = 0; i < SAN_PHAM.length; i++) {
        if ((SAN_PHAM[i].loai == "combo") == laCombo) kq.push(SAN_PHAM[i]);
    }
    return kq;
}

function doc(key, macDinh) {
    try {
        var v = localStorage.getItem(key);
        return v ? JSON.parse(v) : macDinh;
    } catch (e) { return macDinh; }
}
function ghi(key, v) { localStorage.setItem(key, JSON.stringify(v)); }

function layThamSo(ten) {
    return new URLSearchParams(location.search).get(ten);
}

// ====== GIỎ HÀNG ======
function layGio() { return doc("gioHang", []); }

function themGio(ma, sl) {
    var g = layGio();
    var thay = false;
    for (var i = 0; i < g.length; i++) {
        if (g[i].ma == ma) { g[i].sl += sl; thay = true; }
    }
    if (!thay) g.push({ ma: ma, sl: sl });
    ghi("gioHang", g);
    capNhatDem();
    alert("Đã thêm vào giỏ hàng");
}

function capNhatDem() {
    var g = layGio();
    var t = 0;
    for (var i = 0; i < g.length; i++) t += g[i].sl;
    var e = document.getElementById("demGio");
    if (e) e.innerText = t;
}

// ====== TÀI KHOẢN ======
function nguoiDangNhap() {
    var email = doc("dangNhap", null);
    if (!email) return null;
    var ds = doc("nguoiDung", []);
    for (var i = 0; i < ds.length; i++) {
        if (ds[i].email == email) return ds[i];
    }
    return null;
}

function dangXuat() {
    localStorage.removeItem("dangNhap");
    location.href = "~/Home/Index";
}

// ====== TÌM KIẾM ======
function timKiem() {
    var q = document.getElementById("oTim").value;
    location.href = "/Product/SanPham?q=" + encodeURIComponent(q);
}

// ====== VẼ DANH SÁCH SẢN PHẨM ======
function veSP(ds, idVung) {
    var h = "";
    for (var i = 0; i < ds.length; i++) {
        var s = ds[i];
        h += '<div class="sp">' +
            '<a href="/Product/ChiTiet?ma=' + s.ma + '">' +
            '<img src="/Content/images/' + s.hinh + '" alt="' + s.ten + '" />' +
            '<div class="sp-ten">' + s.ten + '</div></a>' +
            '<div class="sp-gia">' + dinhDang(s.gia) + '</div>' +
            '<button class="nut" onclick="themGio(' + s.ma + ',1)">Thêm vào giỏ</button>' +
            '</div>';
    }
    if (ds.length == 0) h = "<p>Không tìm thấy sản phẩm.</p>";
    document.getElementById(idVung).innerHTML = h;
}

// ====== HEADER VÀ FOOTER DÙNG CHUNG ======
async function capNhatTaiKhoan() {
    try {
        var response = await fetch("/Account/TrangThai");
        if (!response.ok) throw new Error("Không tải được trạng thái");

        var data = await response.json();
        var e = document.getElementById("taiKhoanHeader");
        if (!e) return;

        if (data.daDangNhap) {
            e.textContent = "Xin chào, " + data.hoTen + " | ";

            var nutThoat = document.createElement("a");
            nutThoat.href = "#";
            nutThoat.textContent = "Thoát";

            nutThoat.addEventListener("click", async function (event) {
                event.preventDefault();

                var tam = document.createElement("div");
                tam.innerHTML = data.tokenDangXuat;

                var inputToken = tam.querySelector(
                    'input[name="__RequestVerificationToken"]'
                );

                if (!inputToken) {
                    alert("Không tìm thấy mã bảo mật đăng xuất.");
                    return;
                }

                try {
                    var kq = await fetch("/Account/DangXuat", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/x-www-form-urlencoded"
                        },
                        body: new URLSearchParams({
                            __RequestVerificationToken: inputToken.value
                        })
                    });

                    if (!kq.ok) {
                        alert("Đăng xuất thất bại. Mã lỗi: " + kq.status);
                        return;
                    }

                    var result = await kq.json();

                    if (result.success) {
                        localStorage.removeItem("dangNhap");
                        window.location.href = result.redirectUrl;
                    }
                } catch (error) {
                    alert("Không thể kết nối đến máy chủ.");
                    console.error(error);
                }
            });

            e.appendChild(nutThoat);
        } else {
            e.innerHTML =
                '<a href="/Account/DangNhap">&#128100; Đăng nhập</a>';
        }
    } catch (error) {
        console.error(error);
    }
}
function veHeader() {
    var tk = '<span class="icon-tk" id="taiKhoanHeader">Đăng nhập</span>';
    var h = '<div class="topbar">' +
        '<a class="logo" href="index.html">Gift Shop</a>' +
        '<div class="hang-tk">' +
        '<input type="text" id="oTim" class="o-tim" placeholder="Tìm sổ tay, bút, combo..." onkeydown="if(event.key===\'Enter\')timKiem()" />' +
        '<button class="nut-tim" onclick="timKiem()">Tìm</button>' +
        '<a class="icon-tk" href="/Cart/GioHang">&#128722;<span class="dem" id="demGio">0</span></a>' +
        tk +
        '</div>' +
        '<div class="menu">' +
        '<a href="/Home/Index">Trang chủ</a>' +
        '<a href="/Product/SanPham">Sản phẩm</a>' +
        '<a href="/Home/Combo">Combo</a>' +
        '<a href="/Order/DonHang">Đơn hàng</a>' +
        '<a href="/Home/LienHe">Liên hệ</a>' +
        '</div></div>';
    var e = document.getElementById("header");
    if (e) e.innerHTML = h;
}

function veFooter() {
    var e = document.getElementById("footer");
    if (e) e.innerHTML = '<div class="chantrang">Gift Shop - Văn phòng phẩm &amp; Quà lưu niệm</div>';
}

document.addEventListener("DOMContentLoaded", function () {
    veHeader();
    capNhatTaiKhoan();
    veFooter();
    capNhatDem();
});