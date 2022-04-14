﻿/*----------------------------------------------------------
MASV: 19127181
HO TEN: BACH MINH KHOI
LAB: 03
NGAY: 08/04/2022
----------------------------------------------------------*/

-- YEU CAU A ---------------------------------

CREATE DATABASE QLSV
GO

-- YEU CAU B ---------------------------------

USE QLSV
GO

CREATE TABLE SINHVIEN
(
        MASV NVARCHAR(20) PRIMARY KEY,
        HOTEN NVARCHAR(100) NOT NULL,
        NGAYSINH DATETIME,
        DIACHI NVARCHAR(200),
        MALOP VARCHAR (20),
        TENDN NVARCHAR(100) NOT NULL,
        MATKHAU VARBINARY(16) NOT NULL
);

CREATE TABLE NHANVIEN
(
        MANV VARCHAR (20) PRIMARY KEY,
        HOTEN NVARCHAR(100) NOT NULL,
        EMAIL VARCHAR (20),
        LUONG VARBINARY(MAX),
        TENDN NVARCHAR(100) NOT NULL,
        MATKHAU VARBINARY(20) NOT NULL,
);

CREATE TABLE LOP
(
        MALOP VARCHAR (20) PRIMARY KEY,
        TENLOP NVARCHAR(100) NOT NULL,
        MANV VARCHAR (20)
);


-- YEU CAU C ---------------------------------
-- TAO KHOA ----------------------------------
-- SYMMETRIC----------------------------------
if not exists
(
	select *
	from sys.symmetric_keys
	where symmetric_key_id = 101
)
create master key encryption by
	password = '19127181'
go

if not exists
(
	select *
	from sys.certificates
	where name = 'MyCert'
)
create certificate MyCert
	with subject = 'MyCert';
go

if not exists
(
	select *
	from sys.symmetric_keys
	where name = 'PriKey'
)
create symmetric key PriKey with
	algorithm = aes_256
	encryption by password = '19127181';
go

-- ASYMMETRIC --------------------------------
CREATE ASYMMETRIC KEY BMK
WITH ALGORITHM = RSA_2048
ENCRYPTION BY PASSWORD = 'BACHMINHKHOI';
GO
-- DECRYPT ASYM ------------------------------
DECLARE @AsymID INT;
SET @AsymID = ASYMKEY_ID('BMK');
DECLARE @MH VARBINARY(MAX);
SELECT @MH = ENCRYPTBYASYMKEY (@AsymID, '3000000')
SELECT convert(VARCHAR, DECRYPTBYASYMKEY(@AsymID, @MH, N'BACHMINHKHOI'))

-- TAO STORED PROCEDURE ----------------------
CREATE PROCEDURE SP_INS_SINHVIEN (@MASV NVARCHAR(20),
                                  @HOTEN NVARCHAR(100),
                                  @NGAYSINH DATETIME,
                                  @DIACHI NVARCHAR(200),
                                  @MALOP NVARCHAR(20),
                                  @TENDN NVARCHAR(100),
                                  @MATKHAU NVARCHAR(100))
AS
BEGIN
        INSERT INTO SINHVIEN (MASV, HOTEN, NGAYSINH, DIACHI, MALOP, TENDN, MATKHAU)
        VALUES
        (
                @MASV, @HOTEN, @NGAYSINH, @DIACHI, @MALOP, @TENDN, HASHBYTES('MD5', @MATKHAU)
        )
END;
GO

CREATE PROCEDURE SP_INS_NHANVIEN (@MANV VARCHAR (20), @HOTEN NVARCHAR(100), @EMAIL VARCHAR (20), @LUONG INT, @TENDN NVARCHAR(100), @MATKHAU VARCHAR(100))
AS
BEGIN
        OPEN SYMMETRIC KEY PRIKEY
        DECRYPTION BY PASSWORD = '19127181';

        DECLARE @MH VARBINARY(MAX)
        SELECT @MH = CONVERT(VARBINARY(MAX), ENCRYPTBYKEY(KEY_GUID('PRIKEY'), CONVERT(VARCHAR(20), @LUONG)))

        INSERT INTO NHANVIEN(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU)
        VALUES
        (
                @MANV, @HOTEN, @EMAIL, @MH, @TENDN, HASHBYTES('SHA1', @MATKHAU)
        )
        CLOSE SYMMETRIC KEY PRIKEY
END;
GO

---

CREATE PROCEDURE SP_SEL_NHANVIEN
AS
BEGIN
        OPEN SYMMETRIC KEY PriKey
        DECRYPTION BY PASSWORD = '19127181';

        SELECT MANV, HOTEN, EMAIL, CONVERT(VARCHAR, DECRYPTBYKEY(LUONG)) AS LUONGCB
        FROM NHANVIEN

        CLOSE SYMMETRIC KEY PRIKEY
END;
GO

EXEC SP_SEL_NHANVIEN
GO



CREATE PROCEDURE SP_LOGIN_SV (@TENDN NVARCHAR(100), @MATKHAU NVARCHAR(100))
AS 
BEGIN
Declare @MK VARBINARY(MAX)
SELECT @MK = HASHBYTES('MD5', @MATKHAU)
SELECT * from SINHVIEN
WHERE TENDN = @TENDN AND MATKHAU=@MK
END;
GO

/*
CREATE PROCEDURE SP_LOGIN_NV (@TENDN NVARCHAR(100), @MATKHAU NVARCHAR(100))
AS 
BEGIN
Declare @MK VARBINARY(MAX)
SELECT @MK = HASHBYTES('MD5', @MATKHAU)
SELECT * from NHANVIEN
WHERE TENDN = @TENDN AND MATKHAU=@MK
END;
GO
*/

CREATE PROCEDURE SP_LOGIN_NV (@TENDN NVARCHAR(100), @MATKHAU VARCHAR(100))
AS 
BEGIN
	Declare @MK VARBINARY(MAX)
	set @MK = HASHBYTES('SHA1', @MATKHAU)
	print @mk
	SELECT * from NHANVIEN
	WHERE TENDN = @TENDN AND MATKHAU=@MK
END;
GO

CREATE PROCEDURE SP_LOGIN_NV2 (@TENDN NVARCHAR(100), @MATKHAU NVARCHAR(100))
AS 
BEGIN
SELECT * from NHANVIEN
WHERE TENDN = @TENDN AND MATKHAU=HASHBYTES('SHA1', @MATKHAU)
END;
GO

select * from NHANVIEN
select * from SINHVIEN

EXEC SP_INS_SINHVIEN 'SV01', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', 'CNTT-K35', 'NVA', '123456'

EXEC SP_INS_SINHVIEN 'SV02', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', 'CNTT-K35', 'asv', 'a'

EXEC SP_INS_NHANVIEN 'a', 'NVA', 'NVA@', 300000, 'a', 'a'

EXEC SP_INS_NHANVIEN 'b', 'NVA', 'NVA@', 300000, 'b', 'b'


EXEC SP_LOGIN_NV 'a', 'a'
GO
EXEC SP_LOGIN_NV2 'a', 'a'
GO
select * FROM NHANVIEN


SELECT * from NHANVIEN
WHERE TENDN = 'a' AND MATKHAU=HASHBYTES('SHA1', 'a')