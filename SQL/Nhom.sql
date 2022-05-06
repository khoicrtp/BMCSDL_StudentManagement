/*----------------------------------------------------------
MASV:
HO TEN CAC THANH VIEN NHOM:
	BACH MINH KHOI - 19127181
	LAM TRI DUC - 19127122
	TRUONG BUU Y - 19127638
LAB: 04 - NHOM 5
NGAY: 4/28/2022
----------------------------------------------------------*/

--- MAN HINH DANG NHAP
CREATE DATABASE QLSVNhom
GO

USE QLSVNhom
GO

CREATE TABLE SINHVIEN
(
	MASV NVARCHAR(20) PRIMARY KEY,
	HOTEN NVARCHAR(100) NOT NULL,
	NGAYSINH DATETIME,
	DIACHI NVARCHAR(200),
	MALOP VARCHAR (20),
	TENDN NVARCHAR(100) NOT NULL,
	MATKHAU VARCHAR(100) NOT NULL
);

CREATE TABLE NHANVIEN
(
	MANV VARCHAR(20) PRIMARY KEY,
	HOTEN NVARCHAR(100) NOT NULL,
	EMAIL VARCHAR(20),
	LUONG VARCHAR(100),
	TENDN NVARCHAR(100) NOT NULL,
	MATKHAU VARCHAR(100) NOT NULL,
	PUBKEY VARCHAR(20)
);

CREATE TABLE LOP
(
	MALOP VARCHAR(20) PRIMARY KEY,
	TENLOP NVARCHAR(100) NOT NULL,
	MANV VARCHAR(20)
);

CREATE TABLE HOCPHAN
(
	MAHP VARCHAR(20) PRIMARY KEY,
	TENHP NVARCHAR(100) NOT NULL,
	SOTC INT
);

CREATE TABLE BANGDIEM
(
	MASV VARCHAR(20),
	MAHP VARCHAR(20),
	DIEMTHI VARCHAR(10),
	CONSTRAINT PK_BANGDIEM PRIMARY KEY (MASV, MAHP)
);

go

/*Yeu cau c*/
--- i
CREATE PROCEDURE SP_INS_ENCRYPT_SINHVIEN (@MASV VARCHAR(20),
										 @HOTEN NVARCHAR(100),
										 @NGAYSINH DATETIME,
										 @DIACHI NVARCHAR(200),
										 @MALOP VARCHAR(20),
										 @TENDN NVARCHAR(100),
										 @MK VARCHAR(100))
AS
BEGIN
	INSERT INTO SINHVIEN(MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN, MATKHAU)
	VALUES
	(@MASV, @HOTEN, @NGAYSINH, @DIACHI, @MALOP, @TENDN, @MK)
END;
GO
EXEC SP_INS_ENCRYPT_SINHVIEN 'SV02', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', '19MMT', 'asv2', '0cc175b9c0f1b6a831c399e269772661'
GO
------
CREATE PROCEDURE SP_INS_ENCRYPT_NHANVIEN (@MANV VARCHAR(20),
										 @HOTEN NVARCHAR(100),
										 @EMAIL VARCHAR(20),
										 @LUONG VARCHAR(100),
										 @TENDN NVARCHAR(100),
										 @MK VARCHAR(100),
										 @PUBKEY VARCHAR(20))
AS
BEGIN
	INSERT INTO NHANVIEN(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY)
	VALUES
	(@MANV, @HOTEN, @EMAIL, @LUONG, @TENDN, @MK, @PUBKEY)
END;
GO


EXEC SP_INS_ENCRYPT_NHANVIEN 'a', 'NGUYEN VAN A', 'NVA@', '1tuXqKI768EYL4P1Yd9NQw==', 'a', 
					'0cc175b9c0f1b6a831c399e269772661', 'PUBPUB'
GO

--- ii	
--------------------------------
CREATE PROCEDURE SP_SEL_ENCRYPT_NHANVIEN
AS
BEGIN
	SELECT MANV, HOTEN, EMAIL, LUONG
	from NHANVIEN
END;
GO

EXEC SP_SEL_ENCRYPT_NHANVIEN
GO
------------------------------
DROP PROCEDURE SP_DEL_NHANVIEN
GO
CREATE PROCEDURE SP_DEL_NHANVIEN (@MANV VARCHAR(20))
AS
BEGIN
	DELETE 
	from NHANVIEN
	WHERE MANV = @MANV
END;
GO

EXEC SP_DEL_NHANVIEN @MANV ='a2'
GO


create procedure SP_SEL_LOGIN @TENDN nvarchar(100),
							  @MATKHAU VARCHAR(100)
as
begin
	select *
	from NHANVIEN
	where @TENDN = TENDN and @MATKHAU = MATKHAU
end;
go

--- MAN HINH QUAN LI NHAN VIEN

	-- thay doi thong tin nhan vien
create procedure SP_UPD_NHANVIEN @MANV varchar(20),
								 @HOTEN nvarchar(100),
								 @EMAIL varchar(20),
								 @LUONG VARCHAR(100),
								 @TENDN nvarchar(100),
								 @MATKHAU VARCHAR(100),
								 @PUBKEY varchar(20)
as
begin
	update NHANVIEN
	set HOTEN = @HOTEN,
		EMAIL = @EMAIL,
		LUONG = @LUONG,
		TENDN = @TENDN,
		MATKHAU = @MATKHAU,
		PUBKEY = @PUBKEY
	where @MANV = MANV
end;
go

