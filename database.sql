-- CREATE DATABASE QLTV
-- DROP DATABASE QLTV
-- USE QLTV
SET DATEFORMAT dmy; 

CREATE TABLE DOCGIA (
	MaDocGia varchar(50) not null primary key,
	HoVaten nvarchar(50) not null, 
	MaLoaiDocGia varchar(50) not null,
	NgaySinh datetime not null,
	DiaChi nvarchar(150) not null,
	Email varchar(50),
	NgayLapThe datetime not null,
	NgayHetHan datetime not null,
	TongNo INT not null
);

select *from DOCGIA
CREATE TABLE LOAIDOCGIA(
	MaLoaiDocGia varchar(50) not null primary key,
	TenLoaiDocGia nvarchar(50) not null
);

CREATE TABLE PHIEUNHAPSACH(
	MaPhieuNhapSach varchar(50) not null primary key,
	NgayNhap datetime not null,
	TongTien int not null
)


CREATE TABLE CT_PHIEUNHAPSACH(
	MaCTPhieuNhapSach varchar(50) not null primary key,
	MaPhieuNhapSach varchar(50) not null,
	MaSach varchar(50) not null,
	SoLuong int not null,
	DonGia INT not null
);

CREATE TABLE SACH(
	MaSach varchar(50) not null primary key,
	TenSach nvarchar(50) not null,
	MaDauSach varchar(50),
	NamXuatBan int ,
	NhaXuatBan nvarchar(50)
);

CREATE TABLE DAUSACH(
	MaDauSach varchar(50) not null primary key,
	TenDauSach nvarchar(50) not null,
	MaTheLoai varchar(50)
);

CREATE TABLE CUONSACH(
	MaCuonSach varchar(50) not null primary key,
	MaSach varchar(50),
	TinhTrang bit
)

CREATE TABLE CT_TACGIA(
	MaDauSach varchar(50) not null,
	MaTacGia varchar(50) not null,
	CONSTRAINT PK_CT_TACGIA PRIMARY KEY (MaDauSach, MaTacGia)
)

CREATE TABLE TACGIA(
	MaTacGia varchar(50) not null primary key,
	TenTacGia nvarchar(50) not null
)

CREATE TABLE THELOAI(
	MaTheLoai varchar(50) not null primary key,
	TenTheLoai nvarchar(50) not null
)

CREATE TABLE PHIEUMUONTRASACH(
	MaPhieuMuonTraSach varchar(50) not null primary key,
	MaDocGia varchar(50) not null,
	NgayMuon datetime not null,
	MaCuonSach varchar(50) not null,
	NgayPhaiTra datetime not null,
	NgayTra datetime,
	TienPhatKyNay INT not null,
	SoTienTra INT,
	ConLai INT
)


CREATE TABLE PHIEUTHUTIENPHAT(
	MaPhieuThuTien varchar(50) not null primary key,
	MaDocGia varchar(50) not null,
	NgayThu datetime not null,
	SoTienThu int not null,
	ConLai int
)
select * from DOCGIA
CREATE TABLE THAMSO(
	TenThamSo varchar(50),
	GiaTri int
)

CREATE TABLE BAOCAOTRATRE(
	MaBaoCaoTraTre varchar(50) not null primary key,
	NgayLapBaoCao datetime,
	MaCuonSach varchar(50),
	NgayMuon datetime,
	SoNgayTraTre int
)

CREATE TABLE BAOCAOMUONSACH(
	MaBaoCaoMuonSach varchar(50) not null primary key,
	Thang INT,
	NAM INT,
	TongSoLuotMuon int
)

CREATE TABLE CT_BCMUONSACH(
	MaBaoCaoMuonSach varchar(50),
	MaTheLoai varchar(50),
	SoLuotMuon int,
	TiLe float,
	CONSTRAINT PK_CT_BCMUONSACH PRIMARY KEY (MaBaoCaoMuonSach, MaTheLoai)
)

ALTER TABLE DOCGIA
ADD CONSTRAINT FK_LoaiDocGia
FOREIGN KEY (MaLoaiDocGia) REFERENCES LOAIDOCGIA(MaLoaiDocGia);

ALTER TABLE CT_PHIEUNHAPSACH
ADD CONSTRAINT FK_PhieuNhapSach
FOREIGN KEY (MaPhieuNhapSach) REFERENCES PHIEUNHAPSACH(MaPhieuNhapSach);

