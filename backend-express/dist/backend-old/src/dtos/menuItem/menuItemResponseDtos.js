import menuCategoryResponseDtoFactory from "../menuCategory/menuCategoryResponseDtos";
const menuItemResponseDtoFactory = {
    createList(pageCount, items) {
        return {
            pageCount,
            items: items.map(item => this.createBasic(item))
        };
    },
    createBasic(menuItem) {
        return {
            id: menuItem.id,
            name: menuItem.name,
            defaultAmount: Number(menuItem.defaultAmount),
            thumbnailUrl: menuItem.thumbnailUrl,
            category: menuItem.category && menuCategoryResponseDtoFactory.create(menuItem.category)
        };
    },
    createDetail(menuItem) {
        return {
            ...this.createBasic(menuItem),
            defaultVatPercentage: menuItem.defaultVatPercentage
        };
    }
};
export default menuItemResponseDtoFactory;
