import type { MenuCategory } from "@prisma/client";
import type { MenuCategoryResponseDto } from "@shared/dtos";

const menuCategoryResponseDtoFactory = {
  create(menuCategory: MenuCategory): MenuCategoryResponseDto {
    return {
      id: menuCategory.id,
      name: menuCategory.name
    };
  }
};

export default menuCategoryResponseDtoFactory;