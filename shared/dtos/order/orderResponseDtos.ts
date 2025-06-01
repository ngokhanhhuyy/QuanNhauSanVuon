import type { OrderItemBasicResponseDto } from "../orderItem/orderItemResponseDtos";

export type OrderBasicResponseDto = {
  id: number;
  amount: number;
  hasBeenPaid: boolean;
};

export type OrderDetailResponseDto = {
  createdDateTime: Date;
  paidDateTime: Date;
  itemAmount: number;
  vatAmount: number;
  items: OrderItemBasicResponseDto[];
} & OrderBasicResponseDto;