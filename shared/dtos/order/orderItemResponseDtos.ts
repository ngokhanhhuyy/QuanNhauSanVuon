import type { OrderItem } from "@prisma/client";

declare global {
  type OrderItemBasicResponseDto = {
    id: number;
    quantity: number;
    menuItem:
  }
}