ALTER TABLE CT_PHIEUNHAPSACH
ADD CONSTRAINT FK_Sach
FOREIGN KEY (MaSach) REFERENCES SACH(MaSach);

ALTER TABLE SACH
ADD CONSTRAINT FK_DauSach
FOREIGN KEY (MaDauSach) REFERENCES DAUSACH(MaDauSach);

ALTER TABLE DAUSACH
ADD CONSTRAINT FK_TheLoai
FOREIGN KEY (MaTheLoai) REFERENCES THELOAI(MaTheLoai);

ALTER TABLE CUONSACH
ADD CONSTRAINT FK_Sach_CuonSach
FOREIGN KEY (MaSach) REFERENCES SACH(MaSach);

ALTER TABLE CT_TACGIA
ADD CONSTRAINT FK_DauSach_TacGia
FOREIGN KEY (MaDauSach) REFERENCES DAUSACH(MaDauSach);

ALTER TABLE CT_TACGIA
ADD CONSTRAINT FK_TacGia
FOREIGN KEY (MaTacGia) REFERENCES TACGIA(MaTacGia);

ALTER TABLE PHIEUMUONTRASACH
ADD CONSTRAINT FK_DocGia
FOREIGN KEY (MaDocGia) REFERENCES DOCGIA(MaDocGia);

ALTER TABLE PHIEUMUONTRASACH
ADD CONSTRAINT FK_CuonSach
FOREIGN KEY (MaCuonSach) REFERENCES CUONSACH(MaCuonSach);


ALTER TABLE PHIEUTHUTIENPHAT
ADD CONSTRAINT FK_DocGia_PhieuThuTienPhat
FOREIGN KEY (MaDocGia) REFERENCES DOCGIA(MaDocGia);

ALTER TABLE CT_BCMUONSACH
ADD CONSTRAINT FK_BaoCaoMuonSach
FOREIGN KEY (MaBaoCaoMuonSach) REFERENCES BAOCAOMUONSACH(MaBaoCaoMuonSach);

ALTER TABLE CT_BCMUONSACH
ADD CONSTRAINT FK_TheLoai_BaoCaoMuonSach
FOREIGN KEY (MaTheLoai) REFERENCES THELOAI(MaTheLoai);

-- Insert data into THAMSO table
INSERT INTO THAMSO (TenThamSo, GiaTri)
VALUES
    ('TuoiToiThieu', 18),
    ('TuoiToiDa', 55),
    ('ThoiHanThe', 6),
    ('KhoangCachNamXuatBan', 8),
    ('SoNgayMuonToiDa', 4),
    ('SoSachMuonToiDa', 5),
    ('SoTienPhatMoiNgay', 1000);

-- Insert data into LOAIDOCGIA table
INSERT INTO LOAIDOCGIA (MaLoaiDocGia, TenLoaiDocGia) VALUES
('LDG001', N'Thường'),
('LDG002', N'Premium');

-- Insert data into DOCGIA table
INSERT INTO DOCGIA (MaDocGia, HoVaten, MaLoaiDocGia, NgaySinh, DiaChi, Email, NgayLapThe, NgayHetHan, TongNo) VALUES
('DG001', N'Nguyễn Văn A', 'LDG001', '15/05/1990', N'123 Đường Lê Lợi, TP HCM', 'nguyenvana@example.com', '01/01/2022', '01/07/2022', 0),
('DG002', N'Trần Thị B', 'LDG002', '20/10/1985', N'456 Đường Nguyễn Huệ, Hà Nội', 'tranthib@example.com', '01/02/2022', '01/08/2022', 0),
('DG003', N'Lê Minh C', 'LDG001', '10/08/1992', N'789 Đường Trần Phú, Đà Nẵng', 'leminhc@example.com', '01/03/2022', '01/09/2022', 0),
('DG004', N'Phạm Thị D', 'LDG002', '25/03/1995', N'567 Đường Nguyễn Đình Chiểu, Hồ Chí Minh', 'phamthid@example.com', '01/04/2022', '01/10/2022', 0),
('DG005', N'Ngô Đình E', 'LDG001', '12/12/1988', N'890 Đường Lý Tự Trọng, TP HCM', 'ngodinhe@example.com', '01/05/2022', '01/11/2022', 0),
('DG006', N'Lương Văn F', 'LDG001', '08/07/1993', N'234 Đường Lê Lai, Hà Nội', 'luongvanf@example.com', '01/06/2022', '01/12/2022', 0),
('DG007', N'Hồ Thị G', 'LDG002', '18/09/1982', N'678 Đường Nguyễn Thị Minh Khai, Đà Nẵng', 'hothig@example.com', '01/07/2022', '01/01/2023', 0),
('DG008', N'Vũ Minh H', 'LDG001', '05/11/1991', N'432 Đường Lê Lợi, TP HCM', 'vuminhh@example.com', '01/08/2022', '01/02/2023', 0),
('DG009', N'Đặng Thị I', 'LDG002', '30/04/1987', N'345 Đường Hùng Vương, Hà Nội', 'dangthii@example.com', '01/09/2022', '01/03/2023', 0),
('DG010', N'Phan Văn K', 'LDG001', '17/01/1990', N'567 Đường Trần Hưng Đạo, Hà Nội', 'phanvank@example.com', '01/10/2022', '01/04/2023', 0);

