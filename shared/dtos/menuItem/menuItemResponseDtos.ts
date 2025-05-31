import type { MenuCategoryResponseDto } from "../menuCategory/menuCategoryResponseDtos";

export type MenuItemListResponseDto = {
  pageCount: number;
  items: MenuItemBasicResponseDto[];
}

export type MenuItemBasicResponseDto = {
  id: number;
  name: string;
  defaultAmount: number;
  thumbnailUrl: string | null;
  category: MenuCategoryResponseDto | null;
};

export type MenuItemDetailResponseDto = {
  defaultVatPercentage: number;
};