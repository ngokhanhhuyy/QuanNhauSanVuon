import { requestDtoFactory } from "@/dtos";
export class BaseApiController {
    request;
    response;
    requestDtoFactory = requestDtoFactory;
    constructor(request, response) {
        this.request = request;
        this.response = response;
    }
    ok(data) {
        const response = this.response.status(200);
        if (!data) {
            return response;
        }
        return response.json(data);
    }
    created(id, resourceUrl) {
        return this.response.status(201).location(resourceUrl).json(id);
    }
    badRequest(failures) {
        const response = this.response.status(400);
        if (!failures) {
            return response;
        }
        return response.json(failures);
    }
    unauthorized() {
        return this.response.status(401);
    }
    notFound() {
        return this.response.status(404);
    }
    confict() {
        return this.response.status(409);
    }
    unprocessableEntity(operationFailure) {
        const response = this.response.status(422);
        if (!operationFailure) {
            return response;
        }
        return response.json(operationFailure);
    }
}
