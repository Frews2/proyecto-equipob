-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema Accounts
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Accounts
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Accounts` DEFAULT CHARACTER SET utf8 ;
USE `Accounts` ;

-- -----------------------------------------------------
-- Table `Accounts`.`Status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Accounts`.`Status` (
  `StatusID` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`StatusID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Accounts`.`Account`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Accounts`.`Account` (
  `AccountID` VARCHAR(255) NOT NULL,
  `Username` VARCHAR(200) NOT NULL,
  `Email` VARCHAR(200) NOT NULL,
  `IsUser` TINYINT NOT NULL,
  `StatusID` INT NOT NULL,
  PRIMARY KEY (`AccountID`),
  UNIQUE INDEX `email_UNIQUE` (`Email` ASC) VISIBLE,
  INDEX `fk_Account_Status1_idx` (`StatusID` ASC) VISIBLE,
  CONSTRAINT `fk_Account_Status1`
    FOREIGN KEY (`StatusID`)
    REFERENCES `Accounts`.`Status` (`StatusID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Accounts`.`Password`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Accounts`.`Password` (
  `PasswordID` VARCHAR(255) NOT NULL,
  `PasswordString` VARCHAR(60) NOT NULL,
  `OwnerID` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`PasswordID`),
  INDEX `fk_Password_Account_idx` (`OwnerID` ASC) VISIBLE,
  CONSTRAINT `fk_Password_Account`
    FOREIGN KEY (`OwnerID`)
    REFERENCES `Accounts`.`Account` (`AccountID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE USER 'spotymeAdmin' IDENTIFIED BY 'proyectoredes';
GRANT ALL PRIVILEGES ON *.* TO 'spotymeAdmin'@'%' WITH GRANT OPTION;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;


-- -----------------------------------------------------
-- Data for table `Accounts`.`Status`
-- -----------------------------------------------------
START TRANSACTION;
USE `Accounts`;
INSERT INTO `Accounts`.`Status` (`StatusID`, `Name`) VALUES (1, 'Active');
INSERT INTO `Accounts`.`Status` (`StatusID`, `Name`) VALUES (2, 'Bannred');
INSERT INTO `Accounts`.`Status` (`StatusID`, `Name`) VALUES (3, 'Admin');

COMMIT;

-- -----------------------------------------------------
-- Data for table `Accounts`.`Account`
-- -----------------------------------------------------
START TRANSACTION;
USE `Accounts`;
INSERT INTO `Accounts`.`Account` (`AccountID`, `Username`, `Email`, `IsUser`, `StatusID`) 
VALUES ('537a412b-e084-474a-a68d-705982b61802', 'spotymeAdmin', 'parkersjames87@gmail.com', 0, 3);
INSERT INTO `Accounts`.`Account` (`AccountID`, `Username`, `Email`, `IsUser`, `StatusID`) 
VALUES ('3f835f14-8111-4a1d-8385-2871743290e6', 'frews', 'zS18012183@estudiantes.uv.mx', 1, 1);
INSERT INTO `Accounts`.`Account` (`AccountID`, `Username`, `Email`, `IsUser`, `StatusID`) 
VALUES ('da43f900-890f-469e-90b3-18cdfacce74b', 'pklove', 'zS18012143@estudiantes.uv.mx', 1, 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `Accounts`.`Password`
-- -----------------------------------------------------
START TRANSACTION;
USE `Accounts`;
INSERT INTO `Accounts`.`Password` (`PasswordID`, `PasswordString`, `OwnerID`) 
VALUES ('17ab436b-5efc-46a3-a5d8-7d38015b2428', 'proyectoredes', '537a412b-e084-474a-a68d-705982b61802');
INSERT INTO `Accounts`.`Password` (`PasswordID`, `PasswordString`, `OwnerID`) 
VALUES ('796078d1-640a-40a2-83b7-15de3eb1aaae', 'elfrews', '3f835f14-8111-4a1d-8385-2871743290e6');
INSERT INTO `Accounts`.`Password` (`PasswordID`, `PasswordString`, `OwnerID`) 
VALUES ('f5770b74-c458-4d29-9b42-c926b78f8973', 'eladmin', 'da43f900-890f-469e-90b3-18cdfacce74b');

COMMIT;