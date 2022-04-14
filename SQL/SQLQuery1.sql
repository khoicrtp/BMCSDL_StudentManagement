use QLSV
go

EXEC SP_INS_SINHVIEN 'SV01', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', 'CNTT-K35', 'NVA', '123456'
EXEC SP_INS_SINHVIEN 'SV02', 'NGUYEN VAN A', '1/1/1990', '280 AN DUONG VUONG', 'CNTT-K35', 'a', 'a'
EXEC SP_INS_NHANVIEN 'NV01', 'NGUYEN VAN A', 'NVA@', 3000000, 'NVA', 'abcd12'
BEGIN
EXEC SP_SEL_NHANVIEN;
END;
GO
select * from NHANVIEN
select * from SINHVIEN
select* from LOP
Select * from NHANVIEN where TENDN='a'
Select * from SINHVIEN where TENDN='a'
/*
if NOT EXISTS(select * from sys.symmetric_keys)
create master key encryption by password = '19127181'

if NOT EXISTS(select * from sys.certificates)
create certificate bachkhoi with subject='bachkhoi'

IF NOT EXISTS(
  SELECT *
  FROM sys.symmetric_keys
  WHERE name = 'PriKey'
)
CREATE SYMMETRIC KEY PriKey
  --WITH ALGORITHM  TRIPLE_DES
   WITH ALGORITHM = AES_256 -- SQL 2008
  ENCRYPTION BY CERTIFICATE bachkhoi;
GO
*/

open symmetric key PriKey
decryption by password = '19127181';

select convert(varchar, decryptbykey(encryptbykey(KEY_GUID('PriKey'), convert(varchar(20), (SELECT MATKHAU FROM SINHVIEN WHERE TENDN='a')))))


--- b1
if not exists
(
	select *
	from sys.symmetric_keys
	where symmetric_key_id = 101
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
	from sys.symmetric_keys
	where name = 'PriKey'
)
create symmetric key PriKey with
	algorithm = aes_256
	encryption by password = '19127638';
go