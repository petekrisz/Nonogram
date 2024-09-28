Create database NoNoGram;
USE `NonoGram` ;

DROP TABLE IF EXISTS users ;

CREATE TABLE IF NOT EXISTS `NonoGram`.`users` (
  `idUSER` INT UNSIGNED NOT NULL AUTO_INCREMENT unique,
  `userName` VARCHAR(25) NOT NULL unique,
  `timeRegistration` DATETIME NOT NULL,
  `firstName` VARCHAR(30) NOT NULL,
  `middleName` VARCHAR(30) NULL,
  `lastName` VARCHAR(40) NOT NULL,
  `score` INT NULL,
  `avatar` VARCHAR(255) NULL,
  `email` VARCHAR(120) NOT NULL unique,
  `token` INT UNSIGNED NULL,
  PRIMARY KEY (`idUSER`));

DROP TABLE IF EXISTS `NonoGram`.`TypesOfHelps` ;

CREATE TABLE IF NOT EXISTS `NonoGram`.`TypesOfHelps` (
  `typeHelp` VARCHAR(40) NOT NULL,
  `scoreHelp` INT NOT NULL,
  `tokePrice` INT NOT NULL,
  PRIMARY KEY (`typeHelp`));
  
DROP TABLE IF EXISTS HELPS;

CREATE TABLE IF NOT EXISTS HELPS (
  `idUSER` INT UNSIGNED NOT NULL unique,
  `typeHelp` VARCHAR(45) NOT NULL,
  `numberHelp` INT NOT NULL,
  PRIMARY KEY (`idUSER`),
  CONSTRAINT `idUSER`
    FOREIGN KEY (`idUSER`)
    REFERENCES `NonoGram`.`USERs` (`idUSER`),
  CONSTRAINT `typeHelp`
    FOREIGN KEY (`typeHelp`)
    REFERENCES `NonoGram`.`TypesOfHelps` (`typeHelp`));
    
DROP TABLE IF EXISTS `NonoGram`.`IMAGE` ;

CREATE TABLE IF NOT EXISTS `NonoGram`.`IMAGE` (
  `idIMAGE` INT NOT NULL AUTO_INCREMENT unique,
  `title` VARCHAR(60) NOT NULL,
  `rows` TINYINT UNSIGNED NOT NULL,
  `columns` TINYINT UNSIGNED NOT NULL,
  `category` VARCHAR(60) NOT NULL,
  `categoryLogo` VARCHAR(255) NULL,
  `content` LONGTEXT NOT NULL,
  `score` INT NOT NULL,
  `colourType` TINYINT UNSIGNED NOT NULL,
  PRIMARY KEY (`idIMAGE`));
  

  DROP TABLE IF EXISTS `NonoGram`.`SOLVED` ;

CREATE TABLE IF NOT EXISTS `NonoGram`.`SOLVED` (
  `IMAGE` INT NULL,
  `USER` INT UNSIGNED NOT NULL unique,
  `finished` bool NULL,
  `solvedScore` INT NULL,
  PRIMARY KEY (`USER`),
  CONSTRAINT `USER`
    FOREIGN KEY (`USER`)
    REFERENCES `NonoGram`.`USERs` (`idUSER`),
  CONSTRAINT `IMAGE`
    FOREIGN KEY (`IMAGE`)
    REFERENCES `NonoGram`.`IMAGE` (`idIMAGE`));
