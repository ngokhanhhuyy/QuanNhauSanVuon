import { ValidationError, NotFoundError, OperationError } from "@shared/errors";
import { BaseApiController } from "./baseController";
import { MenuItemService } from "@/services/menuItemService";
export class MenuItemController extends BaseApiController {
    service;
    constructor(request, response) {
        super(request, response);
        this.service = new MenuItemService();
    }
    async list() {
        try {
            const requestDto = this.requestDtoFactory.menuItem.createList(this.request.params);
            const responseDto = await this.service.getListAsync(requestDto);
            return this.ok(responseDto);
        }
        catch (error) {
            if (error instanceof ValidationError) {
                return this.badRequest(error.failures);
            }
            throw error;
        }
    }
    async detail() {
        try {
            const id = parseInt(this.request.params.id);
            if (isNaN(id)) {
                return this.notFound();
            }
            const responseDto = await this.service.getDetailAsync(id);
            return this.ok(responseDto);
        }
        catch (error) {
            if (error instanceof NotFoundError) {
                return this.notFound();
            }
            throw error;
        }
    }
    async create() {
        try {
            const requestDto = this.requestDtoFactory.menuItem.createUpsert(this.request.body);
            const id = await this.service.createAsync(requestDto);
            return this.created(id, process.env.BASE_URL + `/menuItem/${id}`);
        }
        catch (error) {
            if (error instanceof ValidationError) {
                return this.badRequest(error.failures);
            }
            if (error instanceof OperationError) {
                return this.unprocessableEntity(error.failure);
            }
            throw error;
        }
    }
    async update() {
        try {
            const id = parseInt(this.request.params.id);
            if (isNaN(id)) {
                return this.notFound();
            }
            const requestDto = this.requestDtoFactory.menuItem.createUpsert(this.request.body);
            await this.service.updateAsync(id, requestDto);
            return this.ok();
        }
        catch (error) {
            if (error instanceof ValidationError) {
                return this.badRequest(error.failures);
            }
            if (error instanceof OperationError) {
                return this.unprocessableEntity(error.failure);
            }
            throw error;
        }
    }
    async delete() {
        try {
            const id = parseInt(this.request.params.id);
            if (isNaN(id)) {
                throw new NotFoundError();
            }
            await this.service.deleteAsync(id);
            return this.ok();
        }
        catch (error) {
            if (error instanceof NotFoundError) {
                return this.notFound();
            }
            throw error;
        }
    }
    static registryController(app) {
        app.get("/", (request, response) => {
            const controller = new MenuItemController(request, response);
            controller.list();
        });
        app.get("/menuItem/:id", (request, response) => {
            const controller = new MenuItemController(request, response);
            controller.detail();
        });
        app.post("/menuItem", (request, response) => {
            const controller = new MenuItemController(request, response);
            controller.create();
        });
        app.put("/menuItem/:id", (request, response) => {
            const controller = new MenuItemController(request, response);
            controller.update();
        });
        app.delete("/menuItem/:id", (request, response) => {
            const controller = new MenuItemController(request, response);
            controller.delete();
        });
    }
}
