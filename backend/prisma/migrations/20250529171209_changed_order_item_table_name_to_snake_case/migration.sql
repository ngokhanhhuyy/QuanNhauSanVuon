/*
  Warnings:

  - You are about to drop the `OrderItem` table. If the table is not empty, all the data it contains will be lost.

*/
-- DropForeignKey
ALTER TABLE `OrderItem` DROP FOREIGN KEY `fk__order_items__orders`;

-- DropTable
DROP TABLE `OrderItem`;

-- CreateTable
CREATE TABLE `order_items` (
    `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    `ordered_datetime` DATETIME(3) NOT NULL,
    `quantity` INTEGER UNSIGNED NOT NULL DEFAULT 1,
    `order_id` INTEGER UNSIGNED NOT NULL,

    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `order_items` ADD CONSTRAINT `fk__order_items__orders` FOREIGN KEY (`order_id`) REFERENCES `orders`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
