/*
  Warnings:

  - Added the required column `unit` to the `menu_items` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `menu_items` ADD COLUMN `unit` VARCHAR(50) NOT NULL;
