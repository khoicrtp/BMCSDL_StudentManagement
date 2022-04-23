/*----------------------------------------------------------
MASV:
HO TEN CAC THANH VIEN NHOM:
	BACH MINH KHOI - 19127181
	LAM TRI DUC - 19127122
	TRUONG BUU Y - 19127638
LAB: 03 - NHOM 5
NGAY: 4/8/2022
----------------------------------------------------------*/

/*
Yeu cau a
*/

create database QLSVNhom
go

use QlSVNhom
go

/*
Yeu cau b
*/

create table SINHVIEN
(
	MASV NVARCHAR(20) primary key,
	HOTEN NVARCHAR(100) not null,
	NGAYSINH DATETIME,
	DIACHI NVARCHAR(200),
	MALOP VARCHAR (20),
	TENDN NVARCHAR(100) not null,
	MATKHAU VARBINARY(16) not null
);

create table NHANVIEN
(
	MANV VARCHAR(20) primary key,
	HOTEN NVARCHAR(100) not null,
	EMAIL VARCHAR(20),
	LUONG VARBINARY(max),
	TENDN NVARCHAR(100) not null,
	MATKHAU VARBINARY(20) not null,
	PUBKEY VARCHAR(20)
);

create table LOP
(
	MALOP VARCHAR(20) primary key,
	TENLOP NVARCHAR(100) not null,
	MANV VARCHAR(20)
);

create table HOCPHAN
(
	MAHP VARCHAR(20) primary key,
	TENHP NVARCHAR(100) not null,
	SOTC INT
);

create table BANGDIEM
(
	MASV VARCHAR(20),
	MAHP VARCHAR(20),
	DIEMTHI VARBINARY(max),
	constraint PK_BANGDIEM primary key (MASV, MAHP)
);

go

/*
Yeu cau c
*/

--- i
/*
--- b1
if not exists
(
	select *
	from sys.asymmetric_keys
	where asymmetric_key_id = 101
)
create master key encryption by
	password = '19127638'
go

--- b2
if not exists
(
	select *
	from sys.certificates
	where name = 'MyCert'
)
create certificate MyCert
	with subject = 'MyCert';
go

--- b3
if not exists
(
	select *
	from sys.asymmetric_keys
	where name = 'PriKey'
)
create asymmetric key PriKey with
	algorithm = rsa_2048
	encryption by password = 'test'
go

--- b4
declare @AsymID int;
set @AsymID = ASYMKEY_ID('PriKey');
select ENCRYPTBYASYMKEY(@AsymID, '3000000')
go

--- b5
declare @AsymID int;
set @AsymID = ASYMKEY_ID('PriKey');
declare @MH varbinary(max)
select @MH = ENCRYPTBYASYMKEY(@AsymID, '3000000')
select convert(varchar, decryptbyasymkey(@AsymID, @MH, N'test'))
go
*/
---

create procedure SP_INS_PUBLIC_NHANVIEN (@MANV VARCHAR(20),
										 @HOTEN NVARCHAR(100),
										 @EMAIL VARCHAR(20),
										 @LUONGCB INT,
										 @TENDN NVARCHAR(100),
										 @MK VARCHAR(100))
as
begin
	declare @H_MK nvarchar(max)
	set @H_MK = convert(nvarchar, hashbytes('SHA1', @MK))
	if not exists
	(
		select *
		from sys.asymmetric_keys
		where name = @MANV
	)
	exec(
	'create asymmetric key ' + @MANV + ' with
		algorithm = rsa_2048
		encryption by password = ''' + @H_MK + ''''
	)

	declare @AsymID int;
	set @AsymID = ASYMKEY_ID(@MANV);
	declare @MH varbinary(max)
	select @MH = ENCRYPTBYASYMKEY(@AsymID, convert(varchar, @LUONGCB))

	insert into NHANVIEN(MANV, 
						 HOTEN, 
						 EMAIL, 
						 LUONG, 
						 TENDN, 
						 MATKHAU,
						 PUBKEY)
	values
	(
		@MANV, 
		@HOTEN, 
		@EMAIL, 
		@MH, 
		@TENDN, 
		hashbytes('SHA1', @MK),
		@MANV
	)
end;
go

EXEC SP_INS_PUBLIC_NHANVIEN 'a', 'NGUYEN VAN A', 'NVA@', 3000000, 'a', 'a'
go

