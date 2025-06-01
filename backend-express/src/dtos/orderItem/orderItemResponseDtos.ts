import type { OrderItem } from "@prisma/client";
import { OrderItemBasicResponseDto } from "@shared/dtos/orderItem/orderItemResponseDtos";
import
  menuItemResponseDtoFactory,
  { type CategoryIncludedMenuItem } from "../menuItem/menuItemResponseDtos";
type MenuItemIncludedOrderItem = OrderItem & { menuItem: CategoryIncludedMenuItem };

const orderItemResponseDtoFactory = {
  createBasic(orderItem: MenuItemIncludedOrderItem): OrderItemBasicResponseDto {
    return {
      id: orderItem.id,
      quantity: orderItem.quantity,
      menuItem: menuItemResponseDtoFactory.createBasic(orderItem.menuItem)
    };
  }
}

export default orderItemResponseDtoFactory;