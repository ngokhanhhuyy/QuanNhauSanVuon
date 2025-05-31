import type { MenuCategory } from "@prisma/client";
import type { MenuCategoryResponseDto } from "@shared/dtos";

function createMenuCategoryResponseDto(menuCategory: MenuCategory): MenuCategoryResponseDto {
  return {
    id: menuCategory.id,
    name: menuCategory.name
  };
}

export { createMenuCategoryResponseDto };