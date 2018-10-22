-- phpMyAdmin SQL Dump
-- version 4.8.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 23, 2018 at 12:12 AM
-- Server version: 10.1.32-MariaDB
-- PHP Version: 7.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `usight`
--

-- --------------------------------------------------------

--
-- Table structure for table `wanted_list`
--

CREATE TABLE `wanted_list` (
  `id` varchar(255) NOT NULL,
  `owner` varchar(255) NOT NULL,
  `license_plate` varchar(255) NOT NULL,
  `engine_number` varchar(255) NOT NULL,
  `vin` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `wanted_list`
--

INSERT INTO `wanted_list` (`id`, `owner`, `license_plate`, `engine_number`, `vin`) VALUES
('1', 'Jonas', 'ABC123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('2', 'Paulius Vaicekauskas', 'ASD 123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('3', 'Jonas', 'ABC123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('4', 'Jonas', 'ABC123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('5', 'Jonas', 'ABC123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('6', 'Jonas', 'ABC123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('7', 'Jonas', 'ABC123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('8', 'Jonas', 'ABC123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('9', 'Jonas', 'ABC123', 'VW12345U123456P', '1N6AD0EV7BC423878'),
('10', 'sdasd', 'asasdasd', 'asdasd', 'asd'),
('11', 'dasdasdasd', 'asdasd', 'asdasd', 'asdasd'),
('12', 'ssss', 'sssss', 'ssssss', 'sss');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
