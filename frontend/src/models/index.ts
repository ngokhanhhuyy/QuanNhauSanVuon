import * as signInModelFactory from "./signInModels";

const modelFactory = {
  signIn: signInModelFactory
};

export function useModelFactory() {
  return modelFactory;
}