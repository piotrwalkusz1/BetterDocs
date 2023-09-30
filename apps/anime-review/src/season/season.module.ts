import { Module, forwardRef } from "@nestjs/common";
import { AuthModule } from "../auth/auth.module";
import { SeasonModuleBase } from "./base/season.module.base";
import { SeasonService } from "./season.service";
import { SeasonController } from "./season.controller";
import { SeasonResolver } from "./season.resolver";

@Module({
  imports: [SeasonModuleBase, forwardRef(() => AuthModule)],
  controllers: [SeasonController],
  providers: [SeasonService, SeasonResolver],
  exports: [SeasonService],
})
export class SeasonModule {}
