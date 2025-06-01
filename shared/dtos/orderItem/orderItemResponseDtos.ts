import type { MenuItemBasicResponseDto } from "../menuItem/menuItemResponseDtos";

export type OrderItemBasicResponseDto = {
  id: number;
  quantity: number;
  menuItem: MenuItemBasicResponseDto;
};

export type OrderItemDetailResponseDto = {
  orderedDateTime: Date;
  lastUpdatedDateTime: Date | null;
} & OrderItemBasicResponseDto;