declare @TENDN nvarchar(100);
set @TENDN = 'NV01';
declare @AsymID int;
set @AsymID = ASYMKEY_ID(@TENDN);
declare @MK varchar(100);
set @MK = 'abcd12';
select MANV, TENDN, convert(varchar, decryptbyasymkey(@AsymID, LUONG, convert(nvarchar, hashbytes('SHA1', @MK)))) as LUONGCB
from NHANVIEN
go

--- ii	
create procedure SP_SEL_PUBLIC_NHANVIEN (@TENDN NVARCHAR(100),
										 @MK VARCHAR(100))
as
begin
	declare @H_MK varbinary(max);
	set @H_MK = HASHBYTES('SHA1', @MK);

	select MANV, HOTEN, EMAIL, convert(varchar, decryptbyasymkey(ASYMKEY_ID(PUBKEY), LUONG, convert(nvarchar, HASHBYTES('SHA1', @MK)))) as LUONGCB
	from NHANVIEN
	where @TENDN = TENDN and @H_MK = MATKHAU
end;
go

EXEC SP_SEL_PUBLIC_NHANVIEN 'a', 'a'
go


-- LOGIN SV
CREATE PROCEDURE SP_INS_SINHVIEN (@MASV NVARCHAR(20),
                                  @HOTEN NVARCHAR(100),
                                  @NGAYSINH DATETIME,
                                  @DIACHI NVARCHAR(200),
                                  @MALOP NVARCHAR(20),
                                  @TENDN NVARCHAR(100),
                                  @MATKHAU VARCHAR(100))
AS
BEGIN
        INSERT INTO SINHVIEN (MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN, MATKHAU)
        VALUES
        (
                @MASV, @HOTEN, @NGAYSINH, @DIACHI, @MALOP, @TENDN, HASHBYTES('MD5', @MATKHAU)
        )
END;
GO



CREATE PROCEDURE SP_LOGIN_SV (@TENDN NVARCHAR(100), @MATKHAU VARCHAR(100))
AS 
BEGIN
Declare @MK VARBINARY(MAX)
SELECT @MK = HASHBYTES('MD5', @MATKHAU)
SELECT * from SINHVIEN
WHERE TENDN = @TENDN AND MATKHAU=@MK
END;
GO

EXEC SP_INS_SINHVIEN 'SV01', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', 'CNTT-K35', 'NVA', '123456'
GO
EXEC SP_INS_SINHVIEN 'SV02', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', '19MMT', 'asv', 'a'
GO

EXEC SP_LOGIN_SV 'asv', 'a'
GO

/*
Yeu cau d
*/

-- BUG: XET THEM DIEU KIEN LOP THUOC QUYEN CUA NV HAY KHONG?

CREATE PROCEDURE AddClass @MALOP varchar(20) = NULL, @TENLOP nvarchar(100) = NULL, @MANV varchar(20) = NULL
AS
INSERT INTO LOP (MALOP, TENLOP, MANV)
VALUES (@MALOP, @TENLOP, @MANV)
GO
EXEC AddClass @MALOP = '19MMT', @TENLOP = 'Bao mat CSDL3', @MANV = 'a'
GO

CREATE PROCEDURE ViewClass @MANV varchar(20)
AS
SELECT MALOP, TENLOP FROM LOP
WHERE MANV = @MANV
GO
EXEC ViewClass @MANV = 'a'
GO

-- BUG: XET THEM DIEU KIEN LOP THUOC QUYEN CUA NV HAY KHONG?

CREATE PROCEDURE UpdateClass @MALOP varchar(20) = NULL, @TENLOP nvarchar(100) = NULL, @MANV varchar(20) = NULL
AS
UPDATE LOP
SET TENLOP = ISNULL(@TENLOP, TENLOP), MANV = ISNULL(@MANV, MANV)
WHERE MALOP = ISNULL(@MALOP, MALOP)
GO
EXEC UpdateClass @MALOP = '19MMT', @TENLOP = 'BM CSDL2', @MANV = 'a'
GO

CREATE PROCEDURE DeleteClass @MANV varchar(20), @MALOP varchar(20)
AS
DELETE FROM LOP
WHERE MALOP = @MALOP AND MANV = @MANV
GO
EXEC DeleteClass @MANV = 'a', @MALOP = '19MMT'
GO


