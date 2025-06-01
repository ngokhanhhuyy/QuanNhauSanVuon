import menuItemResponseDtoFactory from "../menuItem/menuItemResponseDtos";
const orderItemResponseDtoFactory = {
    createBasic(orderItem) {
        return {
            id: orderItem.id,
            quantity: orderItem.quantity,
            menuItem: menuItemResponseDtoFactory.createBasic(orderItem.menuItem)
        };
    }
};
export default orderItemResponseDtoFactory;
