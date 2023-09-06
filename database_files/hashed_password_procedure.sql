DELIMITER //
DROP PROCEDURE IF EXISTS hashing_password //
CREATE PROCEDURE hashing_password (INOUT `new_password` VARCHAR(45), OUT hashed_password BINARY(32))
BEGIN
IF `new_password` = '' THEN SET `new_password` = 'password'; END IF;
SET hashed_password = UNHEX(sha2(`new_password`, 256));
SET `new_password` = NULL;
END//