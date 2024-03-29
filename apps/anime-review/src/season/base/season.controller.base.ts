/*
------------------------------------------------------------------------------ 
This code was generated by Amplication. 
 
Changes to this file will be lost if the code is regenerated. 

There are other ways to to customize your code, see this doc to learn more
https://docs.amplication.com/how-to/custom-code

------------------------------------------------------------------------------
  */
import * as common from "@nestjs/common";
import * as swagger from "@nestjs/swagger";
import { isRecordNotFoundError } from "../../prisma.util";
import * as errors from "../../errors";
import { Request } from "express";
import { plainToClass } from "class-transformer";
import { ApiNestedQuery } from "../../decorators/api-nested-query.decorator";
import * as nestAccessControl from "nest-access-control";
import * as defaultAuthGuard from "../../auth/defaultAuth.guard";
import { SeasonService } from "../season.service";
import { AclValidateRequestInterceptor } from "../../interceptors/aclValidateRequest.interceptor";
import { AclFilterResponseInterceptor } from "../../interceptors/aclFilterResponse.interceptor";
import { SeasonCreateInput } from "./SeasonCreateInput";
import { SeasonWhereInput } from "./SeasonWhereInput";
import { SeasonWhereUniqueInput } from "./SeasonWhereUniqueInput";
import { SeasonFindManyArgs } from "./SeasonFindManyArgs";
import { SeasonUpdateInput } from "./SeasonUpdateInput";
import { Season } from "./Season";

@swagger.ApiBearerAuth()
@common.UseGuards(defaultAuthGuard.DefaultAuthGuard, nestAccessControl.ACGuard)
export class SeasonControllerBase {
  constructor(
    protected readonly service: SeasonService,
    protected readonly rolesBuilder: nestAccessControl.RolesBuilder
  ) {}
  @common.UseInterceptors(AclValidateRequestInterceptor)
  @common.Post()
  @swagger.ApiCreatedResponse({ type: Season })
  @nestAccessControl.UseRoles({
    resource: "Season",
    action: "create",
    possession: "any",
  })
  @swagger.ApiForbiddenResponse({
    type: errors.ForbiddenException,
  })
  async create(@common.Body() data: SeasonCreateInput): Promise<Season> {
    return await this.service.create({
      data: {
        ...data,

        anime: data.anime
          ? {
              connect: data.anime,
            }
          : undefined,
      },
      select: {
        anime: {
          select: {
            id: true,
          },
        },

        createdAt: true,
        id: true,
        name: true,
        updatedAt: true,
      },
    });
  }

  @common.UseInterceptors(AclFilterResponseInterceptor)
  @common.Get()
  @swagger.ApiOkResponse({ type: [Season] })
  @ApiNestedQuery(SeasonFindManyArgs)
  @nestAccessControl.UseRoles({
    resource: "Season",
    action: "read",
    possession: "any",
  })
  @swagger.ApiForbiddenResponse({
    type: errors.ForbiddenException,
  })
  async findMany(@common.Req() request: Request): Promise<Season[]> {
    const args = plainToClass(SeasonFindManyArgs, request.query);
    return this.service.findMany({
      ...args,
      select: {
        anime: {
          select: {
            id: true,
          },
        },

        createdAt: true,
        id: true,
        name: true,
        updatedAt: true,
      },
    });
  }

  @common.UseInterceptors(AclFilterResponseInterceptor)
  @common.Get("/:id")
  @swagger.ApiOkResponse({ type: Season })
  @swagger.ApiNotFoundResponse({ type: errors.NotFoundException })
  @nestAccessControl.UseRoles({
    resource: "Season",
    action: "read",
    possession: "own",
  })
  @swagger.ApiForbiddenResponse({
    type: errors.ForbiddenException,
  })
  async findOne(
    @common.Param() params: SeasonWhereUniqueInput
  ): Promise<Season | null> {
    const result = await this.service.findOne({
      where: params,
      select: {
        anime: {
          select: {
            id: true,
          },
        },

        createdAt: true,
        id: true,
        name: true,
        updatedAt: true,
      },
    });
    if (result === null) {
      throw new errors.NotFoundException(
        `No resource was found for ${JSON.stringify(params)}`
      );
    }
    return result;
  }

  @common.UseInterceptors(AclValidateRequestInterceptor)
  @common.Patch("/:id")
  @swagger.ApiOkResponse({ type: Season })
  @swagger.ApiNotFoundResponse({ type: errors.NotFoundException })
  @nestAccessControl.UseRoles({
    resource: "Season",
    action: "update",
    possession: "any",
  })
  @swagger.ApiForbiddenResponse({
    type: errors.ForbiddenException,
  })
  async update(
    @common.Param() params: SeasonWhereUniqueInput,
    @common.Body() data: SeasonUpdateInput
  ): Promise<Season | null> {
    try {
      return await this.service.update({
        where: params,
        data: {
          ...data,

          anime: data.anime
            ? {
                connect: data.anime,
              }
            : undefined,
        },
        select: {
          anime: {
            select: {
              id: true,
            },
          },

          createdAt: true,
          id: true,
          name: true,
          updatedAt: true,
        },
      });
    } catch (error) {
      if (isRecordNotFoundError(error)) {
        throw new errors.NotFoundException(
          `No resource was found for ${JSON.stringify(params)}`
        );
      }
      throw error;
    }
  }

  @common.Delete("/:id")
  @swagger.ApiOkResponse({ type: Season })
  @swagger.ApiNotFoundResponse({ type: errors.NotFoundException })
  @nestAccessControl.UseRoles({
    resource: "Season",
    action: "delete",
    possession: "any",
  })
  @swagger.ApiForbiddenResponse({
    type: errors.ForbiddenException,
  })
  async delete(
    @common.Param() params: SeasonWhereUniqueInput
  ): Promise<Season | null> {
    try {
      return await this.service.delete({
        where: params,
        select: {
          anime: {
            select: {
              id: true,
            },
          },

          createdAt: true,
          id: true,
          name: true,
          updatedAt: true,
        },
      });
    } catch (error) {
      if (isRecordNotFoundError(error)) {
        throw new errors.NotFoundException(
          `No resource was found for ${JSON.stringify(params)}`
        );
      }
      throw error;
    }
  }
}
