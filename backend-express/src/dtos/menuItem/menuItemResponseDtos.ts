import type { MenuItem, MenuCategory } from "@prisma/client";
import type {
  MenuItemListResponseDto,
  MenuItemBasicResponseDto,
  MenuItemDetailResponseDto,
} from "@shared/dtos";
import menuCategoryResponseDtoFactory from "../menuCategory/menuCategoryResponseDtos";

export type CategoryIncludedMenuItem = MenuItem & { category: MenuCategory | null };

const menuItemResponseDtoFactory = {
  createList(pageCount: number, items: CategoryIncludedMenuItem[]): MenuItemListResponseDto {
    return {
      pageCount,
      items: items.map(item => this.createBasic(item))
    };
  },

  createBasic(menuItem: CategoryIncludedMenuItem): MenuItemBasicResponseDto {
    return {
      id: menuItem.id,
      name: menuItem.name,
      defaultAmount: Number(menuItem.defaultAmount),
      thumbnailUrl: menuItem.thumbnailUrl,
      category: menuItem.category && menuCategoryResponseDtoFactory.create(menuItem.category)
    };
  },

  createDetail(menuItem: CategoryIncludedMenuItem): MenuItemDetailResponseDto {
    return {
      ...this.createBasic(menuItem),
      defaultVatPercentage: menuItem.defaultVatPercentage
    };
  }
};

export default menuItemResponseDtoFactory;