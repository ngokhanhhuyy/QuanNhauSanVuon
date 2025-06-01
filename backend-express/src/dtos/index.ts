// RequestDtoFactories.
import menuItemRequestDtoFactory from "./menuItem/menuItemRequestDtos";

// ResponseDtoFactories
import userResponseDtoFactory from "./user/userResponseDtos";
import menuCategoryResponseDtoFactory from "./menuCategory/menuCategoryResponseDtos";
import menuItemResponseDtoFactory from "./menuItem/menuItemResponseDtos";

export const requestDtoFactory = {
  menuItem: menuItemRequestDtoFactory
}

export const responseDtoFactory = {
  user: userResponseDtoFactory,
  menuCategory: menuCategoryResponseDtoFactory,
  menuItem: menuItemResponseDtoFactory
}