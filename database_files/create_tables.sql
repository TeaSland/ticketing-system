-- -----------------------------------------------------
-- Table `student2021130434`.`users`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `student2021130434`.`users` ;

CREATE TABLE IF NOT EXISTS `student2021130434`.`users` (
  `id_user` INT NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(15) NOT NULL,
  `new_password` VARCHAR(45) NULL,
  `hashed_password` BINARY(32) NULL,
  `occupation` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id_user`),
  UNIQUE INDEX `idusers_UNIQUE` (`id_user` ASC) ,
  UNIQUE INDEX `username_UNIQUE` (`username` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `student2021130434`.`details`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `student2021130434`.`details` ;

CREATE TABLE IF NOT EXISTS `student2021130434`.`details` (
  `id_detail` INT NOT NULL AUTO_INCREMENT,
  `category` VARCHAR(45) NOT NULL,
  `summary` VARCHAR(200) NOT NULL,
  `description` VARCHAR(750) NOT NULL,
  UNIQUE INDEX `iddetails_UNIQUE` (`id_detail` ASC) ,
  PRIMARY KEY (`id_detail`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `student2021130434`.`tickets`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `student2021130434`.`tickets` ;

CREATE TABLE IF NOT EXISTS `student2021130434`.`tickets` (
  `id_ticket` INT NOT NULL AUTO_INCREMENT,
  `id_user` INT NOT NULL,
  `id_detail` INT NOT NULL,
  `assigned` VARCHAR(45) NOT NULL DEFAULT 'unassigned',
  PRIMARY KEY (`id_ticket`),
  UNIQUE INDEX `idticket_UNIQUE` (`id_ticket` ASC) ,
  UNIQUE INDEX `iddetail_UNIQUE` (`id_detail` ASC) ,
  INDEX `user_fk_idx` (`id_user` ASC) ,
  CONSTRAINT `fk_detail`
    FOREIGN KEY (`id_detail`)
    REFERENCES `student2021130434`.`details` (`id_detail`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_user`
    FOREIGN KEY (`id_user`)
    REFERENCES `student2021130434`.`users` (`id_user`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_bin;

