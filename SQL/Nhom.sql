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

drop database QLSVNhom
go

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
WHERE TENDN = @TENDN AND MATKHAU = @MK
END;
GO

EXEC SP_INS_SINHVIEN 'SV01', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', 'CNTT-K35', 'NVA', '123456'
EXEC SP_INS_SINHVIEN 'SV0432', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', '19MMT', 'asv23', 'a'

EXEC SP_LOGIN_SV 'asv', 'a'
GO

select * from SINHVIEN
go

/*
Yeu cau d
*/

--INSERT INTO NHANVIEN (MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY)
--VALUES ('NV01', 'NGUYEN VAN A', 'nva@yahoo.com', 3000000, 'NVA', 123456, 'NV01');

CREATE PROCEDURE ViewClass @MANV varchar(20)
AS
SELECT MALOP, TENLOP FROM LOP
WHERE MANV = @MANV
GO

EXEC ViewClass @MANV = 'a'
go

-- BUG: XET THEM DIEU KIEN LOP THUOC QUYEN CUA NV HAY KHONG?
-- SOLVED
CREATE PROCEDURE UpdateClass @MALOP varchar(20), @TENLOP nvarchar(100), @MANV varchar(20)
AS
begin
	UPDATE LOP
	SET TENLOP = @TENLOP
	WHERE @MALOP = MALOP and @MANV = MANV
end;
GO

EXEC UpdateClass @MALOP = '19MMT', @TENLOP = 'BM CSDL3', @MANV = 'a'
go

CREATE PROCEDURE AddClass @MALOP varchar(20), @TENLOP nvarchar(100), @MANV varchar(20) = NULL
AS
begin
	INSERT INTO LOP (MALOP, TENLOP, MANV)
	VALUES
		(@MALOP, @TENLOP, @MANV)
end;
GO

EXEC AddClass @MALOP = '19MMT', @TENLOP = 'Bao mat CSDL', @MANV = 'a'
go

CREATE PROCEDURE DeleteClass @MANV varchar(20), @MALOP varchar(20)
AS
begin
	DELETE FROM LOP
	WHERE MALOP = @MALOP AND MANV = @MANV
end;
GO

--EXEC DeleteClass @MANV = 'a', @MALOP = '19MMT'
go


CREATE PROCEDURE ViewSV @MALOP varchar(20)
AS
begin
	SELECT MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN
	FROM SINHVIEN
	WHERE MALOP = @MALOP
end;
GO

EXEC ViewSV @MALOP = '19MMT'
go

-- BUG
-- solved
CREATE PROCEDURE UpdateSV @MANV varchar(20),
						  @MASV nvarchar(20), 
						  @HOTEN nvarchar(100), 
						  @NGAYSINH datetime, 
						  @DIACHI nvarchar(200), 
						  @MALOP varchar(20), 
						  @TENDN nvarchar(100), 
						  @MATKHAU varchar(100)
AS
BEGIN
	Declare @MK VARBINARY(MAX)
	SELECT @MK = HASHBYTES('MD5', @MATKHAU)

	UPDATE SINHVIEN
	SET HOTEN = @HOTEN, NGAYSINH = @NGAYSINH, DIACHI = @DIACHI, TENDN = @TENDN, MATKHAU = @MK
	FROM SINHVIEN SV, LOP
	WHERE SV.MASV = @MASV and LOP.MANV = @MANV and LOP.MALOP = @MALOP
END;
GO

EXEC UpdateSV 'NV01', 'SV0432', 'Huynh Van C', '1-1-2001', 'TRAN HUNG DAO', '19MMT', 'HVC', 'ABC'
go

-- XET THEM DIEU KIEN LOP THUOC QUYEN CUA NV HAY KHONG? 
-- solved
CREATE PROCEDURE AddSV (@MASV NVARCHAR(20),
						@HOTEN NVARCHAR(100),
						@NGAYSINH DATETIME,
						@DIACHI NVARCHAR(200),
						@MALOP NVARCHAR(20),
						@TENDN NVARCHAR(100),
						@MATKHAU VARCHAR(100),
						@MANV VARCHAR(20))
AS
BEGIN
	if exists
	(
		select * from LOP 
		where MALOP = @MALOP and MANV = @MANV
	)
    INSERT INTO SINHVIEN (MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN, MATKHAU)
    VALUES
    (
        @MASV, @HOTEN, @NGAYSINH, @DIACHI, @MALOP, @TENDN, HASHBYTES('MD5', @MATKHAU)
    )
END;
GO

EXEC AddSV @MASV = 'SV04', @HOTEN = 'Huynh Van C', @NGAYSINH = '1-1-2001', @DIACHI = '123 TRAN HUNG DAO', @MALOP = '19MMT', @TENDN = 'svtes2t', @MATKHAU = 'a', @MANV = 'a'
go

-- BUG THIEU LOP HOC? (DELETE SINH VIEN TRONG BANG SINHVIEN THUOC LOP QUY DINH)
-- solved
CREATE PROCEDURE DeleteSV @MANV varchar(20), 
						  @MASV nvarchar(20), 
						  @MALOP varchar(20)
AS
begin
	DELETE SV
	FROM SINHVIEN SV, LOP
	WHERE SV.MASV = @MASV AND MANV = @MANV and SV.MALOP = @MALOP
end;
GO

-- UNENCRYPTED POINT?
CREATE PROCEDURE AddPoints @MASV varchar(20) = NULL, 
						   @MAHP varchar(20) = NULL, 
						   @DIEMTHI varbinary = NULL
AS
begin
	INSERT INTO BANGDIEM (MASV, MAHP, DIEMTHI)
	VALUES (@MASV, @MAHP, @DIEMTHI)
end;
GO

EXEC AddPoints @MASV = '19127001', @MAHP = 'CSC10001', @DIEMTHI = 8 
go

-- Uncompleted (DEPEND ON DECRYPT BY NV's KEY)
CREATE PROCEDURE ViewPoints @MASV varchar(20) = NULL, @MAHP varchar(20) = NULL, @DIEMTHI varbinary = NULL
AS
INSERT INTO BANGDIEM (MASV, MAHP, DIEMTHI)
VALUES (@MASV, @MAHP, @DIEMTHI)
GO