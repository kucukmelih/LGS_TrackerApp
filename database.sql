-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: lgs_tracking
-- ------------------------------------------------------
-- Server version	9.2.0

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
-- Table structure for table `exams`
--

DROP TABLE IF EXISTS `exams`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `exams` (
  `exam_id` int NOT NULL AUTO_INCREMENT,
  `student_id` int NOT NULL,
  `exam_date` date NOT NULL,
  `exam_description` text,
  `score` decimal(6,2) DEFAULT NULL,
  PRIMARY KEY (`exam_id`),
  KEY `student_id` (`student_id`),
  CONSTRAINT `exams_ibfk_1` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `exams`
--

LOCK TABLES `exams` WRITE;
/*!40000 ALTER TABLE `exams` DISABLE KEYS */;
INSERT INTO `exams` VALUES (1,1,'2025-05-19','Deneme 1',NULL),(2,1,'2025-05-19','Deneme 2',NULL),(3,1,'2025-05-19','Deneme 3',359.88),(4,1,'2025-05-20','Imported from OCR/PDF',NULL),(5,1,'2025-05-20','Imported from OCR/PDF',NULL),(8,1,'2025-05-20','Deneme 5',393.83),(9,1,'2025-05-20','DENEME SON',401.23),(11,1,'2025-05-21','Deeneme 10',393.83);
/*!40000 ALTER TABLE `exams` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `results`
--

DROP TABLE IF EXISTS `results`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `results` (
  `result_id` int NOT NULL AUTO_INCREMENT,
  `exam_id` int NOT NULL,
  `subject` enum('Matematik','Fen Bilimleri','Türkçe','İnkılap Tarihi','Din Kültürü','İngilizce') NOT NULL,
  `correct` int DEFAULT '0',
  `incorrect` int DEFAULT '0',
  `blank` int DEFAULT '0',
  PRIMARY KEY (`result_id`),
  KEY `exam_id` (`exam_id`),
  CONSTRAINT `results_ibfk_1` FOREIGN KEY (`exam_id`) REFERENCES `exams` (`exam_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `results`
--

LOCK TABLES `results` WRITE;
/*!40000 ALTER TABLE `results` DISABLE KEYS */;
INSERT INTO `results` VALUES (1,1,'Matematik',18,1,1),(2,1,'Fen Bilimleri',16,3,1),(3,1,'Türkçe',19,1,0),(4,1,'İnkılap Tarihi',8,2,0),(5,1,'Din Kültürü',9,0,1),(6,1,'İngilizce',7,1,2),(7,2,'Matematik',15,3,2),(8,2,'Fen Bilimleri',20,0,0),(9,2,'Türkçe',19,0,1),(10,2,'İnkılap Tarihi',8,2,0),(11,2,'Din Kültürü',9,1,0),(12,2,'İngilizce',9,1,0),(13,3,'Matematik',15,5,0),(14,3,'Fen Bilimleri',12,3,5),(15,3,'Türkçe',18,2,0),(16,3,'İnkılap Tarihi',9,0,1),(17,3,'Din Kültürü',10,0,0),(18,3,'İngilizce',9,1,0),(31,8,'Türkçe',17,3,0),(32,8,'İnkılap Tarihi',9,1,0),(33,8,'Din Kültürü',8,2,0),(34,8,'İngilizce',10,0,0),(35,8,'Matematik',17,3,0),(36,8,'Fen Bilimleri',16,4,0),(37,9,'Türkçe',17,3,0),(38,9,'İnkılap Tarihi',9,1,0),(39,9,'Din Kültürü',8,2,0),(40,9,'İngilizce',10,0,0),(41,9,'Matematik',17,0,3),(42,9,'Fen Bilimleri',16,4,0),(49,11,'Türkçe',17,3,0),(50,11,'İnkılap Tarihi',9,1,0),(51,11,'Din Kültürü',8,2,0),(52,11,'İngilizce',10,0,0),(53,11,'Matematik',17,3,0),(54,11,'Fen Bilimleri',16,4,0);
/*!40000 ALTER TABLE `results` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `student_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `gender` enum('Male','Female','Other') DEFAULT NULL,
  `school_name` varchar(100) DEFAULT NULL,
  `class_level` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`student_id`),
  UNIQUE KEY `email` (`email`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `students_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES (1,2,'kucuk.melih@outlook.com','Male','Maltepe Universitesi','3');
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `uploads`
--

DROP TABLE IF EXISTS `uploads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `uploads` (
  `upload_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `file_name` varchar(255) DEFAULT NULL,
  `file_type` enum('pdf','image') DEFAULT NULL,
  `uploaded_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`upload_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `uploads_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `uploads`
--

LOCK TABLES `uploads` WRITE;
/*!40000 ALTER TABLE `uploads` DISABLE KEYS */;
/*!40000 ALTER TABLE `uploads` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(100) NOT NULL,
  `full_name` varchar(100) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `role` enum('admin','student') NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','admin61*','Admin','admin@outlook.com','admin'),(2,'anchovy','anchovy61*','Melih KUCUK',NULL,'student');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-24 15:53:14
