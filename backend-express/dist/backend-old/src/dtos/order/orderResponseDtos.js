function createOrderBasicResponseDto(order) {
    return {
        id: order.id,
        amount: Number(order.itemAmount + order.vatAmount),
        hasBeenPaid: order.paidDateTime != null
    };
}
function createOrderDetailResponseDto(order) {
    return {
        ...createOrderBasicResponseDto(order),
        createdDateTime: order.createdDateTime
    };
}
export { createOrderBasicResponseDto, createOrderDetailResponseDto };