-- XET THEM DIEU KIEN LOP THUOC QUYEN CUA NV HAY KHONG? 

CREATE PROCEDURE AddSV (@MASV NVARCHAR(20),
                                  @HOTEN NVARCHAR(100),
                                  @NGAYSINH DATETIME,
                                  @DIACHI NVARCHAR(200),
                                  @MALOP NVARCHAR(20),
                                  @TENDN NVARCHAR(100),
                                  @MATKHAU VARCHAR(100))
								  --@MANV VARCHAR(20)
AS
BEGIN
        INSERT INTO SINHVIEN (MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN, MATKHAU)
        VALUES
        (
                @MASV, @HOTEN, @NGAYSINH, @DIACHI, @MALOP, @TENDN, HASHBYTES('MD5', @MATKHAU)
        )
END;
GO
EXEC AddSV @MASV = '1912700', @HOTEN = 'Huynh Van C', @NGAYSINH = '1-1-2001', @DIACHI = '123 TRAN HUNG DAO', @MALOP = '19MMT', @TENDN = 'svtes2t', @MATKHAU = 'a'
GO

CREATE PROCEDURE ViewSV @MALOP varchar(20)
AS
SELECT MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN
FROM SINHVIEN
WHERE MALOP = @MALOP
GO
EXEC ViewSV @MALOP = '19MMT'
GO

-- BUG 

CREATE PROCEDURE UpdateSV @MANV varchar(20) = NULL, @MASV nvarchar(20) = NULL, @HOTEN nvarchar(100) = NULL, @NGAYSINH datetime = NULL, @DIACHI nvarchar(200) = NULL, @MALOP varchar(20) = NULL, @TENDN nvarchar(100) = NULL, @MATKHAU varbinary = NULL
AS
BEGIN
UPDATE SINHVIEN
SET HOTEN = ISNULL(@HOTEN, HOTEN), NGAYSINH = ISNULL(@NGAYSINH, NGAYSINH), DIACHI = ISNULL(@DIACHI, DIACHI), MALOP = ISNULL(@MALOP, MALOP), TENDN = ISNULL(@TENDN, TENDN), MATKHAU = ISNULL(@MATKHAU, MATKHAU)
FROM SINHVIEN SV
INNER JOIN LOP L
ON SV.MALOP = L.MALOP
WHERE SV.MASV = ISNULL(@MASV, MASV) AND L.MANV = ISNULL(@MANV, MANV)
END
GO
EXEC UpdateSV @MASV = '19127001', @HOTEN = 'Huynh Van C', @NGAYSINH = '1-1-2001', @DIACHI = 'TRAN HUNG DAO', @MALOP = '19MMT', @TENDN = 'HVC', @MATKHAU = 'ABC'
GO


-- BUG THIEU LOP HOC? (DELETE SINH VIEN TRONG BANG SINHVIEN THUOC LOP QUY DINH)

CREATE PROCEDURE DeleteSV @MANV varchar(20), @MASV nvarchar(20) --@MALOP?
AS
DELETE SV FROM SINHVIEN SV
INNER JOIN LOP L
ON SV.MALOP = L.MALOP
WHERE SV.MASV = @MASV AND L.MANV = @MANV
GO
EXEC DeleteSV @MANV = 'a', @MASV = '19127003'
GO


-- UNENCRYPTED POINT?

CREATE PROCEDURE AddPoints @MASV varchar(20) = NULL, @MAHP varchar(20) = NULL, @DIEMTHI varbinary = NULL
AS
INSERT INTO BANGDIEM (MASV, MAHP, DIEMTHI)
VALUES (@MASV, @MAHP, @DIEMTHI)
GO
EXEC AddPoints @MASV = '19127001', @MAHP = 'CSC10001', @DIEMTHI = 8 
GO

-- Uncompleted (DEPEND ON DECRYPT BY NV's KEY)

CREATE PROCEDURE ViewPoints @MASV varchar(20) = NULL, @MAHP varchar(20) = NULL, @DIEMTHI varbinary = NULL
AS
INSERT INTO BANGDIEM (MASV, MAHP, DIEMTHI)
VALUES (@MASV, @MAHP, @DIEMTHI)
GO
