/*
  Warnings:

  - The primary key for the `OrderItem` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to alter the column `id` on the `OrderItem` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `quantity` on the `OrderItem` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `order_id` on the `OrderItem` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - The primary key for the `menu_categories` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to alter the column `id` on the `menu_categories` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - The primary key for the `menu_items` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to alter the column `id` on the `menu_items` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `default_amount` on the `menu_items` table. The data in that column could be lost. The data in that column will be cast from `BigInt` to `UnsignedBigInt`.
  - You are about to alter the column `vat_amount` on the `menu_items` table. The data in that column could be lost. The data in that column will be cast from `BigInt` to `UnsignedBigInt`.
  - You are about to alter the column `category_id` on the `menu_items` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - The primary key for the `monthly_summaries` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to alter the column `id` on the `monthly_summaries` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `year` on the `monthly_summaries` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `month` on the `monthly_summaries` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `revenue` on the `monthly_summaries` table. The data in that column could be lost. The data in that column will be cast from `BigInt` to `UnsignedBigInt`.
  - You are about to alter the column `collected_vat_amount` on the `monthly_summaries` table. The data in that column could be lost. The data in that column will be cast from `BigInt` to `UnsignedBigInt`.
  - The primary key for the `orders` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to alter the column `id` on the `orders` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `item_amount` on the `orders` table. The data in that column could be lost. The data in that column will be cast from `BigInt` to `UnsignedBigInt`.
  - You are about to alter the column `vat_amount` on the `orders` table. The data in that column could be lost. The data in that column will be cast from `BigInt` to `UnsignedBigInt`.
  - The primary key for the `tables` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to alter the column `id` on the `tables` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `position_x` on the `tables` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `position_y` on the `tables` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - The primary key for the `user_permissions` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to alter the column `id` on the `user_permissions` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - You are about to alter the column `userId` on the `user_permissions` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - The primary key for the `users` table will be changed. If it partially fails, the table could be left without primary key constraint.
  - You are about to alter the column `id` on the `users` table. The data in that column could be lost. The data in that column will be cast from `Int` to `UnsignedInt`.
  - Added the required column `table_id` to the `orders` table without a default value. This is not possible if the table is not empty.

*/
-- DropForeignKey
ALTER TABLE `OrderItem` DROP FOREIGN KEY `fk__order_items__orders`;

-- DropForeignKey
ALTER TABLE `menu_items` DROP FOREIGN KEY `fk__menu_items__menu_categories`;

-- DropForeignKey
ALTER TABLE `user_permissions` DROP FOREIGN KEY `fk__user_permissions__users`;

-- DropIndex
DROP INDEX `fk__order_items__orders` ON `OrderItem`;

-- DropIndex
DROP INDEX `fk__menu_items__menu_categories` ON `menu_items`;

-- AlterTable
ALTER TABLE `OrderItem` DROP PRIMARY KEY,
    MODIFY `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    MODIFY `quantity` INTEGER UNSIGNED NOT NULL DEFAULT 1,
    MODIFY `order_id` INTEGER UNSIGNED NOT NULL,
    ADD PRIMARY KEY (`id`);

-- AlterTable
ALTER TABLE `menu_categories` DROP PRIMARY KEY,
    MODIFY `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    ADD PRIMARY KEY (`id`);

-- AlterTable
ALTER TABLE `menu_items` DROP PRIMARY KEY,
    MODIFY `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    MODIFY `default_amount` BIGINT UNSIGNED NOT NULL DEFAULT 0,
    MODIFY `vat_amount` BIGINT UNSIGNED NOT NULL DEFAULT 0,
    MODIFY `category_id` INTEGER UNSIGNED NOT NULL,
    ADD PRIMARY KEY (`id`);

-- AlterTable
ALTER TABLE `monthly_summaries` DROP PRIMARY KEY,
    MODIFY `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    MODIFY `year` INTEGER UNSIGNED NOT NULL,
    MODIFY `month` INTEGER UNSIGNED NOT NULL,
    MODIFY `revenue` BIGINT UNSIGNED NOT NULL,
    MODIFY `collected_vat_amount` BIGINT UNSIGNED NOT NULL,
    ADD PRIMARY KEY (`id`);

-- AlterTable
ALTER TABLE `orders` DROP PRIMARY KEY,
    ADD COLUMN `table_id` INTEGER UNSIGNED NOT NULL,
    MODIFY `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    MODIFY `item_amount` BIGINT UNSIGNED NOT NULL DEFAULT 0,
    MODIFY `vat_amount` BIGINT UNSIGNED NOT NULL DEFAULT 0,
    ADD PRIMARY KEY (`id`);

-- AlterTable
ALTER TABLE `tables` DROP PRIMARY KEY,
    MODIFY `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    MODIFY `position_x` INTEGER UNSIGNED NOT NULL,
    MODIFY `position_y` INTEGER UNSIGNED NOT NULL,
    ADD PRIMARY KEY (`id`);

-- AlterTable
ALTER TABLE `user_permissions` DROP PRIMARY KEY,
    MODIFY `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    MODIFY `userId` INTEGER UNSIGNED NOT NULL,
    ADD PRIMARY KEY (`id`);

-- AlterTable
ALTER TABLE `users` DROP PRIMARY KEY,
    MODIFY `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
    ADD PRIMARY KEY (`id`);

-- AddForeignKey
ALTER TABLE `orders` ADD CONSTRAINT `fk__orders__tables` FOREIGN KEY (`table_id`) REFERENCES `tables`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `OrderItem` ADD CONSTRAINT `fk__order_items__orders` FOREIGN KEY (`order_id`) REFERENCES `orders`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `menu_items` ADD CONSTRAINT `fk__menu_items__menu_categories` FOREIGN KEY (`category_id`) REFERENCES `menu_categories`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `user_permissions` ADD CONSTRAINT `fk__user_permissions__users` FOREIGN KEY (`userId`) REFERENCES `users`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
