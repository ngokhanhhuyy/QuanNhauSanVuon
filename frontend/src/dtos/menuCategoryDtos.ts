declare global {
  type MenuCategoryDto = {
    id: number;
    name: string;
  };

  type MenuCategoryUpsertDto = {
    name: string;
  };
}

export { };