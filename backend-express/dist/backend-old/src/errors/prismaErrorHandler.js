export function usePrismaClientErrorHandler() {
    return {
        handle(error) {
            const result = {
                violatedField: null,
                violatedEntity: null,
                isNotFoundError: false,
                isUniqueConstraintError: false,
                isNotNullConstraintError: false,
                isForeignKeyConstraintError: false
            };
            switch (error.code) {
                case "P2002":
                    result.isUniqueConstraintError = true;
                    result.violatedEntity = error.meta.modelName;
                    result.violatedField = error.meta.target.split("_")[2];
                    break;
                case "P2003":
                    result.isForeignKeyConstraintError = true;
                case "P2025":
                    result.isNotFoundError = true;
                    result.violatedEntity = error.meta.modelname;
                    break;
            }
            return result;
        }
    };
}
