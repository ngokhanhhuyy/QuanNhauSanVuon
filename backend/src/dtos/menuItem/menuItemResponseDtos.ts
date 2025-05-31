import type { MenuItem, MenuCategory } from "@prisma/client";
import type {
  MenuItemListResponseDto,
  MenuItemBasicResponseDto,
  MenuItemDetailResponseDto,
  MenuCategoryResponseDto
} from "@shared/dtos";
import { createMenuCategoryResponseDto } from "../menuCategory/menuCategoryResponseDtos";

type CategoryIncludedMenuItem = MenuItem & { category: MenuCategory | null };
type ListResponseDtoCreateParam = {
  pageCount: number;
  items: CategoryIncludedMenuItem[];
}

function createList(params?: ListResponseDtoCreateParam): MenuItemListResponseDto {
  const responseDto: MenuItemListResponseDto = {
    pageCount: 0,
    items: []
  }

  if (params) {
    responseDto.pageCount = params.pageCount;
    responseDto.items = params.items.map(item => createBasic(item));
  }

  return responseDto;
}

function createBasic(menuItem: CategoryIncludedMenuItem): MenuItemBasicResponseDto {
  return {
    id: menuItem.id,
    name: menuItem.name,
    defaultAmount: Number(menuItem.defaultAmount),
    thumbnailUrl: menuItem.thumbnailUrl,
    category: menuItem.category && createMenuCategoryResponseDto(menuItem.category)
  };
}

function createDetail(menuItem: CategoryIncludedMenuItem): MenuItemDetailResponseDto {
  return {
    ...createBasic(menuItem),
    defaultVatPercentage: menuItem.defaultVatPercentage
  }
}

export {
  createList as createMenuItemListResponseDto,
  createBasic as createMenuItemBasicResponseDto,
  createDetail as createMenuItemDetailResponseDto
}