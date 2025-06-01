import type { IMenuCategoryResponseDto } from "../menuCategory/menuCategoryResponseDtos";

export type MenuItemListResponseDto = {
  pageCount: number;
  items: MenuItemBasicResponseDto[];
};

export type MenuItemBasicResponseDto = {
  id: number;
  name: string;
  defaultAmount: number;
  thumbnailUrl: string | null;
  category: IMenuCategoryResponseDto | null;
};

export type MenuItemDetailResponseDto = {
  defaultVatPercentage: number;
};