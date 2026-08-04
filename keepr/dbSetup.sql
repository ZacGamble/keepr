CREATE DATABASE IF NOT EXISTS keepr;
USE keepr;

-- 1. Accounts Table (Auth0 string ID)
CREATE TABLE IF NOT EXISTS `accounts` (
    `id` VARCHAR(255) NOT NULL PRIMARY KEY COMMENT 'primary key',
    `createdAt` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
    `updatedAt` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
    `name` VARCHAR(255) COMMENT 'User Name',
    `email` VARCHAR(255) COMMENT 'User Email',
    `picture` VARCHAR(255) COMMENT 'User Picture'
) DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;

-- 2. Keeps Table
CREATE TABLE IF NOT EXISTS `keeps` (
    `id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `creatorId` VARCHAR(255) NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `description` TEXT NOT NULL,
    `img` TEXT NOT NULL,
    `views` INT DEFAULT 0,
    `kept` INT DEFAULT 0,
    CONSTRAINT `fk_keeps_account` FOREIGN KEY (`creatorId`) REFERENCES `accounts` (`id`) ON DELETE CASCADE
) DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;

-- 3. Vaults Table
CREATE TABLE IF NOT EXISTS `vaults` (
    `id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `creatorId` VARCHAR(255) NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    `description` TEXT NOT NULL,
    `img` TEXT NOT NULL,
    `isPrivate` TINYINT DEFAULT 0,
    CONSTRAINT `fk_vaults_account` FOREIGN KEY (`creatorId`) REFERENCES `accounts` (`id`) ON DELETE CASCADE
) DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;

-- 4. VaultKeeps Table
CREATE TABLE IF NOT EXISTS `vaultkeeps` (
    `id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `creatorId` VARCHAR(255) NOT NULL,
    `vaultId` INT NOT NULL,
    `keepId` INT NOT NULL,
    CONSTRAINT `fk_vaultkeeps_account` FOREIGN KEY (`creatorId`) REFERENCES `accounts` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_vaultkeeps_vault` FOREIGN KEY (`vaultId`) REFERENCES `vaults` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_vaultkeeps_keep` FOREIGN KEY (`keepId`) REFERENCES `keeps` (`id`) ON DELETE CASCADE
) DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;