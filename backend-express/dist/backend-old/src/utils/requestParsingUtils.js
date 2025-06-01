import dot from "dot-object";
const requestParsingUtils = {
    searchParamsToObject(searchParams) {
        const searchParamsAsObject = {};
        for (const [path, value] of searchParams.entries()) {
            searchParamsAsObject[path] = value;
        }
        return dot.object(searchParamsAsObject);
    }
};
export function useRequestParsingUtils() {
    return requestParsingUtils;
}
