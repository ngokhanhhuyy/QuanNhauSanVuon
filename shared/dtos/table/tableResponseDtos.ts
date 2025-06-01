import type { OrderBasicResponseDto } from "../order/orderResponseDtos";

export type TableBasicResponseDto = {
  id: number;
  name: string;
  positionX: number;
  positionY: number;
};

export type TableDetailResponseDto = {
  order: OrderBasicResponseDto;
} & TableBasicResponseDto;