INSERT INTO THELOAI (MaTheLoai, TenTheLoai) VALUES
('TL001', N'Tiểu thuyết'),
('TL002', N'Khoa học');
INSERT INTO THELOAI (MaTheLoai, TenTheLoai) VALUES
('TL003', N'Tâm lý'),
('TL004', N'Kinh tế'),
('TL005', N'Lịch sử'),
('TL006', N'Thơ ca'),
('TL007', N'Trinh thám');


INSERT INTO DAUSACH (MaDauSach, TenDauSach, MaTheLoai) VALUES
('DS001', N'Tiểu thuyết 1', 'TL001'),
('DS002', N'Khoa học 1', 'TL002');
INSERT INTO DAUSACH (MaDauSach, TenDauSach, MaTheLoai) VALUES
('DS003', N'Tâm lý 1', 'TL003'),
('DS004', N'Kinh tế 1', 'TL004'),
('DS005', N'Lịch sử 1', 'TL005'),
('DS006', N'Thơ ca 1', 'TL006'),
('DS007', N'Trinh thám 1', 'TL007');
select * from DOCGIA

INSERT INTO SACH (MaSach, TenSach, MaDauSach, NamXuatBan, NhaXuatBan) VALUES
('S001', N'Sách 1', 'DS001', 2010, N'NXB A'),
('S002', N'Sách 2', 'DS002', 2015, N'NXB B'),
('S003', N'Sách 3', 'DS003', 2020, N'NXB C'),
('S004', N'Sách 4', 'DS004', 2018, N'NXB D'),
('S005', N'Sách 5', 'DS005', 2016, N'NXB E'),
('S006', N'Sách 6', 'DS006', 2019, N'NXB F'),
('S007', N'Sách 7', 'DS007', 2017, N'NXB G'),
('S008', N'Sách 8', 'DS003', 2021, N'NXB H'),
('S009', N'Sách 9', 'DS004', 2022, N'NXB I'),
('S010', N'Sách 10', 'DS005', 2014, N'NXB J'),
('S011', N'Sách 11', 'DS006', 2013, N'NXB K'),
('S012', N'Sách 12', 'DS007', 2011, N'NXB L');


INSERT INTO PHIEUNHAPSACH (MaPhieuNhapSach, NgayNhap, TongTien) VALUES
('PNS001', '10/01/2023', 200000),
('PNS002', '15/02/2023', 350000);
INSERT INTO PHIEUNHAPSACH (MaPhieuNhapSach, NgayNhap, TongTien) VALUES
('PNS003', '20/03/2023', 280000),
('PNS004', '25/04/2023', 400000),
('PNS005', '30/05/2023', 220000),
('PNS006', '05/06/2023', 180000),
('PNS007', '10/07/2023', 310000),
('PNS008', '15/08/2023', 270000),
('PNS009', '20/09/2023', 380000),
('PNS010', '25/10/2023', 430000),
('PNS011', '30/11/2023', 210000),
('PNS012', '05/12/2023', 330000);


