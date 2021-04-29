-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Libraries
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Libraries` DEFAULT CHARACTER SET utf8 ;
USE `Libraries` ;

-- -----------------------------------------------------
-- Table `Libraries`.`Library`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Libraries`.`Library` (
  `LibraryID` VARCHAR(255) NOT NULL,
  `LibraryName` VARCHAR(60) NOT NULL,
  `AccountID` VARCHAR(255) NULL,
  PRIMARY KEY (`LibraryID`),
  UNIQUE INDEX `AccountID_UNIQUE` (`AccountID` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Libraries`.`Genre`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Libraries`.`Genre` (
  `GenreID` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(60) NOT NULL,
  PRIMARY KEY (`GenreID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Libraries`.`Album`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Libraries`.`Album` (
  `AlbumID` VARCHAR(255) NOT NULL,
  `Name` VARCHAR(60) NOT NULL,
  PRIMARY KEY (`AlbumID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Libraries`.`SongStatus`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Libraries`.`SongStatus` (
  `StatusID` INT NOT NULL,
  `Status` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`StatusID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Libraries`.`Song`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Libraries`.`Song` (
  `SongID` VARCHAR(255) NOT NULL,
  `Title` VARCHAR(255) NOT NULL,
  `LibraryID` VARCHAR(255) NOT NULL,
  `ArtistID` VARCHAR(255) NOT NULL,
  `Duration` INT NOT NULL,
  `releaseYear` VARCHAR(5) NOT NULL,
  `Producer` VARCHAR(60) NULL,
  `Composer` VARCHAR(60) NULL,
  `MultimediaID` VARCHAR(255) NULL,
  `GenreID` INT NOT NULL,
  `AlbumID` VARCHAR(255) NOT NULL,
  `StatusID` INT NOT NULL,
  PRIMARY KEY (`SongID`),
  INDEX `fk_Song_Genre1_idx` (`GenreID` ASC) VISIBLE,
  INDEX `fk_Song_Album1_idx` (`AlbumID` ASC) VISIBLE,
  INDEX `fk_Song_SongStatus1_idx` (`StatusID` ASC) VISIBLE,
  CONSTRAINT `FK_Song_PersonalLibrary`
    FOREIGN KEY (`LibraryID`)
    REFERENCES `Libraries`.`Library` (`LibraryID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Song_Genre1`
    FOREIGN KEY (`GenreID`)
    REFERENCES `Libraries`.`Genre` (`GenreID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Song_Album1`
    FOREIGN KEY (`AlbumID`)
    REFERENCES `Libraries`.`Album` (`AlbumID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Song_SongStatus1`
    FOREIGN KEY (`StatusID`)
    REFERENCES `Libraries`.`SongStatus` (`StatusID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE USER 'spotymeAdmin' IDENTIFIED BY 'proyectoredes';
GRANT ALL PRIVILEGES ON *.* TO 'spotymeAdmin'@'%' WITH GRANT OPTION;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;