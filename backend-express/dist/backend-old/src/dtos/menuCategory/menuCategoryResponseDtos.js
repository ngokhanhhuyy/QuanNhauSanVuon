const menuCategoryResponseDtoFactory = {
    create(menuCategory) {
        return {
            id: menuCategory.id,
            name: menuCategory.name
        };
    }
};
export default menuCategoryResponseDtoFactory;
