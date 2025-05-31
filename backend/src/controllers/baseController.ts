import type { Request as ExpressRequest, Response as ExpressResponse } from "express";
import type { ValidationFailure, OperationFailure } from "@shared/errors";

export class BaseApiController {
  protected request: ExpressRequest;
  protected response: ExpressResponse;
  
  constructor(request: ExpressRequest, response: ExpressResponse) {
    this.request = request;
    this.response = response;
  }

  protected ok<TData>(data?: TData): ExpressResponse<TData> {
    const response = this.response.status(200);
    if (!data) {
      return response;
    }

    return response.json(data);
  }

  protected badRequest(failures?: ValidationFailure[]): ExpressResponse<ValidationFailure[]> {
    const response = this.response.status(400);
    if (!failures) {
      return response;
    }

    return response.json(failures);
  }

  protected unauthorized(): ExpressResponse<undefined> {
    return this.response.status(401);
  }

  protected notFound(): ExpressResponse<undefined> {
    return this.response.status(404);
  }

  protected confict(): ExpressResponse<undefined> {
    return this.response.status(409);
  }

  protected unprocessableEntity(
      operationFailure?: OperationFailure): ExpressResponse<OperationFailure> {
    const response = this.response.status(422);
    if (!operationFailure) {
      return response;
    }

    return response.json(operationFailure);
  }
}