SP_UPD_NHANVIEN @MANV = 'a2',
								 @HOTEN ='NVAA riu',
								 @EMAIL = 'NVA@1',
								 @LUONG = 'wRFEUEoI3b37mhmNChNXsQ==',
								 @TENDN = 'a2new',
								 @MATKHAU = '0cc175b9c0f1b6a831c399e269772661',
								 @PUBKEY = 'PUBKEY'
GO

--- MAN HINH QUAN LI LOP
	-- xem lop
create procedure SP_SEL_LOP @MANV varchar(20)
as
begin
		SELECT * FROM LOP
		WHERE MANV=@MANV
end;
go

EXEC SP_SEL_LOP 'a'
GO
	-- them lop 
create procedure SP_INS_LOP @MALOP varchar(20),
							@TENLOP nvarchar(100),
							@MANV varchar(20)
as
begin
		insert into LOP(MALOP, TENLOP, MANV)
		values
		(@MALOP, @TENLOP, @MANV)
end;
go

EXEC SP_INS_LOP @MALOP='19MMT', @TENLOP='BMCSDL', @MANV='a'
GO

	-- xoa lop hoc
create procedure SP_DEL_LOP @MALOP varchar(20)
as
begin
	delete from LOP
	where @MALOP = MALOP
end;
go

	-- thay doi thong tin lop hoc
create procedure SP_UPD_LOP @MALOP varchar(20),
							@TENLOP nvarchar(100),
							@MANV varchar(20)
as
begin
	update LOP
	set TENLOP = @TENLOP
	where @MANV = MANV
end;
go

--- MAN HINH QUAN LI SINH VIEN

	-- select danh sach sinh vien thuoc lop nhan vien quan li
create procedure SP_SEL_SINHVIEN @MALOP VARCHAR(20),
								 @MANV varchar(20)
as
begin
	SELECT SV.MASV, SV.HOTEN, SV.NGAYSINH, SV.DIACHI, SV.MALOP, SV.TENDN
	FROM SINHVIEN SV, LOP L, NHANVIEN NV
	WHERE SV.MALOP = L.MALOP AND L.MANV=NV.MANV
end;
go

EXEC SP_SEL_SINHVIEN '19MMT', 'a'
GO

	-- xoa sinh vien thuoc lop nhan vien quan li
create procedure SP_DEL_SINHVIEN @MASV varchar(20),
								 @MANV varchar(20)
as
begin
	delete SV
	from SINHVIEN SV, LOP
	where @MASV = MASV and SV.MALOP = LOP.MALOP and @MANV = LOP.MANV
end;
go

	-- thay đổi thông tin sv thuộc lớp nhân viên quản lí
create procedure SP_UPD_SINHVIEN @MASV VARCHAR(20),
								 @HOTEN NVARCHAR(100),
								 @NGAYSINH DATETIME,
								 @DIACHI NVARCHAR(200),
								 @MALOP VARCHAR(20),
								 @TENDN NVARCHAR(100),
								 @MATKHAU VARCHAR(100),
								 @MANV varchar(20)
as
begin
	update SV
	set HOTEN = @HOTEN,
		NGAYSINH = @NGAYSINH,
		DIACHI = @DIACHI,
		MALOP = @MALOP,
		TENDN = @TENDN,
		MATKHAU = @MATKHAU
	from SINHVIEN SV, LOP
	where @MASV = SV.MASV and SV.MALOP = LOP.MALOP and @MALOP = LOP.MALOP and @MANV = LOP.MANV
end;
go

EXEC SP_UPD_SINHVIEN 'a5','a5new','07/07/2001','NVC','19MMT','sv5','0cc175b9c0f1b6a831c399e269772661','a'
GO

	-- nhập bảng điểm của từng sinh viên, cột điểm thi được mã hóa bằng public key của nhân viên (đã đăng nhập)
create procedure SP_INS_DIEM_SINHVIEN @MASV varchar(20),
									  @MAHP VARCHAR (20),
									  @DIEMTHI VARBINARY(max),
									  @MANV varchar(20)
as
begin
	if exists
	(
		select * 
		from SINHVIEN SV, LOP
		where @MASV = SV.MASV and SV.MALOP = LOP.MALOP and @MANV = LOP.MANV
	)
		insert into BANGDIEM(MASV, MAHP, DIEMTHI)
		values
		(@MASV, @MAHP, @DIEMTHI)
	else
		Print 'Error'
end;
go

	-- update bang diem cua sinh vien
create procedure SP_UPD_DIEM_SINHVIEN @MASV varchar(20),
									  @MAHP VARCHAR(20),
									  @DIEMTHI VARBINARY(max),
									  @MANV varchar(20)
as
begin
	update BD
	set BD.DIEMTHI = @DIEMTHI
	from SINHVIEN SV, LOP, BANGDIEM BD
	where @MAHP = BD.MAHP and @MASV = BD.MASV and @MASV = SV.MASV and SV.MALOP = LOP.MALOP and @MANV = LOP.MANV
end;
go

	-- xoa bang diem cua sinh vien
create procedure SP_DEL_DIEM_SINHVIEN @MASV varchar(20),
									  @MAHP VARCHAR(20),
									  @MANV varchar(20)
as
begin
	delete BD 
	from BANGDIEM BD, SINHVIEN SV, LOP
	where @MASV = BD.MASV and @MAHP = BD.MAHP and @MASV = SV.MASV and SV.MALOP = LOP.MALOP and @MANV = LOP.MANV
end;
go