INSERT INTO CT_PHIEUNHAPSACH (MaCTPhieuNhapSach, MaPhieuNhapSach, MaSach, SoLuong, DonGia) VALUES
('CTPNS001', 'PNS001', 'S001', 5, 10000),
('CTPNS002', 'PNS002', 'S002', 3, 15000);
INSERT INTO CT_PHIEUNHAPSACH (MaCTPhieuNhapSach, MaPhieuNhapSach, MaSach, SoLuong, DonGia) VALUES
('CTPNS003', 'PNS003', 'S003', 4, 12000),
('CTPNS004', 'PNS004', 'S004', 6, 14000),
('CTPNS005', 'PNS005', 'S005', 7, 11000),
('CTPNS006', 'PNS006', 'S006', 8, 9000),
('CTPNS007', 'PNS007', 'S007', 3, 13000),
('CTPNS008', 'PNS008', 'S008', 5, 10000),
('CTPNS009', 'PNS009', 'S009', 4, 15000),
('CTPNS010', 'PNS010', 'S010', 6, 12000),
('CTPNS011', 'PNS011', 'S011', 7, 14000),
('CTPNS012', 'PNS012', 'S012', 8, 10000);


INSERT INTO CUONSACH (MaCuonSach, MaSach, TinhTrang) VALUES
('CS001', 'S001', 1),
('CS002', 'S001', 0);
INSERT INTO CUONSACH (MaCuonSach, MaSach, TinhTrang) VALUES
('CS003', 'S002', 1),
('CS004', 'S002', 0),
('CS005', 'S003', 1),
('CS006', 'S003', 0),
('CS007', 'S004', 1),
('CS008', 'S004', 0),
('CS009', 'S005', 1),
('CS010', 'S005', 0),
('CS011', 'S006', 1),
('CS012', 'S006', 0);


INSERT INTO TACGIA (MaTacGia, TenTacGia) VALUES
('TG001', N'Tác giả 1'),
('TG002', N'Tác giả 2'),
('TG003', N'Tác giả 3'),
('TG004', N'Tác giả 4'),
('TG005', N'Tác giả 5'),
('TG006', N'Tác giả 6'),
('TG007', N'Tác giả 7'),
('TG008', N'Tác giả 8'),
('TG009', N'Tác giả 9'),
('TG010', N'Tác giả 10'),
('TG011', N'Tác giả 11'),
('TG012', N'Tác giả 12');



INSERT INTO CT_TACGIA (MaDauSach, MaTacGia) VALUES
('DS001', 'TG001'),
('DS002', 'TG002');
INSERT INTO CT_TACGIA (MaDauSach, MaTacGia) VALUES
('DS003', 'TG003'),
('DS004', 'TG004'),
('DS005', 'TG005'),
('DS006', 'TG006'),
('DS007', 'TG007');


-- B?ng PHIEUMUONTRASACH
--INSERT INTO PHIEUMUONTRASACH (MaPhieuMuonTraSach, MaDocGia, NgayMuon, MaCuonSach, NgayPhaiTra, NgayTra, TienPhatKyNay, SoTienTra, ConLai) VALUES
--('PMTS001', 'DG001', '05/01/2023', 'CS001', '20/01/2023', '20/01/2023', 0.00, 0.00, 0.00),
--('PMTS002', 'DG002', '10/02/2023', 'CS002', '25/02/2023', '27/02/2023', 20.00, 20.00, 0.00),
--('PMTS003', 'DG003', '15/03/2023', 'CS003', '30/03/2023', '30/03/2023', 0.00, 0.00, 0.00),
--('PMTS004', 'DG004', '20/04/2023', 'CS004', '05/05/2023', NULL, 0.00, 0.00, 0.00),
--('PMTS005', 'DG005', '25/05/2023 08:00:00', 'CS005', '09/06/2023 08:00:00', NULL, 0.00, 0.00, 0.00),
--'PMTS006', 'DG006', '30/06/2023 08:00:00', 'CS006', '15/07/2023 08:00:00', NULL, 0.00, 0.00, 0.00),
---('PMTS007', 'DG007', '05/07/2023 08:00:00', 'CS007', '20/07/2023 08:00:00', NULL, 0.00, 0.00, 0.00),
--('PMTS008', 'DG008', '10/08/2023 08:00:00', 'CS008', '25/08/2023 08:00:00', NULL, 0.00, 0.00, 0.00),
--('PMTS009', 'DG009', '15/09/2023 08:00:00', 'CS009', '30/09/2023 08:00:00', NULL, 0.00, 0.00, 0.00),
--('PMTS010', 'DG010', '20/10/2023 08:00:00', 'CS010', '04/11/2023 08:00:00', NULL, 0.00, 0.00, 0.00),
--('PMTS011', 'DG011', '25/11/2023 08:00:00', 'CS011', '10/12/2023 08:00:00', NULL, 0.00, 0.00, 0.00),
--('PMTS012', 'DG012', '30/12/2023 08:00:00', 'CS012', '14/01/2024 08:00:00', NULL, 0.00, 0.00, 0.00);

