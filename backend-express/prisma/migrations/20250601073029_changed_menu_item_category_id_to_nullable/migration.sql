-- DropForeignKey
ALTER TABLE `menu_items` DROP FOREIGN KEY `fk__menu_items__menu_categories`;

-- DropIndex
DROP INDEX `fk__menu_items__menu_categories` ON `menu_items`;

-- AlterTable
ALTER TABLE `menu_items` MODIFY `category_id` INTEGER UNSIGNED NULL;

-- AddForeignKey
ALTER TABLE `menu_items` ADD CONSTRAINT `fk__menu_items__menu_categories` FOREIGN KEY (`category_id`) REFERENCES `menu_categories`(`id`) ON DELETE SET NULL ON UPDATE CASCADE;
