/*
  Warnings:

  - Added the required column `menuItemId` to the `order_items` table without a default value. This is not possible if the table is not empty.

*/
-- AlterTable
ALTER TABLE `order_items` ADD COLUMN `menuItemId` INTEGER UNSIGNED NOT NULL;

-- AddForeignKey
ALTER TABLE `order_items` ADD CONSTRAINT `fk__order_items__menu_items` FOREIGN KEY (`menuItemId`) REFERENCES `menu_items`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
