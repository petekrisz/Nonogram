USE `NonoGram`;

DROP TABLE IF EXISTS users;
CREATE TABLE IF NOT EXISTS `users` (
    `idUSER` INT UNSIGNED NOT NULL AUTO_INCREMENT UNIQUE,
    `userName` VARCHAR(25) NOT NULL UNIQUE,
    `timeRegistration` DATETIME NOT NULL,
    `firstName` VARCHAR(30) NOT NULL,
    `middleName` VARCHAR(30) NULL,
    `lastName` VARCHAR(40) NOT NULL,
    `score` INT NULL,
    `avatar` VARCHAR(255) NULL,
    `email` VARCHAR(120) NOT NULL UNIQUE,
    PRIMARY KEY (`idUSER`)
);

DROP TABLE IF EXISTS `TypesOfHelps`;
CREATE TABLE IF NOT EXISTS `TypesOfHelps` (
    `typeHelp` VARCHAR(40) NOT NULL,
    `scoreHelp` INT NOT NULL,
    `tokePrice` INT NOT NULL,
    PRIMARY KEY (`typeHelp`)
);

DROP TABLE IF EXISTS `HELPS`;
CREATE TABLE IF NOT EXISTS `HELPS` (
    `idHELP` INT NOT NULL AUTO_INCREMENT UNIQUE,
    `idUSER` INT UNSIGNED NOT NULL,
    `typeHelp` VARCHAR(45) NOT NULL,
    `numberHelp` INT NOT NULL,
    PRIMARY KEY (`idHELP`),
    CONSTRAINT `USER` FOREIGN KEY (`idUSER`) REFERENCES `users` (`idUSER`),
    CONSTRAINT `typeHelp` FOREIGN KEY (`typeHelp`) REFERENCES `TypesOfHelps` (`typeHelp`)
);

DROP TABLE IF EXISTS `IMAGE`;
CREATE TABLE IF NOT EXISTS `IMAGE` (
    `idIMAGE` INT NOT NULL AUTO_INCREMENT UNIQUE,
    `title` VARCHAR(60) NOT NULL,
    `rows` TINYINT UNSIGNED NOT NULL,
    `columns` TINYINT UNSIGNED NOT NULL,
    `category` VARCHAR(60) NOT NULL,
    `categoryLogo` VARCHAR(255) NULL,
    `content` LONGTEXT NOT NULL,
    `score` INT NOT NULL,
    `colourType` TINYINT UNSIGNED NOT NULL,
    PRIMARY KEY (`idIMAGE`)
);

DROP TABLE IF EXISTS `SOLVED`;
CREATE TABLE IF NOT EXISTS `SOLVED` (
    `IMAGE` INT NULL,
    `USER` INT UNSIGNED NOT NULL UNIQUE,
    `finished` BOOL NULL,
    `solvedScore` INT NULL,
    PRIMARY KEY (`USER`),
    CONSTRAINT `USER` FOREIGN KEY (`USER`) REFERENCES `users` (`idUSER`),
    CONSTRAINT `IMAGE` FOREIGN KEY (`IMAGE`) REFERENCES `IMAGE` (`idIMAGE`)
);