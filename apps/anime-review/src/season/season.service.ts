import { Injectable } from "@nestjs/common";
import { PrismaService } from "../prisma/prisma.service";
import { SeasonServiceBase } from "./base/season.service.base";

@Injectable()
export class SeasonService extends SeasonServiceBase {
  constructor(protected readonly prisma: PrismaService) {
    super(prisma);
  }
}
