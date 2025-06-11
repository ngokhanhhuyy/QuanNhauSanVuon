import { createBaseModel } from "./baseModels";

declare global {
  type SignInModel = {
    userName: string;
    password: string;
    toDto(): SignInDto;
  };
}

export function create(): SignInModel {
  return {
    ...createBaseModel(),
    userName: "",
    password: "",
    toDto(): SignInDto {
      return {
        ...this
      };
    }
  };
}