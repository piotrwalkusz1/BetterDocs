import * as common from "@nestjs/common";
import * as swagger from "@nestjs/swagger";
import * as nestAccessControl from "nest-access-control";
import { SeasonService } from "./season.service";
import { SeasonControllerBase } from "./base/season.controller.base";

@swagger.ApiTags("seasons")
@common.Controller("seasons")
export class SeasonController extends SeasonControllerBase {
  constructor(
    protected readonly service: SeasonService,
    @nestAccessControl.InjectRolesBuilder()
    protected readonly rolesBuilder: nestAccessControl.RolesBuilder
  ) {
    super(service, rolesBuilder);
  }
}
