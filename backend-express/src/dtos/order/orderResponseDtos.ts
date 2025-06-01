import type { Order } from "@prisma/client";

declare global {
  type OrderBasicResponseDto = {
    id: number;
    amount: number;
    hasBeenPaid: boolean;
  };

  type OrderDetailResponseDto = {
    createdDateTime: Date,
    paidDateTime: Date,
    itemAmount: number;
    vatAmount: number;
    items:
  } & OrderBasicResponseDto;
}

function createOrderBasicResponseDto(order: Order): OrderBasicResponseDto {
  return {
    id: order.id,
    amount: Number(order.itemAmount + order.vatAmount),
    hasBeenPaid: order.paidDateTime != null
  };
}

function createOrderDetailResponseDto(order: Order): OrderDetailResponseDto {
  return {
    ...createOrderBasicResponseDto(order),
    createdDateTime: order.createdDateTime
  }
}

export {
  createOrderBasicResponseDto,
  createOrderDetailResponseDto
}