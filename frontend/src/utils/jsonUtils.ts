interface IJsonUtils {
  parseJson<T>(json: string): T | null;
}

const jsonUtils: IJsonUtils = {
  parseJson<T>(json: string): T | null {
    try {
      const jsonValue: T = JSON.parse(json);
      return jsonValue;
    } catch {
      return null;
    }
  }
};

export function useJsonUtils(): IJsonUtils {
  return jsonUtils;
}