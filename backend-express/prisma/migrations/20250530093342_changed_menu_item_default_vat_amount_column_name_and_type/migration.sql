/*
  Warnings:

  - You are about to alter the column `vat_amount` on the `menu_items` table. The data in that column could be lost. The data in that column will be cast from `UnsignedBigInt` to `UnsignedInt`.

*/
-- AlterTable
ALTER TABLE `menu_items` MODIFY `vat_amount` INTEGER UNSIGNED NOT NULL DEFAULT 0;
