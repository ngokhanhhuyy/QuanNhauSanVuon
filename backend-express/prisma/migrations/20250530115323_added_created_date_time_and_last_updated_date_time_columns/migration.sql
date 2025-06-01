/*
  Warnings:

  - Added the required column `created_datetime` to the `menu_items` table without a default value. This is not possible if the table is not empty.
  - Added the required column `last_updated_datetime` to the `order_items` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `menu_items` ADD COLUMN `created_datetime` DATETIME(3) NOT NULL,
    ADD COLUMN `last_updated_datetime` DATETIME(3) NULL;

-- AlterTable
ALTER TABLE `order_items` ADD COLUMN `last_updated_datetime` DATETIME(3) NOT NULL;

-- AlterTable
ALTER TABLE `orders` ADD COLUMN `last_updated_datetime` DATETIME(3) NULL;