-- B?ng PHIEUTHUTIENPHAT
INSERT INTO PHIEUTHUTIENPHAT (MaPhieuThuTien, MaDocGia, NgayThu, SoTienThu, ConLai) VALUES
('PTTP001', 'DG001', '2023-01-25', 0, 0),
('PTTP002', 'DG002', '2023-02-28', 20000, 0),
('PTTP003', 'DG003', '2023-03-30', 0, 0),
('PTTP004', 'DG004', '2023-04-30', 0, 0),
('PTTP005', 'DG005', '2023-05-31', 0, 0),
('PTTP006', 'DG006', '2023-06-30', 0, 0),
('PTTP007', 'DG007', '2023-07-31', 0, 0),
('PTTP008', 'DG008', '2023-08-31', 0, 0),
('PTTP009', 'DG009', '2023-09-30', 0, 0),
('PTTP010', 'DG010', '2023-10-31', 0, 0);

-- B?ng BAOCAOTRATRE
INSERT INTO BAOCAOTRATRE (MaBaoCaoTraTre, NgayLapBaoCao, MaCuonSach, NgayMuon, SoNgayTraTre) VALUES
('BCTT001', '15/01/2023', 'CS001', '01/01/2023', 5),
('BCTT002', '20/02/2023', 'CS002', '10/02/2023', 2),
('BCTT003', '25/03/2023', 'CS003', '15/03/2023', 3),
('BCTT004', '30/04/2023', 'CS004', '20/04/2023', 4),
('BCTT005', '05/05/2023', 'CS005', '25/05/2023', 1),
('BCTT006', '10/06/2023', 'CS006', '30/06/2023', 0),
('BCTT007', '15/07/2023', 'CS007', '05/07/2023', 10),
('BCTT008', '20/08/2023', 'CS008', '10/08/2023', 7),
('BCTT009', '25/09/2023', 'CS009', '15/09/2023', 2),
('BCTT010', '30/10/2023', 'CS010', '20/10/2023', 3),
('BCTT011', '05/11/2023', 'CS011', '25/11/2023', 0),
('BCTT012', '10/12/2023', 'CS012', '30/12/2023', 1);

-- B?ng BAOCAOMUONSACH
INSERT INTO BAOCAOMUONSACH (MaBaoCaoMuonSach, Thang, Nam, TongSoLuotMuon) VALUES
('BCMS001', 1,2023, 50),
('BCMS002', 2,2023, 70),
('BCMS003', 3,2023, 60),
('BCMS004', 4,2023, 80),
('BCMS005', 5,2023, 90),
('BCMS006', 6,2023, 100),
('BCMS007', 7,2023, 110),
('BCMS008', 8,2023, 120),
('BCMS009', 9,2023, 130),
('BCMS010', 10,2023, 140),
('BCMS011', 11,2023, 150),
('BCMS012', 12,2023, 160);


INSERT INTO CT_BCMUONSACH (MaBaoCaoMuonSach, MaTheLoai, SoLuotMuon, TiLe) VALUES
('BCMS001', 'TL001', 30, 0.6),
('BCMS001', 'TL002', 20, 0.4);
INSERT INTO CT_BCMUONSACH (MaBaoCaoMuonSach, MaTheLoai, SoLuotMuon, TiLe) VALUES
('BCMS002', 'TL003', 42, 0.6),
('BCMS002', 'TL004', 28, 0.4),
('BCMS003', 'TL005', 45, 0.75),
('BCMS003', 'TL006', 15, 0.25),
('BCMS004', 'TL007', 80, 1)

UPDATE PHIEUNHAPSACH
SET TongTien = 0