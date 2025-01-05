-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: nonogram
-- ------------------------------------------------------
-- Server version	9.1.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `help`
--

DROP TABLE IF EXISTS `help`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `help` (
  `TypeOfHelp` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Price` int DEFAULT NULL,
  `Weight` double DEFAULT NULL,
  `HelpLogoG` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `HelpLogoL` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`TypeOfHelp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `help`
--

LOCK TABLES `help` WRITE;
/*!40000 ALTER TABLE `help` DISABLE KEYS */;
INSERT INTO `help` VALUES ('Check3H',18,0.4,'CheckAll_3H_gold.png','CheckAll_3H_light.png'),('Erase',15,0.6,'EraseW_gold.png','EraseW_light.png'),('H1',5,1,'1H_gold.png','1H_light.png'),('H13',25,0.12,'13HDiamond_gold.png','13HDiamond_light.png'),('H3',12,0.8,'3H_gold.png','3H_light.png'),('H8',18,0.3,'8H_gold.png','8H_light.png'),('L1',25,0.2,'1LHint_gold.png','1LHint_light.png'),('L3',50,0.07,'3LineHint_gold.png','3LineHint_light.png');
/*!40000 ALTER TABLE `help` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `image`
--

DROP TABLE IF EXISTS `image`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `image` (
  `IMAGEId` int NOT NULL AUTO_INCREMENT,
  `Title` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `IMAGERows` int DEFAULT NULL,
  `IMAGEColumns` int DEFAULT NULL,
  `Category` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `CategoryLogo` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Content` text COLLATE utf8mb4_unicode_ci,
  `Score` int DEFAULT NULL,
  `ColourType` int DEFAULT NULL,
  `RowFinished` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `ColumnFinished` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`IMAGEId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image`
--

LOCK TABLES `image` WRITE;
/*!40000 ALTER TABLE `image` DISABLE KEYS */;
INSERT INTO `image` VALUES (1,'Déli',15,15,'növény','plant_icon_gold.png','000011111100000001110000011000011000010001100010010101111100100000011111110101000110101110101101110101111100001011010111100101001110011100001011110111111001110110111110010100101111011000110101110001100011111100000011111111000',429,0,'000000100000000','000000000000000'),(2,'Mozgó szökőkút',30,30,'állat','animal_icon_gold.png','110010100110000000000110000000011011101100000000000111000000001101011000000000000111100000000101010000000000000111100000000101010000000000000111100000000010100000000000000011100000000001000000000000000001111110000001000000000000000001111111000001000000000000000011101111000001000000000000000111100000000001000000000000001111100000000001000000000000011111100000000001000000000011111111100000000110111111111111111111100000001111111111111111111111100000011111111111111111111111000000111111111111111111111111000000111111111111111111111111000000111111111111111111111110000000111111111011111111111100000000111111111111111111111000000000000000001111111111110000000110000000001111111111000000111100001111111111111110000000000011000111111111000111000011111100110000000000000011101111100111000011111000111100000001110000111111001111000011111111100011000111100011111111000011111110011100011110000110001110000111',1929,0,'000000000000000000000000000000','000000000000000000000000000000'),(3,'Kormányos',40,30,'ember','person_icon_gold.png','000011000000011110000000000000000010000000110011000000011011001101000001101111100000010101001011000001110001110000000000000000000011101110110000000000000000000011010001011000000000000000000110111111101110000000000000001101010101010111011100110110011101000100010110110010101010011011101100111101111001000000011011010011011111100001000000001101001110011101111001000000000111000000111011100010000000000011100001100111110111000000001111011111011111111101000000111110100100000110100100000001110001100100011111100111000001111111110100111100100100000011000111110111100011111111000011110011101110011110101100000110101011011001100001101110000100110110100111000111101110000110011011011100001111101111000111001111110100000111101001000111100100110100001110101000001110101100010100111110101000001100011010011100011110101000011110010101101101111110101000011010110110110110111110101000011100101111011011111110101000011001101111001101111111101000011101011111100110111111101000011011011111110111011111101000011110111111100101101111101001101110111111000100110111101011100001111111000101111011101110100111111111100111111101101101100100011111111111111110111011110010010000000000000001101110011111101111111111111111000100',2326,0,'0000000000000000000000000000000000000000','000000000000000000000000000000'),(4,'Simple',5,5,'semmi','NO_icon_gold.png','1111100000110110000011111',500,0,'11011','00100');
/*!40000 ALTER TABLE `image` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `UserName` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Password` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `FirstName` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `LastName` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Email` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `TimeOfRegistration` datetime DEFAULT NULL,
  `Score` int DEFAULT NULL,
  `Tokens` int DEFAULT NULL,
  `Avatar` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`UserName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('karcsika','1635c8525afbae58c37bede3c9440844e9143727cc7c160bed665ec378d8a262','Károly','Pál','spam.pk75.spam@gmail.com','2025-01-01 23:49:15',0,40,''),('netuddki','e0bebd22819993425814866b62701e2919ea26f1370499c1037b53b9d49c2c8a','Szultán','Török','netuddki@rejtochars.hu','2018-01-19 03:14:07',17679,475,'Avatar_netuddki.png');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userhelp`
--

DROP TABLE IF EXISTS `userhelp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userhelp` (
  `UserName` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `H1` int DEFAULT NULL,
  `H3` int DEFAULT NULL,
  `H8` int DEFAULT NULL,
  `H13` int DEFAULT NULL,
  `L1` int DEFAULT NULL,
  `L3` int DEFAULT NULL,
  `Check3H` int DEFAULT NULL,
  `Erase` int DEFAULT NULL,
  PRIMARY KEY (`UserName`),
  CONSTRAINT `userhelp_ibfk_1` FOREIGN KEY (`UserName`) REFERENCES `user` (`UserName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userhelp`
--

LOCK TABLES `userhelp` WRITE;
/*!40000 ALTER TABLE `userhelp` DISABLE KEYS */;
INSERT INTO `userhelp` VALUES ('karcsika',2,0,0,0,0,0,0,0),('netuddki',23,15,13,9,7,23,16,6);
/*!40000 ALTER TABLE `userhelp` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userimage`
--

DROP TABLE IF EXISTS `userimage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userimage` (
  `UserName` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `IMAGEId` int NOT NULL,
  `Finished` tinyint(1) DEFAULT NULL,
  `Content` text COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`UserName`,`IMAGEId`),
  KEY `IMAGEId` (`IMAGEId`),
  CONSTRAINT `userimage_ibfk_1` FOREIGN KEY (`UserName`) REFERENCES `user` (`UserName`),
  CONSTRAINT `userimage_ibfk_2` FOREIGN KEY (`IMAGEId`) REFERENCES `image` (`IMAGEId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userimage`
--

LOCK TABLES `userimage` WRITE;
/*!40000 ALTER TABLE `userimage` DISABLE KEYS */;
INSERT INTO `userimage` VALUES ('karcsika',2,0,'111xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx'),('netuddki',1,1,'x'),('netuddki',2,0,'xxxxxxxxxxxxxxxxxxxxxxxxxxx?????????????????????xxxxxxxxxxx1111000000??????????????????????xxxxxx????????????xxxxxx111x1x1x????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx111xxxxxxxxxxxxxxxxxx1xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx1xxxxxxxxxxxxx1xxxxxxxxxxxxxxxxxxxxxxxxxxx1xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx1xxxxxxxxxxxxxxxxxxxxxxx0xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx0xxxxxxxxxxxxxxxxxxxxxxxxx0xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx0xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx0000xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx0000xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx0000xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx000????????????????xxxxxxxxxxxxxxxxxxxxxx'),('netuddki',4,1,'1111100000110110000011111');
/*!40000 ALTER TABLE `userimage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'nonogram'
--

--
-- Dumping routines for database 'nonogram'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-01-05  0:39:42
