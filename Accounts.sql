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